using System.Globalization;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Localization;
using EvoSC.Common.Models.Audit;
using EvoSC.Common.Remote;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace EvoSC.Testing;

public static class Mocking
{
    /// <summary>
    /// Create a new controller context mock that is set up with audit and context service mocks.
    /// </summary>
    /// <typeparam name="TContext">The context type to mock.</typeparam>
    /// <returns></returns>
    public static ControllerContextMock<TContext> NewControllerContextMock<TContext>()
        where TContext : class, IControllerContext
    {
        return new ControllerContextMock<TContext>();
    }

    /// <summary>
    /// Set up a player interaction context mock.
    /// </summary>
    /// <param name="mock">The mock to set up.</param>
    /// <param name="actor">The mocked actor that triggered this action.</param>
    /// <returns></returns>
    public static ControllerContextMock<IPlayerInteractionContext> SetupMock(
        this ControllerContextMock<IPlayerInteractionContext> mock, Mock<IServerClient> serverClient, IOnlinePlayer actor)
    {
        mock.Context.Setup(c => c.Player).Returns(actor);
        mock.Context.Setup(c => c.Server).Returns(serverClient.Object);
        mock.Context.Setup(c => c.Chat).Returns(serverClient.Object.Chat);
        mock.Context.Object.AuditEvent.CausedBy(actor);

        return mock;
    }

    /// <summary>
    /// Create a new player interaction context mock.
    /// </summary>
    /// <param name="actor">The mocked actor that triggered this action.</param>
    /// <returns></returns>
    public static ControllerContextMock<IPlayerInteractionContext>
        NewPlayerInteractionContextMock(Mock<IServerClient> serverClient, IOnlinePlayer actor) =>
        new ControllerContextMock<IPlayerInteractionContext>().SetupMock(serverClient, actor);

    /// <summary>
    /// Set up a command interaction context mock.
    /// </summary>
    /// <param name="mock">The mock to set up.</param>
    /// <param name="actor">The mocked actor that triggered this command.</param>
    /// <returns></returns>
    public static ControllerContextMock<ICommandInteractionContext> SetupMock(
        this ControllerContextMock<ICommandInteractionContext> mock, Mock<IServerClient> serverClient, IOnlinePlayer actor)
    {
        mock.Context.Setup(c => c.Player).Returns(actor);
        mock.Context.Setup(c => c.Server).Returns(serverClient.Object);
        mock.Context.Setup(c => c.Chat).Returns(serverClient.Object.Chat);
        mock.Context.Object.AuditEvent.CausedBy(actor);

        return mock;
    }

    /// <summary>
    /// Create a new command interaction context mock.
    /// </summary>
    /// <param name="actor">The mocked actor that triggered this command.</param>
    /// <returns></returns>
    public static ControllerContextMock<ICommandInteractionContext>
        NewCommandInteractionContextMock(Mock<IServerClient> serverClient, IOnlinePlayer actor) =>
        new ControllerContextMock<ICommandInteractionContext>().SetupMock(serverClient, actor);

    /// <summary>
    /// Set up a new Manialink context mock.
    /// </summary>
    /// <param name="mock">The mock to set up.</param>
    /// <param name="actor">The mocked actor that triggered this manialink action.</param>
    /// <param name="actionContext">The mocked action context to use.</param>
    /// <param name="mlManager">The mocked manialink manager to use.</param>
    /// <returns></returns>
    public static ControllerContextMock<IManialinkInteractionContext> SetupMock(
        this ControllerContextMock<IManialinkInteractionContext> mock, Mock<IServerClient> serverClient, IOnlinePlayer actor,
        IManialinkActionContext actionContext, IManialinkManager mlManager)
    {
        mock.Context.Setup(c => c.Player).Returns(actor);
        mock.Context.Setup(c => c.ManialinkAction).Returns(actionContext);
        mock.Context.Setup(m => m.ManialinkManager).Returns(mlManager);
        mock.Context.Setup(m => m.Server).Returns(serverClient.Object);
        mock.Context.Setup(m => m.Chat).Returns(serverClient.Object.Chat);
        mock.Context.Object.AuditEvent.CausedBy(actor);

        return mock;
    }

    /// <summary>
    /// Create a new manialink context mock.
    /// </summary>
    /// <param name="actor">The mocked actor that triggered this manialink action.</param>
    /// <param name="actionContext">The mocked action context to use.</param>
    /// <param name="mlManager">The mocked manialink manager to use.</param>
    /// <returns></returns>
    public static ControllerContextMock<IManialinkInteractionContext> NewManialinkInteractionContextMock(
        Mock<IServerClient> serverClient, IOnlinePlayer actor, IManialinkActionContext actionContext, IManialinkManager mlManager) =>
        new ControllerContextMock<IManialinkInteractionContext>().SetupMock(serverClient, actor, actionContext, mlManager);

    /// <summary>
    /// Create a new controller instance that will use the mocked context and services given.
    /// </summary>
    /// <param name="contextMock">The mocked controller context to use.</param>
    /// <param name="services">Either mocked or plain objects of services to pass to the constructor.</param>
    /// <typeparam name="TController">The type of the controller to create an instance for.</typeparam>
    /// <typeparam name="TContext">The context type which the controller uses.</typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if the controller instance cannot be created.</exception>
    public static TController NewControllerMock<TController, TContext>(ControllerContextMock<TContext> contextMock,
        params object[] services)
        where TController : class, IController
        where TContext : class, IControllerContext
    {
        var ctorArgs = services.Select(s => s.GetType().IsAssignableTo(typeof(Mock)) ? ((Mock)s).Object : s).ToArray();
        var controller = Activator.CreateInstance(typeof(TController), ctorArgs) as TController;

        if (controller == null)
        {
            throw new InvalidOperationException($"Failed to create instance of controller {typeof(TController)}");
        }

        controller.SetContext(contextMock.Context.Object);

        return controller;
    }

    /// <summary>
    /// Create new individual controller mock that is not part of a test class.
    /// </summary>
    /// <param name="services">Mocked or plain objects of services to pass to the controller's constructor.</param>
    /// <typeparam name="TController">The controller to instantiate.</typeparam>
    /// <typeparam name="TContext">The context type used by the controller.</typeparam>
    /// <returns></returns>
    public static (TController Controller, ControllerContextMock<TContext> ContextMock) NewControllerMock<TController,
        TContext>(params object[] services)
        where TController : class, IController
        where TContext : class, IControllerContext
    {
        var contextMock = NewControllerContextMock<TContext>();
        var controller = NewControllerMock<TController, TContext>(contextMock, services);

        return (controller, contextMock);
    }

    /// <summary>
    /// Create a new context context service mock from the given context and actor.
    /// </summary>
    /// <param name="context">The context which the context service will use.</param>
    /// <param name="actor">The actor that triggered the action.</param>
    /// <returns></returns>
    public static Mock<IContextService> NewContextServiceMock(IControllerContext context, IOnlinePlayer? actor)
    {
        var mock = new Mock<IContextService>();
        
        mock.Setup(s => s.Audit()).Returns(context.AuditEvent);
        mock.Setup(s => s.GetContext()).Returns(context);

        if (actor != null)
        {
            mock.Object.Audit().CausedBy(actor);
        }

        return mock;
    }

    /// <summary>
    /// Automatic setup of controller context and player object based on the provided controller context type.
    /// </summary>
    /// <typeparam name="T">Controller context type</typeparam>
    /// <returns></returns>
    public static (
        Mock<IContextService> ContextService,
        ControllerContextMock<T> ControllerContext,
        Mock<IOnlinePlayer> Player ) NewContextServiceMock<T>() where T : class, IControllerContext
    {
        var controllerContext = NewControllerContextMock<T>();
        var player = new Mock<IOnlinePlayer>();

        return (
            NewContextServiceMock(controllerContext.Context.Object, player.Object),
            controllerContext,
            player
        );
    }

    /// <summary>
    /// Creates a generic controller context of IControllerContext.
    /// </summary>
    /// <returns></returns>
    public static (
        Mock<IContextService> ContextService,
        ControllerContextMock<IControllerContext> ControllerContext,
        Mock<IOnlinePlayer> Player
        ) NewGenericServiceMock()
    {
        var controllerContext = NewControllerContextMock<IControllerContext>();
        var player = new Mock<IOnlinePlayer>();

        return (
            NewContextServiceMock(controllerContext.Context.Object, player.Object),
            controllerContext,
            player
        );
    }

    /// <summary>
    /// Create a mocked instance of the locale manager. All localizations will return "Test_Locale_String".
    /// </summary>
    /// <param name="contextService">The context service to use for this localization manager.</param>
    /// <returns></returns>
    public static Locale NewLocaleMock(IContextService contextService)
    {
        var config = new Mock<IEvoScBaseConfig>();
        config.Setup(m => m.Locale.DefaultLanguage).Returns("en");
        var localeManager = new Mock<ILocalizationManager>();
        localeManager.Setup(m => m.GetString(It.IsAny<CultureInfo>(), It.IsAny<string>(), It.IsAny<object[]>()))
            .Returns("Test_Locale_String");

        var locale = new LocaleResource(localeManager.Object, contextService, config.Object);
        return locale;
    }

    /// <summary>
    /// Create a new mock of the server client and it's GBXRemoteClient.
    /// </summary>
    /// <returns></returns>
    public static (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) NewServerClientMock()
    {
        var remote = new Mock<IGbxRemoteClient>();
        var client = new Mock<IServerClient>();
        var chat = new Mock<IChatService>();
        client.Setup(m => m.Remote).Returns(remote.Object);
        client.Setup(m => m.Chat).Returns(chat.Object);

        return (client, remote, chat);
    }

    /// <summary>
    /// Create a new mock of the audit event builder. It does not assign any values, but all methods
    /// simply returns itself. Is used to verify the methods which are called for checking if auditing occured.
    /// </summary>
    /// <returns></returns>
    public static Mock<IAuditEventBuilder> NewAuditEventBuilderMock()
    {
        var builder = new Mock<IAuditEventBuilder>();

        builder.Setup(m => m.CausedBy(It.IsAny<IPlayer>())).Returns(builder.Object);
        builder.Setup(m => m.Comment(It.IsAny<string>())).Returns(builder.Object);
        builder.Setup(m => m.HavingProperties(It.IsAny<object>())).Returns(builder.Object);
        builder.Setup(m => m.Cancel()).Returns(builder.Object);
        builder.Setup(m => m.Cancel(It.IsAny<bool>())).Returns(builder.Object);
        builder.Setup(m => m.Error()).Returns(builder.Object);
        builder.Setup(m => m.Info()).Returns(builder.Object);
        builder.Setup(m => m.Success()).Returns(builder.Object);
        builder.Setup(m => m.UnCancel()).Returns(builder.Object);
        builder.Setup(m => m.WithStatus(It.IsAny<AuditEventStatus>())).Returns(builder.Object);
        builder.Setup(m => m.WithEventName(It.IsAny<string>())).Returns(builder.Object);
        builder.Setup(m => m.WithEventName(It.IsAny<Enum>())).Returns(builder.Object);

        return builder;
    }
}
