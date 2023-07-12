using System.Dynamic;
using System.Globalization;
using System.Resources;
using EvoSC.Commands;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Localization;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using Org.BouncyCastle.Crypto.Tls;

namespace EvoSC.Testing;

public static class Mocking
{
    public static ControllerContextMock<TContext> NewControllerContextMock<TContext>()
        where TContext : class, IControllerContext
    {
        return new ControllerContextMock<TContext>();
    }

    public static ControllerContextMock<IPlayerInteractionContext> SetupMock(
        this ControllerContextMock<IPlayerInteractionContext> mock, IOnlinePlayer actor)
    {
        mock.Context.Setup(c => c.Player).Returns(actor);
        mock.Context.Object.AuditEvent.CausedBy(actor);

        return mock;
    }

    public static ControllerContextMock<IPlayerInteractionContext>
        NewPlayerInteractionContextMock(IOnlinePlayer actor) =>
        new ControllerContextMock<IPlayerInteractionContext>().SetupMock(actor);

    public static ControllerContextMock<ICommandInteractionContext> SetupMock(
        this ControllerContextMock<ICommandInteractionContext> mock, IOnlinePlayer actor)
    {
        mock.Context.Setup(c => c.Player).Returns(actor);
        mock.Context.Object.AuditEvent.CausedBy(actor);

        return mock;
    }

    public static ControllerContextMock<ICommandInteractionContext>
        NewCommandInteractionContextMock(IOnlinePlayer actor) =>
        new ControllerContextMock<ICommandInteractionContext>().SetupMock(actor);

    public static ControllerContextMock<IManialinkInteractionContext> SetupMock(
        this ControllerContextMock<IManialinkInteractionContext> mock, IOnlinePlayer actor,
        IManialinkActionContext actionContext)
    {
        mock.Context.Setup(c => c.Player).Returns(actor);
        mock.Context.Setup(c => c.ManialinkAction).Returns(actionContext);
        mock.Context.Object.AuditEvent.CausedBy(actor);

        return mock;
    }

    public static ControllerContextMock<IManialinkInteractionContext> NewManialinkInteractionContextMock(
        IOnlinePlayer actor, IManialinkActionContext actionContext) =>
        new ControllerContextMock<IManialinkInteractionContext>().SetupMock(actor, actionContext);

    public static TController NewControllerMock<TController,
        TContext>(ControllerContextMock<TContext> contextMock, params Mock[] services)
        where TController : class, IController
        where TContext : class, IControllerContext
    {
        var ctorArgs = services.Select(s => s.Object).ToArray();
        var controller = Activator.CreateInstance(typeof(TController), ctorArgs) as TController;

        if (controller == null)
        {
            throw new InvalidOperationException($"Failed to create instance of controller {typeof(TController)}");
        }

        controller.SetContext(contextMock.Context.Object);

        return controller;
    }

    public static (TController Controller, ControllerContextMock<TContext> ContextMock) NewControllerMock<TController,
        TContext>(params Mock[] services)
        where TController : class, IController
        where TContext : class, IControllerContext
    {
        var contextMock = NewControllerContextMock<TContext>();
        var controller = NewControllerMock<TController, TContext>(contextMock, services);

        return (controller, contextMock);
    }

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

    public static (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote) NewServerClientMock()
    {
        var remote = new Mock<IGbxRemoteClient>();
        var client = new Mock<IServerClient>();
        client.Setup(m => m.Remote).Returns(remote.Object);

        return (client, remote);
    }
}
