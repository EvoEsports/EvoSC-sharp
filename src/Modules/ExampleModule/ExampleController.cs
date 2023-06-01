using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Builders;
using EvoSC.Common.Util.MatchSettings.Models.ModeScriptSettingsModels;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController : EvoScController<PlayerInteractionContext>
{
    private readonly IMySettings _settings;
    private readonly IServerClient _server;
    private readonly IChatCommandManager _chatCommands;
    private readonly IPermissionManager _permissions;
    private readonly IPermissionRepository _permRepo;
    private readonly IMapRepository _mapRepo;
    private readonly IMatchSettingsService _matchSettings;
    private readonly IManialinkActionManager _manialinkActions;

    public ExampleController(IMySettings settings, IChatCommandManager cmds, IServerClient server,
        IChatCommandManager chatCommands, IPermissionManager permissions, IPermissionRepository permRepo,
        IMapRepository mapRepo, IMatchSettingsService matchSettings, IManialinkActionManager manialinkActions)
    {
        _settings = settings;
        _server = server;
        _chatCommands = chatCommands;
        _permissions = permissions;
        _permRepo = permRepo;
        _mapRepo = mapRepo;
        _matchSettings = matchSettings;
        _manialinkActions = manialinkActions;
    }

    [ChatCommand("hey", "Say hey!")]
    public async Task TmxAddMap(string name)
    {
        await _server.SendChatMessageAsync($"hello, {name}!", Context.Player);
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
            await _server.SendChatMessageAsync("Rating must be between 0 and 100 inclusively.", Context.Player);
        }
        else
        {
            await _server.SendChatMessageAsync($"Your rating: {rating}");
        }
    }

    [ChatCommand("test", "Some testing.")]
    public async Task TestCommand()
    {
        var version = await _server.Remote.GetVersionAsync();
    }
}
