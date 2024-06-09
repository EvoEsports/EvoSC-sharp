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
public class MatchManagerHandlerService(ILiveModeService liveModeService, IServerClient server,
        IMatchSettingsService matchSettings, ILogger<MatchManagerHandlerService> logger, IEventManager events,
        IContextService context, Locale locale)
    : IMatchManagerHandlerService
{
    private readonly dynamic _locale = locale;

    public async Task SetModeAsync(string mode, IPlayer actor)
    {
        if (mode == "list")
        {
            var modes = string.Join(", ", liveModeService.GetAvailableModes());
            await server.SuccessMessageAsync(actor, _locale.PlayerLanguage.AvailableModes(modes));
        }
        else
        {
            try
            {
                string modeName = await liveModeService.LoadModeAsync(mode);
                
                await events.RaiseAsync(MatchSettingsEvent.LiveModeSet,
                    new LiveModeSetEventArgs {ModeAlias = mode, ModeName = modeName});
            }
            catch (LiveModeNotFoundException ex)
            {
                var modes = string.Join(", ", liveModeService.GetAvailableModes());
                await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.LiveModeNotFound(ex.Message, modes));
            }
        }
    }

    public async Task LoadMatchSettingsAsync(string name, IPlayer actor)
    {
        try
        {
            await matchSettings.LoadMatchSettingsAsync(name);

            context.Audit().Success()
                .WithEventName(AuditEvents.MatchSettingsLoaded)
                .HavingProperties(new {Name = name});
            
            await server.InfoMessageAsync(_locale.LoadedMatchSettings(actor.NickName, name));

            await events.RaiseAsync(MatchSettingsEvent.MatchSettingsLoaded,
                new MatchSettingsLoadedEventArgs {Name = name});
        }
        catch (FileNotFoundException ex)
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.CannotFindMatchSettings(name));
        }
        catch (Exception ex)
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.UnknownErrorWhenLoadingMatchSettings);
            logger.LogError(ex, "Failed to load MatchSettings");
            throw;
        }
    }

    public async Task SetScriptSettingAsync(string name, string value, IPlayer actor)
    {
        try
        {
            var settings = await matchSettings.GetCurrentScriptSettingsAsync();

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

            await matchSettings.SetCurrentScriptSettingsAsync(s => s[name] = convertedValue);

            context.Audit().Success()
                .WithEventName(AuditEvents.ScriptSettingsModified)
                .HavingProperties(new {Name = name, Value = value})
                .Comment(_locale.Audit_ModeScriptSettingsModified);
            
            await server.SuccessMessageAsync(_locale.ScriptSettingsSetTo(name, value));
        }
        catch (FormatException ex)
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.WrongScriptSettingFormat);
            logger.LogError(ex, "Wrong format while setting script setting value");
        }
        catch (XmlRpcFaultException ex)
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguageFailedSettingScriptSetting(name, ex.Fault.FaultString));
            logger.LogError(ex, "XMLRPC fault while setting script setting");
        }
        catch (Exception ex)
        {
            await server.ErrorMessageAsync(actor, _locale.PlayerLanguage.ErrorOccuredWhenSettingScriptSetting(ex.Message));
            logger.LogError(ex, "Failed to set script setting value");
            throw;
        }
    }
}
