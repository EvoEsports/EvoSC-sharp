using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace EvoSC.Manialinks;

public class ManialinkActionManager : IManialinkActionManager
{
    private const BindingFlags ActionMethodBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
    private static char RouteDelimiter = '/';
    
    private readonly ILogger<ManialinkActionManager> _logger;
    private readonly IMlRouteNode _rootNode = new MlRouteNode("<root>");

    public ManialinkActionManager(ILogger<ManialinkActionManager> logger)
    {
        _logger = logger;
    }

    private string GetControllerRoute(Type type)
    {
        var name = type.Name;

        if (name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
        {
            return name[0..^10];
        }

        return name;
    }

    private string GetMethodRoute(MethodInfo method, IMlActionParameter firstParameter)
    {
        var name = method.Name;

        if (name.EndsWith("Async", StringComparison.OrdinalIgnoreCase))
        {
            name = name[0..^5];
        }

        var route = new StringBuilder(name);

        var currentParam = firstParameter;

        while (currentParam != null)
        {
            route.Append(RouteDelimiter);
            route.Append('{');
            route.Append(currentParam.Name);
            route.Append('}');
            
            currentParam = currentParam.NextParameter;
        }

        return route.ToString();
    }

    private IMlActionParameter? GetActionParameters(MethodInfo method)
    {
        var methodParams = method.GetParameters();

        var first = methodParams.FirstOrDefault();

        if (first == null)
        {
            return null;
        }

        var firstParam = new MlActionParameter {ParameterInfo = first, NextParameter = null};
        var currentParam = firstParam;

        foreach (var methodParam in methodParams[1..])
        {
            var nextParam = new MlActionParameter {ParameterInfo = methodParam};
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
        if (Regex.IsMatch(routeComponent, "\\{[\\w\\d_]+\\}"))
        {
            if (currentActionParameter == null)
            {
                throw new InvalidOperationException(
                    $"Route is expecting a parameter '{routeComponent}', but the action method does not have any for this one.");
            }

            newNode.IsParameter = true;
            return currentActionParameter.NextParameter;
        }
        else if (!Regex.IsMatch(routeComponent, "[_\\.\\w\\d]+"))
        {
            throw new InvalidOperationException(
                $"The route '{route}' contains the invalid component '{routeComponent}'.");
        }

        return currentActionParameter;
    }

    private void AddAction(string route, IManialinkAction action)
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
        _logger.LogDebug("Registered manialink route: {Route}", route);
    }

    public void AddActions(Type controllerType)
    {
        if (!controllerType.IsControllerClass())
        {
            throw new InvalidOperationException($"The provided type {controllerType.Name} is not a controller class.");
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

            AddAction(route, action);
        }
    }

    public IManialinkAction FindAction(string action)
    {
        return new ManialinkAction
        {
            Permission = null,
            ControllerType = null,
            HandlerMethod = null,
            FirstParameter = null
        };
    }
}
