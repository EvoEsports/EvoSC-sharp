using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchManagerModule.Exceptions;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchManagerHandlerService : IMatchManagerHandlerService
{
    private readonly ILiveModeService _liveModeService;
    private readonly IServerClient _server;
    private readonly IMatchSettingsService _matchSettings;
    private readonly ILogger<MatchManagerHandlerService> _logger;
    private readonly IEventManager _events;

    public MatchManagerHandlerService(ILiveModeService liveModeService, IServerClient server,
        IMatchSettingsService matchSettings, ILogger<MatchManagerHandlerService> logger, IEventManager events)
    {
        _liveModeService = liveModeService;
        _server = server;
        _matchSettings = matchSettings;
        _logger = logger;
        _events = events;
    }

    public async Task SetModeAsync(string mode, IPlayer actor)
    {
        if (mode == "list")
        {
            var modes = string.Join(", ", _liveModeService.GetAvailableModes());
            await _server.SuccessMessageAsync($"Available modes: $fff{modes}", actor);
        }
        else
        {
            try
            {
                string modeName = await _liveModeService.LoadModeAsync(mode);
                
                await _events.RaiseAsync(MatchSettingsEvent.LiveModeSet,
                    new LiveModeSetEventArgs {ModeAlias = mode, ModeName = modeName});
            }
            catch (LiveModeNotFoundException ex)
            {
                var modes = string.Join(", ", _liveModeService.GetAvailableModes());
                await _server.ErrorMessageAsync($"{ex.Message} Available modes: {modes}.", actor);
            }
        }
    }

    public async Task LoadMatchSettingsAsync(string name, IPlayer actor)
    {
        try
        {
            await _matchSettings.LoadMatchSettingsAsync(name);
            await _server.InfoMessageAsync($"{actor.NickName} loaded match settings: {name}");

            await _events.RaiseAsync(MatchSettingsEvent.MatchSettingsLoaded,
                new MatchSettingsLoadedEventArgs {Name = name});
        }
        catch (FileNotFoundException ex)
        {
            await _server.ErrorMessageAsync($"Cannot find MatchSettings named '{name}'.", actor);
        }
        catch (Exception ex)
        {
            await _server.ErrorMessageAsync($"An unknown error occured while trying to load the MatchSettings.", actor);
            _logger.LogError(ex, "Failed to load MatchSettings");
            throw;
        }
    }
}
