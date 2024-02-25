using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Audit;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Testing.Tests.TestClasses;
using NSubstitute;

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
        Assert.Equal(mock.AuditEventBuilder, mock.Context.AuditEvent);
        Assert.Equal(mock.Context, mock.ContextService.GetContext());
        Assert.Equal(mock.AuditEventBuilder, mock.ContextService.Audit());
    }

    [Fact]
    public void SetupMock_For_PlayerInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<IPlayerInteractionContext>();
        var player = Substitute.For<IOnlinePlayer>();
        mock.SetupMock(player);
        
        Assert.Equal(player, mock.Context.Player);
        mock.AuditEventBuilder.Received(1).CausedBy(player);
    }

    [Fact]
    public void NewPlayerInteractionContextMock_Returns_Correct_Mock()
    {
        var player = Substitute.For<IOnlinePlayer>();
        var mock = Mocking.NewPlayerInteractionContextMock(player);
        
        Assert.Equal(player, mock.Context.Player);
        mock.AuditEventBuilder.Received(1).CausedBy(player);
    }

    [Fact]
    public void SetupMock_For_CommandInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<ICommandInteractionContext>();
        var player = Substitute.For<IOnlinePlayer>();
        mock.SetupMock(player);
        
        Assert.Equal(player, mock.Context.Player);
        mock.AuditEventBuilder.Received(1).CausedBy(player);
    }
    
    [Fact]
    public void NewCommandInteractionContextMock_Returns_Correct_Mock()
    {
        var player = Substitute.For<IOnlinePlayer>();
        var mock = Mocking.NewCommandInteractionContextMock(player);
        
        Assert.Equal(player, mock.Context.Player);
        mock.AuditEventBuilder.Received(1).CausedBy(player);
    }
    
    
    [Fact]
    public void SetupMock_For_ManialinkInteraction_Sets_Up_Correctly()
    {
        var mock = Mocking.NewControllerContextMock<IManialinkInteractionContext>();
        var player = Substitute.For<IOnlinePlayer>();
        var mlActionContext = Substitute.For<IManialinkActionContext>();
        var mlManager = Substitute.For<IManialinkManager>();
        mock.SetupMock(player, mlActionContext, mlManager);
        
        Assert.Equal(player, mock.Context.Player);
        Assert.Equal(mlActionContext, mock.Context.ManialinkAction);
        Assert.Equal(mlManager, mock.Context.ManialinkManager);
        mock.AuditEventBuilder.Received(1).CausedBy(player);
    }
    
    [Fact]
    public void NewManialinkInteractionContextMock_Returns_Correct_Mock()
    {
        var player = Substitute.For<IOnlinePlayer>();
        var mlActionContext = Substitute.For<IManialinkActionContext>();
        var mlManager = Substitute.For<IManialinkManager>();
        var mock = Mocking.NewManialinkInteractionContextMock(player, mlActionContext, mlManager);
        
        Assert.Equal(player, mock.Context.Player);
        Assert.Equal(mlActionContext, mock.Context.ManialinkAction);
        Assert.Equal(mlManager, mock.Context.ManialinkManager);
        mock.AuditEventBuilder.Received(1).CausedBy(player);
    }

    [Fact]
    public async Task New_Controller_Mock_Returns_Mocked_Controller_Instance()
    {
        var contextMock = Mocking.NewControllerContextMock<IPlayerInteractionContext>();
        var controller = Mocking.NewControllerMock<TestController, IPlayerInteractionContext>(contextMock);

        await controller.DoingSomething();
        
        contextMock.AuditEventBuilder.Received(1).Success();
    }

    [Fact]
    public async Task New_Individual_Controller_And_Context_Mocks()
    {
        var mock = Mocking.NewControllerMock<TestController, IPlayerInteractionContext>();

        await mock.Controller.DoingSomething();
        
        mock.ContextMock.AuditEventBuilder.Received(1).Success();
    }

    [Fact]
    public async Task New_Controller_Mock_Passes_Ctor_Services()
    {
        var serviceMock = Substitute.For<ITestService>();
        var mock = Mocking.NewControllerMock<TestControllerWithServices, IPlayerInteractionContext>(serviceMock);

        await mock.Controller.DoSomething();
        
        serviceMock.Received(1).DoSomethingElse();
    }

    [Fact]
    public void NewContextServiceMock_Sets_Up_Correctly()
    {
        var contextMock = Mocking.NewControllerContextMock<IControllerContext>();
        var mock = Mocking.NewContextServiceMock(contextMock.Context, null);
        
        Assert.Equal(contextMock.AuditEventBuilder, mock.Audit());
        Assert.Equal(contextMock.Context, mock.GetContext());
        contextMock.AuditEventBuilder.DidNotReceive().CausedBy(Arg.Any<IOnlinePlayer>());
    }

    [Fact]
    public void NewContextServiceMock_Sets_Up_Correctly_With_Actor()
    {
        var actor = Substitute.For<IOnlinePlayer>();
        var contextMock = Mocking.NewControllerContextMock<IControllerContext>();
        var mock = Mocking.NewContextServiceMock(contextMock.Context, actor);
        
        Assert.Equal(contextMock.AuditEventBuilder, mock.Audit());
        Assert.Equal(contextMock.Context, mock.GetContext());
        contextMock.AuditEventBuilder.Received(1).CausedBy(actor);
    }

    [Fact]
    public void NewLocaleMock_Returns_Simplified_Mock()
    {
        var context = Mocking.NewControllerContextMock<IPlayerInteractionContext>();
        var locale = Mocking.NewLocaleMock(context.ContextService);

        var str = locale["SomeRandomLocale"];
        
        Assert.Equal("Test_Locale_String", str);
    }

    [Fact]
    public void NewServerClientMock_Returns_Mocked_Client()
    {
        var server = Mocking.NewServerClientMock();
        
        Assert.NotNull(server.Client);
        Assert.Equal(server.Remote, server.Client.Remote);
    }

    [Fact]
    public void NewAuditEventBuilderMock_Returns_Mock_With_No_Null_Methods()
    {
        var player = Substitute.For<IOnlinePlayer>();
        var mock = Mocking.NewAuditEventBuilderMock();
        
        Assert.NotNull(mock.WithEventName(""));
        Assert.NotNull(mock.WithEventName(TestEnum.TestField));
        Assert.NotNull(mock.HavingProperties(new{}));
        Assert.NotNull(mock.WithStatus(AuditEventStatus.Info));
        Assert.NotNull(mock.Success());
        Assert.NotNull(mock.Info());
        Assert.NotNull(mock.Error());
        Assert.NotNull(mock.CausedBy(player));
        Assert.NotNull(mock.Cancel());
        Assert.NotNull(mock.Cancel(true));
        Assert.NotNull(mock.UnCancel());
        Assert.NotNull(mock.Comment(""));
    }
}
