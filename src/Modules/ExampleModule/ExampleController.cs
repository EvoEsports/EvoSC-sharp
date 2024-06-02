using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController : EvoScController<IPlayerInteractionContext>
{
    private readonly IMySettings _settings;
    private readonly IServerClient _server;
    private readonly IChatCommandManager _chatCommands;
    private readonly IPermissionManager _permissions;
    private readonly IPermissionRepository _permRepo;
    private readonly IMapRepository _mapRepo;
    private readonly IMatchSettingsService _matchSettings;
    private readonly IManialinkActionManager _manialinkActions;
    private readonly Locale _locale;
    private readonly IEventManager _events;
    private readonly IMapService _mapService;

    public ExampleController(IMySettings settings, IChatCommandManager cmds, IServerClient server,
        IChatCommandManager chatCommands, IPermissionManager permissions, IPermissionRepository permRepo,
        IMapRepository mapRepo, IMatchSettingsService matchSettings, IManialinkActionManager manialinkActions,
        Locale locale, IEventManager events, IMapService mapService)
    {
        _settings = settings;
        _server = server;
        _chatCommands = chatCommands;
        _permissions = permissions;
        _permRepo = permRepo;
        _mapRepo = mapRepo;
        _matchSettings = matchSettings;
        _manialinkActions = manialinkActions;
        _locale = locale;
        _events = events;
        _mapService = mapService;
    }

    [ChatCommand("hey", "Say hey!")]
    public async Task TmxAddMap(string name)
    {
        await _server.SendChatMessageAsync(Context.Player, $"hello, {name}!");
    }

    [ChatCommand("ratemap", "Rate the current map.", "test")]
    [CommandAlias("+++", 100)]
    [CommandAlias("++", true, 80)]
    [CommandAlias("+", 60)]
    [CommandAlias("-", 40)]
    [CommandAlias("--", 20)]
    [CommandAlias("---", 0)]
    public async Task RateMap(int rating)
    {
        if (rating is < 0 or > 100)
        {
            await _server.SendChatMessageAsync(Context.Player, "Rating must be between 0 and 100 inclusively.");
        }
        else
        {
            await _server.SendChatMessageAsync($"Your rating: {rating}");
        }
    }

    [ChatCommand("test", "Some testing.")]
    public async Task TestCommand()
    {
        var mapList = await _server.Remote.GetMapListAsync(-1, 0);

        var maps = new List<IMap>();
        foreach (var map in mapList)
        {
            maps.Add(await _mapService.GetMapByUidAsync(map.UId));
        }
    }

    [ChatCommand("rejoin", "Simulates the player joining the server.")]
    public async Task RejoinCommand()
    {
        _events.RaiseAsync(GbxRemoteEvent.PlayerConnect,
            new PlayerConnectGbxEventArgs
            {
                Login = Context.Player.GetLogin(), 
                IsSpectator = false
            });
    }

    [ChatCommand("fakeplayer", "Add a fake player to the game.")]
    public async Task AddFakePlayer()
    {
        await _server.Remote.ConnectFakePlayerAsync();
    }
}
