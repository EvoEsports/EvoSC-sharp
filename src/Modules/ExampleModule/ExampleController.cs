using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController : EvoScController<IControllerContext>
{
    private readonly ILogger<ExampleController> _logger;
    private readonly IServerClient _server;

    public ExampleController(ILogger<ExampleController> logger, IServerClient server)
    {
        _logger = logger;
        _server = server;
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat2(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("2Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat3(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("3Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat4(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("4Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat5(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("5Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat6(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("6Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }
    
    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat7(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("7Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat8(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("8Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat9(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("9Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat10(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("10Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat11(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("11Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat12(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("12Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat13(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("13Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat14(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("14Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat15(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("15Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat16(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("16Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat17(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("17Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat18(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("18Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat19(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("19Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat20(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("20Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat21(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("21Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat22(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("22Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat23(object sender, PlayerChatEventArgs args)
    {
        var player = await _server.Remote.GetPlayerInfoAsync(args.Login);
        _logger.LogInformation("23Player {Name} sent message: {Msg}", player.NickName, args.Text);
    }
}
