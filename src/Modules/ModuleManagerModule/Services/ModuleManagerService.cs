using System.Drawing;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Events;
using EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Services;


[Service]
public class ModuleManagerService : IModuleManagerService
{
    private readonly IContextService _context;
    private readonly IModuleManager _modules;
    private readonly IServerClient _server;
    private readonly dynamic _locale;
    
    public ModuleManagerService(IContextService context, IModuleManager modules, IServerClient server, Locale locale)
    {
        _context = context;
        _modules = modules;
        _server = server;
        _locale = locale;
    }
    
    public async Task EnableModuleAsync(IModuleLoadContext module)
    {
        _context.Audit()
            .WithEventName(AuditEvents.ModuleEnabled)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleEnabled);

        var actor = _context.Audit().Actor;
        
        try
        {
            await _modules.EnableAsync(module.LoadId);
            _context.Audit().Success();
            
            if (actor != null)
            {
                await _server.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasEnabled(module.ModuleInfo.Name), actor);
            }
        }
        catch (Exception ex)
        {
            _context.Audit().Error();
            
            if (actor != null)
            {
                await _server.ErrorMessageAsync(_locale.PlayerLanguage.FailedEnablingModule(ex.Message), actor);
            }
            
            throw;
        }
    }

    public async Task DisableModuleAsync(IModuleLoadContext module)
    {
        _context.Audit()
            .WithEventName(AuditEvents.ModuleEnabled)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleDisabled);

        var actor = _context.Audit().Actor;
        
        try
        {
            await _modules.DisableAsync(module.LoadId);
            _context.Audit().Success();
            
            if (actor != null)
            {
                await _server.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasDisabled(module.ModuleInfo.Name), actor);
            }
        }
        catch (Exception ex)
        {
            _context.Audit().Error();
            
            if (actor != null)
            {
                await _server.ErrorMessageAsync(_locale.PlayerLanguage.FailedDisablingModule(ex.Message), actor);
            }
            
            throw;
        }
    }

    public Task ListModulesAsync(IPlayer actor)
    {
        var message = new TextFormatter();
        message.AddText(_locale.PlayerLanguage.LoadedModules);

        foreach (var module in _modules.LoadedModules)
        {
            message.AddText(module.ModuleInfo.Name, style => style
                .WithColor(module.IsEnabled ? Color.Green : Color.Red)
            );

            message.AddText(", ");
        }

        return _server.InfoMessageAsync(message.ToString(), actor);
    }
}
