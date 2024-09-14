using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Audit;
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
        var serverClient = Mocking.NewServerClientMock();
        var player = new Mock<IOnlinePlayer>();
        mock.SetupMock(serverClient.Client, player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }

    [Fact]
    public void NewPlayerInteractionContextMock_Returns_Correct_Mock()
    {
        var serverClient = Mocking.NewServerClientMock();
        var player = new Mock<IOnlinePlayer>();
        var mock = Mocking.NewPlayerInteractionContextMock(serverClient.Client, player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }

    [Fact]
    public void SetupMock_For_CommandInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<ICommandInteractionContext>();
        var serverClient = Mocking.NewServerClientMock();
        var player = new Mock<IOnlinePlayer>();
        mock.SetupMock(serverClient.Client, player.Object);
        
        Assert.Equal(player.Object, mock.Context.Object.Player);
        mock.AuditEventBuilder.Verify(m => m.CausedBy(player.Object), Times.Once);
    }
    
    [Fact]
    public void NewCommandInteractionContextMock_Returns_Correct_Mock()
    {
        var server = Mocking.NewServerClientMock();
        var player = new Mock<IOnlinePlayer>();
        var mock = Mocking.NewCommandInteractionContextMock(server.Client, player.Object);
        
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
        var server = Mocking.NewServerClientMock();
        mock.SetupMock(server.Client, player.Object, mlActionContext.Object, mlManager.Object);
        
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
        var server = Mocking.NewServerClientMock();
        var mock = Mocking.NewManialinkInteractionContextMock(server.Client, player.Object, mlActionContext.Object, mlManager.Object);
        
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

    [Fact]
    public void NewLocaleMock_Returns_Simplified_Mock()
    {
        var context = Mocking.NewControllerContextMock<IPlayerInteractionContext>();
        var locale = Mocking.NewLocaleMock(context.ContextService.Object);

        var str = locale["SomeRandomLocale"];
        
        Assert.Equal("Test_Locale_String", str);
    }

    [Fact]
    public void NewServerClientMock_Returns_Mocked_Client()
    {
        var server = Mocking.NewServerClientMock();
        
        Assert.NotNull(server.Remote);
        Assert.Equal(server.Remote.Object, server.Client.Object.Remote);
    }

    [Fact]
    public void NewAuditEventBuilderMock_Returns_Mock_With_No_Null_Methods()
    {
        var player = new Mock<IOnlinePlayer>();
        var mock = Mocking.NewAuditEventBuilderMock();
        
        Assert.NotNull(mock.Object.WithEventName(""));
        Assert.NotNull(mock.Object.WithEventName(TestEnum.TestField));
        Assert.NotNull(mock.Object.HavingProperties(new{}));
        Assert.NotNull(mock.Object.WithStatus(AuditEventStatus.Info));
        Assert.NotNull(mock.Object.Success());
        Assert.NotNull(mock.Object.Info());
        Assert.NotNull(mock.Object.Error());
        Assert.NotNull(mock.Object.CausedBy(player.Object));
        Assert.NotNull(mock.Object.Cancel());
        Assert.NotNull(mock.Object.Cancel(true));
        Assert.NotNull(mock.Object.UnCancel());
        Assert.NotNull(mock.Object.Comment(""));
    }
}
