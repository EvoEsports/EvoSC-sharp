using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using LinqToDB.Extensions;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks;

public class ManialinkActionManager : IManialinkActionManager
{
    private const BindingFlags ActionMethodBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
    private static readonly char RouteDelimiter = '/';
    private static readonly Regex RouteParamRegex = new("\\{[\\w\\d_]+\\}", RegexOptions.None, TimeSpan.FromMilliseconds(100));
    private static readonly Regex ValidCharactersInRouteNameRegex = new("[_\\.\\w\\d]+", RegexOptions.None, TimeSpan.FromMilliseconds(100));
    
    private readonly ILogger<ManialinkActionManager> _logger;
    private readonly IMlRouteNode _rootNode = new MlRouteNode("<root>"){Children = new Dictionary<string, IMlRouteNode>()};
    private readonly object _rootNodeMutex = new();
    private readonly Dictionary<Type, List<string>> _controllerRoutes = new();
    private readonly object _controllerRoutesMutex = new();

    public ManialinkActionManager(ILogger<ManialinkActionManager> logger)
    {
        _logger = logger;
    }

    private string GetControllerRoute(Type type)
    {
        var routeAttr = type.GetCustomAttribute<ManialinkRouteAttribute>();

        if (routeAttr != null)
        {
            return routeAttr.Route;
        }
        
        var name = type.Name;

        if (name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
        {
            return name[..^10];
        }

        return name;
    }

    private string GetMethodRoute(MethodInfo method, IMlActionParameter firstParameter)
    {
        var routeAttr = method.GetCustomAttribute<ManialinkRouteAttribute>();

        if (routeAttr != null)
        {
            return routeAttr.Route;
        }
        
        var name = method.Name;

        if (name.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
        {
            name = name[..^5];
        }

        var route = new StringBuilder(name);

        var currentParam = firstParameter;

        while (currentParam != null)
        {
            if (!currentParam.IsEntryModel)
            {
                route.Append(RouteDelimiter);
                route.Append('{');
                route.Append(currentParam.Name);
                route.Append('}');
            }

            currentParam = currentParam.NextParameter;
        }

        return route.ToString();
    }

    private IMlActionParameter GetActionParameter(ParameterInfo parInfo)
    {
        var entryModelAttr = parInfo.ParameterType.GetCustomAttribute<EntryModelAttribute>();

        return new MlActionParameter
        {
            ParameterInfo = parInfo, 
            IsEntryModel = entryModelAttr != null,
            NextParameter = null
        };
    }
    
    private IMlActionParameter? GetActionParameters(MethodInfo method)
    {
        var methodParams = method.GetParameters();

        var first = methodParams.FirstOrDefault();

        if (first == null)
        {
            return null;
        }

        var firstParam = GetActionParameter(first);
        var currentParam = firstParam;

        foreach (var methodParam in methodParams[1..])
        {
            var nextParam = GetActionParameter(methodParam);
            currentParam.NextParameter = nextParam;
            currentParam = nextParam;
        }

        return firstParam;
    }

    private string BuildActionRoute(string controllerRoute, MethodInfo method, IMlActionParameter actionParameters)
    {
        var route = new StringBuilder(controllerRoute);
        var methodRoute = GetMethodRoute(method, actionParameters);

        if (methodRoute.StartsWith(RouteDelimiter))
        {
            route.Clear();
            methodRoute = methodRoute[1..];
        }
        else if (!string.IsNullOrEmpty(controllerRoute))
        {
            route.Append(RouteDelimiter);
        }

        route.Append(methodRoute);
        return route.ToString();
    }
    
    private static IMlActionParameter? CheckParameterValidity(string route, string routeComponent,
        IMlActionParameter? currentActionParameter, MlRouteNode newNode)
    {
        if (RouteParamRegex.IsMatch(routeComponent))
        {
            if (currentActionParameter == null)
            {
                throw new InvalidOperationException(
                    $"Action method missing parameter for route parameter '{routeComponent}'.");
            }

            newNode.IsParameter = true;
            return currentActionParameter.NextParameter;
        }
        else if (!ValidCharactersInRouteNameRegex.IsMatch(routeComponent))
        {
            throw new InvalidOperationException(
                $"The route '{route}' contains the invalid component '{routeComponent}'.");
        }

        return currentActionParameter;
    }

    private void AddActionInternal(string route, IManialinkAction action)
    {
        if (route.StartsWith(RouteDelimiter))
        {
            throw new InvalidOperationException($"Routes cannot begin with the route delimiter '{RouteDelimiter}'.");
        }

        lock (_rootNodeMutex)
        {
            var currentNode = _rootNode;
            var routeComponents = route.Split(RouteDelimiter);
            var currentActionParameter = action.FirstParameter;

            foreach (var routeComponent in routeComponents)
            {
                if (string.IsNullOrEmpty(routeComponent))
                {
                    throw new InvalidOperationException($"Route '{route}' contains an empty component.");
                }
            
                if (currentNode.Children == null)
                {
                    currentNode.Children = new Dictionary<string, IMlRouteNode>();
                }
            
                if (currentNode.Children.TryGetValue(routeComponent, out var child))
                {
                    currentNode = child;
                }
                else
                {
                    var newNode = new MlRouteNode(routeComponent);

                    currentActionParameter = CheckParameterValidity(route, routeComponent, currentActionParameter, newNode);

                    currentNode.Children[routeComponent] = newNode;
                    currentNode = newNode;
                }
            }

            if (currentNode.IsAction)
            {
                throw new InvalidOperationException($"An action already exists for the route '{route}'.");
            }

            currentNode.Action = action;
        }
        
        _logger.LogDebug("Registered manialink route: {Route}", route);
    }

    public void AddRoute(string route, IManialinkAction action) => AddActionInternal(route, action);

    private void RemoveRouteInternal(IMlRouteNode route, IMlRouteNode currentNode)
    {
        if (route.IsAction || route.Children == null || route.Children.Count == 0)
        {
            return;
        }

        var nextNode = currentNode?.Children?[route.Name];

        if (nextNode != null)
        {
            RemoveRouteInternal(route.Children.Values.First(), nextNode); 
        }

        if (currentNode?.Children is {Count: 0 or 1})
        {
            currentNode.Children.Remove(route.Name);
        }
    }
    
    public void RemoveRoute(string route)
    {
        lock (_rootNodeMutex)
        {
            var (_, path) = FindAction(route);
            RemoveRouteInternal(path, _rootNode);

            if (path.Children is {Count: 0 or 1})
            {
                _rootNode.Children?.Remove(path.Name);
            }
        }
    }

    private (IManialinkAction?, IMlRouteNode?) FindActionInternal(string[] nextComponents, string lastComponent, IMlRouteNode currentNode)
    {
        if (nextComponents.Length == 0 || currentNode.Children == null)
        {
            if (nextComponents.Length > 0)
            {
                return (null, null);
            }
            
            return (currentNode.Action,
                new MlRouteNode(lastComponent) {Action = currentNode.Action, IsParameter = currentNode.IsParameter});
        }

        var currentComponent = nextComponents.First();
        var pathNode = new MlRouteNode(currentComponent)
        {
            Children = new Dictionary<string, IMlRouteNode>(),
            IsParameter = currentNode.IsParameter
        };

        foreach (var child in currentNode.Children.Values)
        {
            if (!child.IsParameter && !child.Name.Equals(currentComponent, StringComparison.Ordinal))
            {
                continue;
            }

            var (manialinkAction, nextNode) = FindActionInternal(nextComponents[1..], currentComponent, child);

            if (nextNode != null)
            {
                pathNode.Children[nextNode.Name] = nextNode;
            }

            if (manialinkAction != null)
            {
                return (manialinkAction, pathNode);
            }
        }

        return (null, null);
    }
    
    public (IManialinkAction, IMlRouteNode) FindAction(string action)
    {
        var routeComponents = action.Split(RouteDelimiter);

        lock (_rootNodeMutex)
        {
            foreach (var child in _rootNode.Children.Values)
            {
                var (manialinkAction, path) = FindActionInternal(routeComponents[1..], routeComponents[0], child);

                if (manialinkAction == null || path == null)
                {
                    continue;
                }
            
                var pathNode = new MlRouteNode(routeComponents[0])
                {
                    Children = new Dictionary<string, IMlRouteNode>(),
                    IsParameter = path.IsParameter
                };
            
                pathNode.Children.Add(path.Name, path);

                return (manialinkAction, pathNode);
            }
        }

        throw new InvalidOperationException($"No manialink route matches '{action}'.");
    }

    public void RegisterForController(Type controllerType)
    {
        if (!controllerType.IsControllerClass())
        {
            throw new InvalidOperationException($"The provided type {controllerType.Name} is not a controller class.");
        }

        if (!typeof(ManialinkController).IsAssignableFrom(controllerType))
        {
            return;
        }

        lock (_controllerRoutesMutex)
        {
            if (!_controllerRoutes.ContainsKey(controllerType))
            {
                _controllerRoutes[controllerType] = new List<string>();
            }
        }
        
        var methods = controllerType.GetMethods(ActionMethodBindingFlags);
        var controllerRoute = GetControllerRoute(controllerType);

        foreach (var method in methods)
        {
            var firstActionParameter = GetActionParameters(method);

            // build route
            var route = BuildActionRoute(controllerRoute, method, firstActionParameter);

            if (string.IsNullOrEmpty(route))
            {
                throw new InvalidOperationException(
                    $"Route is empty for method {method.Name} in controller {controllerType.Name}.");
            }

            var action = new ManialinkAction
            {
                Permission = null,
                ControllerType = controllerType,
                HandlerMethod = method,
                FirstParameter = firstActionParameter
            };

            AddActionInternal(route, action);

            lock (_controllerRoutesMutex)
            {
                _controllerRoutes[controllerType].Add(route);
            }
        }
    }

    public void UnregisterForController(Type controllerType)
    {
        lock (_controllerRoutesMutex)
        {
            if (!_controllerRoutes.ContainsKey(controllerType))
            {
                return;
            }

            foreach (var route in _controllerRoutes[controllerType])
            {
                RemoveRoute(route);
            }
        }
    }
}
