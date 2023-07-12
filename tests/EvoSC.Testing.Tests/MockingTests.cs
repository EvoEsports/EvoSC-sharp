using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Testing.Tests.TestClasses;
using Moq;

namespace EvoSC.Testing.Tests;

public class MockingTests
{
    [Fact]
    public void NewControllerContextMock_Returns_Correct_Mock()
    {
        var mock = Mocking.NewControllerContextMock<IControllerContext>();
        
        Assert.NotNull(mock.Context);
        Assert.NotNull(mock.AuditEventBuilder);
        Assert.NotNull(mock.AuditService);
        Assert.NotNull(mock.ContextService);
        Assert.Equal(mock.AuditEventBuilder.Object, mock.Context.Object.AuditEvent);
        Assert.Equal(mock.Context.Object, mock.ContextService.Object.GetContext());
        Assert.Equal(mock.AuditEventBuilder.Object, mock.ContextService.Object.Audit());
    }

    [Fact]
    public void SetupMock_For_PlayerInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<IPlayerInteractionContext>();
        var player = new Mock<IOnlinePlayer>();
        mock.SetupMock(player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }

    [Fact]
    public void NewPlayerInteractionContextMock_Returns_Correct_Mock()
    {
        var player = new Mock<IOnlinePlayer>();
        var mock = Mocking.NewPlayerInteractionContextMock(player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }

    [Fact]
    public void SetupMock_For_CommandInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<ICommandInteractionContext>();
        var player = new Mock<IOnlinePlayer>();
        mock.SetupMock(player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }
    
    [Fact]
    public void NewCommandInteractionContextMock_Returns_Correct_Mock()
    {
        var player = new Mock<IOnlinePlayer>();
        var mock = Mocking.NewCommandInteractionContextMock(player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }
    
    
    [Fact]
    public void SetupMock_For_ManialinkInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<IManialinkInteractionContext>();
        var player = new Mock<IOnlinePlayer>();
        var mlActionContext = new Mock<IManialinkActionContext>();
        var mlManager = new Mock<IManialinkManager>();
        mock.SetupMock(player.Object, mlActionContext.Object, mlManager.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        Assert.Equal(mlActionContext.Object, mock.Context.Object.ManialinkAction);
        Assert.Equal(mlManager.Object, mock.Context.Object.ManialinkManager);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }
    
    [Fact]
    public void NewManialinkInteractionContextMock_Returns_Correct_Mock()
    {
        var player = new Mock<IOnlinePlayer>();
        var mlActionContext = new Mock<IManialinkActionContext>();
        var mlManager = new Mock<IManialinkManager>();
        var mock = Mocking.NewManialinkInteractionContextMock(player.Object, mlActionContext.Object, mlManager.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        Assert.Equal(mlActionContext.Object, mock.Context.Object.ManialinkAction);
        Assert.Equal(mlManager.Object, mock.Context.Object.ManialinkManager);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }

    [Fact]
    public async Task New_Controller_Mock_Returns_Mocked_Controller_Instance()
    {
        var contextMock = Mocking.NewControllerContextMock<IPlayerInteractionContext>();
        var controller = Mocking.NewControllerMock<TestController, IPlayerInteractionContext>(contextMock);

        await controller.DoingSomething();
        
        contextMock.AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }

    [Fact]
    public async Task New_Individual_Controller_And_Context_Mocks()
    {
        var mock = Mocking.NewControllerMock<TestController, IPlayerInteractionContext>();

        await mock.Controller.DoingSomething();
        
        mock.ContextMock.AuditEventBuilder.Verify(m => m.Success(), Times.Once);
    }

    [Fact]
    public async Task New_Controller_Mock_Passes_Ctor_Services()
    {
        var serviceMock = new Mock<ITestService>();
        var mock = Mocking.NewControllerMock<TestControllerWithServices, IPlayerInteractionContext>(serviceMock);

        await mock.Controller.DoSomething();
        
        serviceMock.Verify(m => m.DoSomethingElse(), Times.Once);
    }

    [Fact]
    public void NewContextServiceMock_Sets_Up_Correctly()
    {
        var contextMock = Mocking.NewControllerContextMock<IControllerContext>();
        var mock = Mocking.NewContextServiceMock(contextMock.Context.Object, null);
        
        Assert.Equal(contextMock.AuditEventBuilder.Object, mock.Object.Audit());
        Assert.Equal(contextMock.Context.Object, mock.Object.GetContext());
        contextMock.AuditEventBuilder.Verify(m => m.CausedBy(It.IsAny<IOnlinePlayer>()), Times.Never);
    }

    [Fact]
    public void NewContextServiceMock_Sets_Up_Correctly_With_Actor()
    {
        var actor = new Mock<IOnlinePlayer>();
        var contextMock = Mocking.NewControllerContextMock<IControllerContext>();
        var mock = Mocking.NewContextServiceMock(contextMock.Context.Object, actor.Object);
        
        Assert.Equal(contextMock.AuditEventBuilder.Object, mock.Object.Audit());
        Assert.Equal(contextMock.Context.Object, mock.Object.GetContext());
        contextMock.AuditEventBuilder.Verify(m => m.CausedBy(actor.Object), Times.Once);
    }
    
}
