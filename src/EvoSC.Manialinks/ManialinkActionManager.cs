using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks;

public class ManialinkActionManager : IManialinkActionManager
{
    private const BindingFlags ActionMethodBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
    private static readonly char RouteDelimiter = '/';
    private static readonly Regex RouteParamRegex = new("\\{[\\w\\d_]+\\}", RegexOptions.None, TimeSpan.FromMilliseconds(100));
    private static readonly Regex ValidCharactersInRouteNameRegex = new("[_\\.\\w\\d]+", RegexOptions.None, TimeSpan.FromMilliseconds(100));
    private static readonly string ControllerPostfix = "Controller";
    private static readonly string AsyncMethodPostfix = "Async";
    
    private readonly ILogger<ManialinkActionManager> _logger;
    private readonly IMlRouteNode _rootNode = new MlRouteNode("<root>"){Children = new Dictionary<string, IMlRouteNode>()};
    private readonly object _rootNodeMutex = new();
    private readonly Dictionary<Type, List<string>> _controllerRoutes = new();
    private readonly object _controllerRoutesMutex = new();

    public ManialinkActionManager(ILogger<ManialinkActionManager> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get the effective route to a controller.
    /// </summary>
    /// <param name="type">The type of the controller class.</param>
    /// <returns></returns>
    private ManialinkRouteAttribute GetControllerRoute(Type type)
    {
        var routeAttr = type.GetCustomAttribute<ManialinkRouteAttribute>();

        if (routeAttr?.Route != null)
        {
            return routeAttr;
        }
        
        var name = type.Name;

        if (name.EndsWith(ControllerPostfix, StringComparison.OrdinalIgnoreCase))
        {
            // return the name of the controller class without the "Controller" postfix
            return new ManialinkRouteAttribute {Route = name[..^ControllerPostfix.Length], Permission = routeAttr?.Permission};
        }

        return new ManialinkRouteAttribute {Route = name, Permission = routeAttr?.Permission};
    }

    /// <summary>
    /// Get the permission name from a string or enum identifier.
    /// </summary>
    /// <param name="permission">The permission to get the name of.</param>
    /// <returns></returns>
    private string? GetPermissionName(object? permission)
    {
        if (permission == null)
        {
            return null;
        }

        if (permission is Enum)
        {
            return permission.AsEnum().GetIdentifier();
        }

        return permission.ToString();
    }
    
    /// <summary>
    /// Get the effective route of a method.
    /// </summary>
    /// <param name="method">The method to get the route from.</param>
    /// <param name="firstParameter">Pointer to the first parameter.</param>
    /// <returns></returns>
    private ManialinkRouteAttribute GetMethodRoute(MethodInfo method, IMlActionParameter? firstParameter)
    {
        var routeAttr = method.GetCustomAttribute<ManialinkRouteAttribute>();

        if (routeAttr?.Route != null)
        {
            return routeAttr;
        }
        
        var name = method.Name;

        if (name.EndsWith(AsyncMethodPostfix, StringComparison.OrdinalIgnoreCase))
        {
            // get the name of the method without the "Async" postfix
            name = name[..^AsyncMethodPostfix.Length];
        }

        var route = new StringBuilder(name);

        // add route parameters from the method parameters
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

        return new ManialinkRouteAttribute {Route = route.ToString(), Permission = routeAttr?.Permission};
    }

    /// <summary>
    /// Build an info object for a parameter from an action.
    /// </summary>
    /// <param name="parInfo">The parameter to build the info object from.</param>
    /// <returns></returns>
    private IMlActionParameter GetActionParameter(ParameterInfo parInfo)
    {
        var entryModelAttr = parInfo.ParameterType.GetCustomAttribute<FormEntryModelAttribute>();

        return new MlActionParameter
        {
            ParameterInfo = parInfo, 
            IsEntryModel = entryModelAttr != null,
            NextParameter = null
        };
    }
    
    /// <summary>
    /// Get all the action parameters from a method.
    /// </summary>
    /// <param name="method">The method to get the parameters from.</param>
    /// <returns></returns>
    private IMlActionParameter? GetActionParameters(MethodInfo method)
    {
        var methodParams = method.GetParameters();

        var first = methodParams.FirstOrDefault();

        if (first == null)
        {
            // method contains no parameters
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

    /// <summary>
    /// Build the full route for an action.
    /// </summary>
    /// <param name="controllerRoute">Route to the controller which the action callback is part of.</param>
    /// <param name="method">The method used as a callback for the action.</param>
    /// <param name="actionParameters">Pointer to the first parameter for this action.</param>
    /// <returns></returns>
    private (string, ManialinkRouteAttribute) BuildActionRoute(string controllerRoute, MethodInfo method, IMlActionParameter? actionParameters)
    {
        var route = new StringBuilder(controllerRoute);
        var routeInfo = GetMethodRoute(method, actionParameters);
        var methodRoute = routeInfo.Route;

        if (methodRoute.StartsWith(RouteDelimiter))
        {
            // use the method route as the root route
            route.Clear();
            methodRoute = methodRoute[1..];
        }
        else if (!string.IsNullOrEmpty(controllerRoute))
        {
            route.Append(RouteDelimiter);
        }

        route.Append(methodRoute);
        return (route.ToString(), routeInfo);
    }
    
    /// <summary>
    /// Validate the parameter for a route.
    /// </summary>
    /// <param name="route">The route which the parameter is part of.</param>
    /// <param name="routeComponent">The route component of this parameter.</param>
    /// <param name="currentActionParameter">The parameter to validate.</param>
    /// <param name="nextNode">The next node in the parameter list.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static IMlActionParameter? CheckParameterValidity(string route, string routeComponent,
        IMlActionParameter? currentActionParameter, MlRouteNode nextNode)
    {
        if (RouteParamRegex.IsMatch(routeComponent))
        {
            if (currentActionParameter == null)
            {
                throw new InvalidOperationException(
                    $"Action method missing parameter for route parameter '{routeComponent}'.");
            }

            nextNode.IsParameter = true;
            return currentActionParameter.NextParameter;
        }
        else if (!ValidCharactersInRouteNameRegex.IsMatch(routeComponent))
        {
            throw new InvalidOperationException(
                $"The route '{route}' contains the invalid component '{routeComponent}'.");
        }

        return currentActionParameter;
    }

    /// <summary>
    /// Main internal method for registering an action to the manager.
    /// </summary>
    /// <param name="route">The route to register.</param>
    /// <param name="action">The action to be called for the specified route.</param>
    /// <exception cref="InvalidOperationException"></exception>
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
            
                // make sure we create the child dict to avoid null references
                if (currentNode.Children == null)
                {
                    currentNode.Children = new Dictionary<string, IMlRouteNode>();
                }
            
                // try to find the next component in the route, if already registered
                if (currentNode.Children.TryGetValue(routeComponent, out var child))
                {
                    currentNode = child;
                }
                else
                {
                    // the current path is completely new, so we just register it
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

    /// <summary>
    /// Main internal method for unregistering a route.
    /// </summary>
    /// <param name="route">Current route component in the recursive call.</param>
    /// <param name="currentNode">Current node in the recursive call.</param>
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

        // make sure we don't remove the child node if there are more than 1 children
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

    /// <summary>
    /// Recursively search for an action with the given route components.
    /// </summary>
    /// <param name="nextComponents">Next components in the route to search for.</param>
    /// <param name="previousComponent">The previous component from the recursive call.</param>
    /// <param name="currentNode">The current working route node.</param>
    /// <returns></returns>
    private (IManialinkAction?, IMlRouteNode?) FindActionInternal(string[] nextComponents, string previousComponent, IMlRouteNode currentNode)
    {
        if (nextComponents.Length == 0 || currentNode.Children == null)
        {
            if (nextComponents.Length > 0)
            {
                // reached end of current branch, but did not find the action
                return (null, null);
            }
            
            // reached end of current branch and found an action
            return (currentNode.Action,
                new MlRouteNode(previousComponent) {Action = currentNode.Action, IsParameter = currentNode.IsParameter});
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
                // found a branch to the route action
                pathNode.Children[nextNode.Name] = nextNode;
            }

            if (manialinkAction != null)
            {
                // found the corresponding action for the route, so let's just return here
                return (manialinkAction, pathNode);
            }
        }

        // no action found for current route
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
                    // action not found, try next root node
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
            throw new InvalidOperationException(
                $"The provided type {controllerType.Name} is not a manialink controller class.");
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

            var (route, routeInfo) = BuildActionRoute(controllerRoute.Route, method, firstActionParameter);

            if (string.IsNullOrEmpty(route))
            {
                throw new InvalidOperationException(
                    $"Route is empty for method {method.Name} in controller {controllerType.Name}.");
            }

            var action = new ManialinkAction
            {
                Permission = GetPermissionName(routeInfo.Permission ?? controllerRoute.Permission),
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
