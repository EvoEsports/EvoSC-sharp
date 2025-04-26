using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<ExampleController> _logger;

    public ExampleController(IMySettings settings, IChatCommandManager cmds, IServerClient server,
        IChatCommandManager chatCommands, IPermissionManager permissions, IPermissionRepository permRepo,
        IMapRepository mapRepo, IMatchSettingsService matchSettings, IManialinkActionManager manialinkActions,
        Locale locale, IEventManager events, IMapService mapService, ILogger<ExampleController> logger)
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
        _logger = logger;
    }

    [ChatCommand("hey", "Say hey!")]
    public async Task TmxAddMap(string name)
    {
        await _server.Chat.SendChatMessageAsync($"hello, {name}!", Context.Player);
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
            await _server.Chat.SendChatMessageAsync("Rating must be between 0 and 100 inclusively.", Context.Player);
        }
        else
        {
            await _server.Chat.SendChatMessageAsync($"Your rating: {rating}");
        }
    }

    [ChatCommand("test", "Some testing.")]
    public async Task TestCommand()
    {
        try
        {
            var mc = new MultiCall();
            var n = 50000;

            for (var i = 0; i < n; i++)
            {
                mc.Add("GetVersion");
            }

            Console.WriteLine($"Calling {n} methods with multicall ...");
            var result = await _server.Remote.MultiCallAsync(mc);

            Console.WriteLine($"Got {result.Length} results.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
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
