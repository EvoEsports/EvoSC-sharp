using System;
using System.Threading.Tasks;
using EvoSC.Common.Application;
using EvoSC.Common.Application.Exceptions;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Tests.Application.TestObjects;
using Moq;
using SimpleInjector;
using Xunit;

namespace EvoSC.Common.Tests.Application;

public class StartupPipelineTests
{
    private Mock<IEvoScBaseConfig> _config;

    public StartupPipelineTests()
    {
        var loggingMock = new Mock<ILoggingConfig>();
        loggingMock.Setup(l => l.LogLevel).Returns("info");
        _config = new Mock<IEvoScBaseConfig>();
        _config.Setup(c => c.Logging).Returns(loggingMock.Object);
    }
    
    [Fact]
    public async Task Simple_Service_Is_Set_Up()
    {
        var startup = new StartupPipeline(_config.Object);
        startup.Services("TestService", s => s
            .Register<IMyService, MyService>()
        );

        await startup.ExecuteAsync("TestService");

        var service = startup.ServiceContainer.GetInstance<IMyService>();
        Assert.NotNull(service);
    }

    [Fact]
    public async Task Dependency_Is_Also_Executed()
    {
        var startup = new StartupPipeline(_config.Object);
        startup.Services("TestService", s => s
            .Register<IMyService, MyService>()
        );
        startup.Services("TestService2", s => s
                .Register<IMyService2, MyService2>()
            , "TestService");

        await startup.ExecuteAsync("TestService2");
        
        var service = startup.ServiceContainer.GetInstance<IMyService>();
        Assert.NotNull(service);
    }
    
    [Fact]
    public async Task Only_Specified_Component_Is_Executed()
    {
        var startup = new StartupPipeline(_config.Object);
        startup.Services("TestService", s => s
            .Register<IMyService, MyService>()
        );
        startup.Services("TestService2", s => s
                .Register<IMyService2, MyService2>()
        );

        await startup.ExecuteAsync("TestService");
        
        var service = startup.ServiceContainer.GetInstance<IMyService>();

        Assert.NotNull(service);
        Assert.Throws<ActivationException>(() => startup.ServiceContainer.GetInstance<IMyService2>());
    }

    [Fact]
    public async Task All_Components_Are_Executed()
    {
        var startup = new StartupPipeline(_config.Object);
        var actionExecuted = false;
        var asyncActionExecuted = true;
        
        startup.Services("TestService", s => s.Register<IMyService, MyService>());
        startup.Action("MyAction", s => actionExecuted = true);
        startup.ActionAsync("MyAsyncAction", s =>
        {
            asyncActionExecuted = true;
            return Task.CompletedTask;
        });

        await startup.ExecuteAllAsync();

        var service = startup.ServiceContainer.GetInstance<IMyService>();
        
        Assert.NotNull(service);
        Assert.True(actionExecuted);
        Assert.True(asyncActionExecuted);
    }

    [Fact]
    public async Task Dependency_Cycle_Detected_Throws_Exception()
    {
        var startup = new StartupPipeline(_config.Object);
        startup.Services("TestService", s => s
            .Register<IMyService, MyService>()
        , "TestService2");
        startup.Services("TestService2", s => s
            .Register<IMyService2, MyService2>()
        , "TestService");

        await Assert.ThrowsAsync<StartupDependencyCycleException>(() => startup.ExecuteAsync("TestService"));
    }

    [Fact]
    public async Task Startup_Component_Not_Existing_Throws_Exception()
    {
        var startup = new StartupPipeline(_config.Object);
        await Assert.ThrowsAsync<StartupPipelineException>(() => startup.ExecuteAsync("InvalidComponent"));
    }
}
