using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks.Tests;

public class ManialinkActionManagerTests
{
    private readonly ILogger<ManialinkActionManager> _logger;
    
    public ManialinkActionManagerTests()
    {
        
        _logger = LoggerFactory.Create(c => { }).CreateLogger<ManialinkActionManager>();
    }

    [Theory]
    [InlineData("a/b", "a/b", 0, null)]
    [InlineData("a/b", "a/c", 0, typeof(InvalidOperationException))]
    [InlineData("a/b", "a/b/c", 0, typeof(InvalidOperationException))]
    [InlineData("a/b/c", "a/b", 0, typeof(InvalidOperationException))]
    [InlineData("a/{b}", "a/b", 1, null)]
    [InlineData("a/{b}", "a/32456", 1, null)]
    [InlineData("", "", 0, typeof(InvalidOperationException))]
    [InlineData("/", "", 0, typeof(InvalidOperationException))]
    [InlineData("/a/b", "a/b", 0, typeof(InvalidOperationException))]
    [InlineData("a/b/{c}/{d}/{e}", "a/b/325/6524/2547", 3, null)]
    [InlineData("a/{b}/c/{d}", "a/463/c/436", 2, null)]
    [InlineData("a/{b}/c/{d}", "a/463/34534/436", 2, typeof(InvalidOperationException))]
    [InlineData("a", "a", 0, null)]
    [InlineData("_", "_", 0, null)]
    [InlineData(".", ".", 0, null)]
    [InlineData("-", "-", 0, typeof(InvalidOperationException))]
    public void Test_Add_Routes(string route, string request, int paramCount, Type? exceptionType)
    {
        IManialinkActionManager manager = new ManialinkActionManager(_logger);
        var action = new ManialinkAction
        {
            Permission = null, ControllerType = null, HandlerMethod = null, FirstParameter = null
        };

        if (paramCount > 0)
        {
            action.FirstParameter = new MlActionParameter {ParameterInfo = null, NextParameter = null};
            var currentParam = action.FirstParameter;

            for (var i = 1; i < paramCount; i++)
            {
                currentParam.NextParameter = new MlActionParameter {ParameterInfo = null, NextParameter = null};
                currentParam = currentParam.NextParameter;
            }
        }

        if (exceptionType != null)
        {
            Assert.Throws(exceptionType, () =>
            {
                manager.AddRoute(route, action);
                manager.FindAction(request);
            });
        }
        else
        {
            manager.AddRoute(route, action);
            var (foundAction, _) = manager.FindAction(request);
            Assert.Equal(action, foundAction);
        }
    }

    [Fact]
    public void Test_Equal_Length_Routes_With_Different_Group_Names_Is_Allowed()
    {
        IManialinkActionManager manager = new ManialinkActionManager(_logger);
        var action1 = new ManialinkAction
        {
            Permission = "p1", ControllerType = null, HandlerMethod = null, FirstParameter = null
        };
        
        var action2 = new ManialinkAction
        {
            Permission = "p2", ControllerType = null, HandlerMethod = null, FirstParameter = null
        };
        
        var route1 = "a/b/c";
        var route2 = "a/b/d";
        
        manager.AddRoute(route1, action1);
        manager.AddRoute(route2, action2);

        var (foundAction1, _) = manager.FindAction(route1);
        var (foundAction2, _) = manager.FindAction(route2);
        
        Assert.Equal("p1", foundAction1.Permission);
        Assert.Equal("p2", foundAction2.Permission);
    }

    [Fact]
    public void Test_Two_Routes_For_Different_Actions()
    {
        IManialinkActionManager manager = new ManialinkActionManager(_logger);
        var action1 = new ManialinkAction
        {
            Permission = "p1", ControllerType = null, HandlerMethod = null, FirstParameter = null
        };
        
        var action2 = new ManialinkAction
        {
            Permission = "p2", ControllerType = null, HandlerMethod = null, FirstParameter = null
        };

        var route = "a/b/c";

        Assert.Throws<InvalidOperationException>(() =>
        {
            manager.AddRoute(route, action1);
            manager.AddRoute(route, action2);
        });
    }

    [Fact]
    public void Test_Simple_And_Parameterized_Routes_Conflicting()
    {
        IManialinkActionManager manager = new ManialinkActionManager(_logger);
        var action = new ManialinkAction
        {
            Permission = "p1", ControllerType = null, HandlerMethod = null, FirstParameter = null
        };

        var route1 = "a/b";
        var route2 = "a/{b}";

        Assert.Throws<InvalidOperationException>(() =>
        {
            manager.AddRoute(route1, action);
            manager.AddRoute(route2, action);
        });
    }

    [Fact]
    public void Test_Simple_Route_Removed()
    {
        IManialinkActionManager manager = new ManialinkActionManager(_logger);
        var action = new ManialinkAction
        {
            Permission = null, ControllerType = null, HandlerMethod = null, FirstParameter = null
        };
        var route = "a/b/c";
        
        manager.AddRoute(route, action);

        var (foundAction, _) = manager.FindAction(route);
        Assert.Equal(foundAction, action);
        
        manager.RemoveRoute(route);

        Assert.Throws<InvalidOperationException>(() =>
        {
            manager.FindAction(route);
        });
    }
}
