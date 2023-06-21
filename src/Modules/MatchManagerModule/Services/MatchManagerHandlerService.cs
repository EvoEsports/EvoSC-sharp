using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Modules.Official.MatchManagerModule.Events;
using EvoSC.Modules.Official.MatchManagerModule.Events.EventArgObjects;
using EvoSC.Modules.Official.MatchManagerModule.Exceptions;
using EvoSC.Modules.Official.MatchManagerModule.Interfaces;
using GbxRemoteNet.Exceptions;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchManagerModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class MatchManagerHandlerService : IMatchManagerHandlerService
{
    private readonly ILiveModeService _liveModeService;
    private readonly IServerClient _server;
    private readonly IMatchSettingsService _matchSettings;
    private readonly ILogger<MatchManagerHandlerService> _logger;
    private readonly IEventManager _events;
    private readonly IContextService _context;
    private readonly dynamic _locale;

    public MatchManagerHandlerService(ILiveModeService liveModeService, IServerClient server,
        IMatchSettingsService matchSettings, ILogger<MatchManagerHandlerService> logger, IEventManager events,
        IContextService context, ILocale locale)
    {
        _liveModeService = liveModeService;
        _server = server;
        _matchSettings = matchSettings;
        _logger = logger;
        _events = events;
        _context = context;
        _locale = locale;
    }

    public async Task SetModeAsync(string mode, IPlayer actor)
    {
        if (mode == "list")
        {
            var modes = string.Join(", ", _liveModeService.GetAvailableModes());
            await _server.SuccessMessageAsync(_locale.PlayerLanguage.AvailableModes(modes), actor);
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
                await _server.ErrorMessageAsync(_locale.PlayerLanguage.LiveModeNotFound(ex.Message, modes), actor);
            }
        }
    }

    public async Task LoadMatchSettingsAsync(string name, IPlayer actor)
    {
        try
        {
            await _matchSettings.LoadMatchSettingsAsync(name);

            _context.Audit().Success()
                .WithEventName(AuditEvents.MatchSettingsLoaded)
                .HavingProperties(new {Name = name});
            
            await _server.InfoMessageAsync(_locale.LoadedMatchSettings(actor.NickName, name));

            await _events.RaiseAsync(MatchSettingsEvent.MatchSettingsLoaded,
                new MatchSettingsLoadedEventArgs {Name = name});
        }
        catch (FileNotFoundException ex)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.CannotFindMatchSettings(name), actor);
        }
        catch (Exception ex)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.UnknownErrorWhenLoadingMatchSettings, actor);
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

            _context.Audit().Success()
                .WithEventName(AuditEvents.ScriptSettingsModified)
                .HavingProperties(new {Name = name, Value = value})
                .Comment(_locale.Audit_ModeScriptSettingsModified);
            
            await _server.SuccessMessageAsync(_locale.ScriptSettingsSetTo(name, value));
        }
        catch (FormatException ex)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.WrongScriptSettingFormat, actor);
            _logger.LogError(ex, "Wrong format while setting script setting value");
        }
        catch (XmlRpcFaultException ex)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguageFailedSettingScriptSetting(name, ex.Fault.FaultString), actor);
            _logger.LogError(ex, "XMLRPC fault while setting script setting");
        }
        catch (Exception ex)
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.ErrorOccuredWhenSettingScriptSetting(ex.Message), actor);
            _logger.LogError(ex, "Failed to set script setting value");
            throw;
        }
    }
}
