using System.Globalization;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Localization;
using EvoSC.Common.Models.Audit;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using NSubstitute;

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
        this ControllerContextMock<IPlayerInteractionContext> mock, IOnlinePlayer actor)
    {
        mock.Context.Player.Returns(actor);
        mock.Context.AuditEvent.CausedBy(actor);

        return mock;
    }

    /// <summary>
    /// Create a new player interaction context mock.
    /// </summary>
    /// <param name="actor">The mocked actor that triggered this action.</param>
    /// <returns></returns>
    public static ControllerContextMock<IPlayerInteractionContext>
        NewPlayerInteractionContextMock(IOnlinePlayer actor) =>
        new ControllerContextMock<IPlayerInteractionContext>().SetupMock(actor);

    /// <summary>
    /// Set up a command interaction context mock.
    /// </summary>
    /// <param name="mock">The mock to set up.</param>
    /// <param name="actor">The mocked actor that triggered this command.</param>
    /// <returns></returns>
    public static ControllerContextMock<ICommandInteractionContext> SetupMock(
        this ControllerContextMock<ICommandInteractionContext> mock, IOnlinePlayer actor)
    {
        mock.Context.Player.Returns(actor);
        mock.Context.AuditEvent.CausedBy(actor);

        return mock;
    }

    /// <summary>
    /// Create a new command interaction context mock.
    /// </summary>
    /// <param name="actor">The mocked actor that triggered this command.</param>
    /// <returns></returns>
    public static ControllerContextMock<ICommandInteractionContext>
        NewCommandInteractionContextMock(IOnlinePlayer actor) =>
        new ControllerContextMock<ICommandInteractionContext>().SetupMock(actor);

    /// <summary>
    /// Set up a new Manialink context mock.
    /// </summary>
    /// <param name="mock">The mock to set up.</param>
    /// <param name="actor">The mocked actor that triggered this manialink action.</param>
    /// <param name="actionContext">The mocked action context to use.</param>
    /// <param name="mlManager">The mocked manialink manager to use.</param>
    /// <returns></returns>
    public static ControllerContextMock<IManialinkInteractionContext> SetupMock(
        this ControllerContextMock<IManialinkInteractionContext> mock, IOnlinePlayer actor,
        IManialinkActionContext actionContext, IManialinkManager mlManager)
    {
        mock.Context.Player.Returns(actor);
        mock.Context.ManialinkAction.Returns(actionContext);
        mock.Context.ManialinkManager.Returns(mlManager);
        mock.Context.AuditEvent.CausedBy(actor);

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
        IOnlinePlayer actor, IManialinkActionContext actionContext, IManialinkManager mlManager) =>
        new ControllerContextMock<IManialinkInteractionContext>().SetupMock(actor, actionContext, mlManager);

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
        var controller = Activator.CreateInstance(typeof(TController), services) as TController;

        if (controller == null)
        {
            throw new InvalidOperationException($"Failed to create instance of controller {typeof(TController)}");
        }

        controller.SetContext(contextMock.Context);

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
    public static IContextService NewContextServiceMock(IControllerContext context, IOnlinePlayer? actor)
    {
        var auditEvent = context.AuditEvent;
        var mock = Substitute.For<IContextService>();
        
        mock.Audit().Returns(auditEvent);
        mock.GetContext().Returns(context);

        if (actor != null)
        {
            mock.Audit().CausedBy(actor);
        }

        return mock;
    }

    /// <summary>
    /// Create a mocked instance of the locale manager. All localizations will return "Test_Locale_String".
    /// </summary>
    /// <param name="contextService">The context service to use for this localization manager.</param>
    /// <returns></returns>
    public static Locale NewLocaleMock(IContextService contextService)
    {
        var config = Substitute.For<IEvoScBaseConfig>();
        config.Locale.DefaultLanguage.Returns("en");
        var localeManager = Substitute.For<ILocalizationManager>();
        localeManager.GetString(Arg.Any<CultureInfo>(), Arg.Any<string>(), Arg.Any<object[]>())
            .Returns("Test_Locale_String");

        var locale = new LocaleResource(localeManager, contextService, config);
        return locale;
    }

    /// <summary>
    /// Create a new mock of the server client and it's GBXRemoteClient.
    /// </summary>
    /// <returns></returns>
    public static (IServerClient Client, IGbxRemoteClient Remote) NewServerClientMock()
    {
        var remote = Substitute.For<IGbxRemoteClient>();
        var client = Substitute.For<IServerClient>();
        client.Remote.Returns(remote);

        return (client, remote);
    }

    /// <summary>
    /// Create a new mock of the audit event builder. It does not assign any values, but all methods
    /// simply returns itself. Is used to verify the methods which are called for checking if auditing occured.
    /// </summary>
    /// <returns></returns>
    public static IAuditEventBuilder NewAuditEventBuilderMock()
    {
        var builder = Substitute.For<IAuditEventBuilder>();

        builder.CausedBy(Arg.Any<IPlayer>()).Returns(builder);
        builder.Comment(Arg.Any<string>()).Returns(builder);
        builder.HavingProperties(Arg.Any<object>()).Returns(builder);
        builder.Cancel().Returns(builder);
        builder.Cancel(Arg.Any<bool>()).Returns(builder);
        builder.Error().Returns(builder);
        builder.Info().Returns(builder);
        builder.Success().Returns(builder);
        builder.UnCancel().Returns(builder);
        builder.WithStatus(Arg.Any<AuditEventStatus>()).Returns(builder);
        builder.WithEventName(Arg.Any<string>()).Returns(builder);
        builder.WithEventName(Arg.Any<Enum>()).Returns(builder);

        return builder;
    }
}
