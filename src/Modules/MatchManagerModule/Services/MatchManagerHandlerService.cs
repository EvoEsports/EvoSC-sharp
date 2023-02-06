using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchManagerModule.Exceptions;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using GbxRemoteNet.Exceptions;
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

    public async Task SetScriptSettingAsync(string name, string value, IPlayer actor)
    {
        try
        {
            var settings = await _matchSettings.GetCurrentScriptSettingsAsync();

            if (settings == null)
            {
                throw new InvalidOperationException("Failed to get current script settings.");
            }

            var type = typeof(string);

            if (settings.ContainsKey(name))
            {
                type = settings[name].GetType();
            }

            var convertedValue = await MatchSettingsMapper.ToValueTypeAsync(type, value);

            await _matchSettings.SetCurrentScriptSettingsAsync(s => s[name] = convertedValue);
            await _server.SuccessMessageAsync($"Script setting '{name}' was set to: {value}");
        }
        catch (FormatException ex)
        {
            await _server.ErrorMessageAsync("Wrong format for script setting.", actor);
            _logger.LogError(ex, "Wrong format while setting script setting value");
        }
        catch (XmlRpcFaultException ex)
        {
            await _server.ErrorMessageAsync($"Failed to set script setting '{name}': {ex.Fault.FaultString}", actor);
            _logger.LogError(ex, "XMLRPC fault while setting script setting");
        }
        catch (Exception ex)
        {
            await _server.ErrorMessageAsync($"An error occured while trying to set the script setting: {ex.Message}");
            _logger.LogError(ex, "Failed to set script setting value");
            throw;
        }
    }
}
