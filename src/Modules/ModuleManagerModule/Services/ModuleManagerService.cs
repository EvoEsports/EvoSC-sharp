using System.Drawing;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Events;
using EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Services;


[Service]
public class ModuleManagerService(IContextService context, IModuleManager modules, IChatService chat, Locale locale)
    : IModuleManagerService
{
    private readonly dynamic _locale = locale;

    public async Task EnableModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleEnabled)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleEnabled);

        var actor = context.Audit().Actor;
        
        try
        {
            await modules.EnableAsync(module.LoadId);
            context.Audit().Success();
            
            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasEnabled(module.ModuleInfo.Name), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();
            
            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedEnablingModule(ex.Message), actor);
            }
            
            throw;
        }
    }

    public async Task DisableModuleAsync(IModuleLoadContext module)
    {
        context.Audit()
            .WithEventName(AuditEvents.ModuleEnabled)
            .HavingProperties(new {module.LoadId, module.ModuleInfo})
            .Comment(_locale.Audit_ModuleDisabled);

        var actor = context.Audit().Actor;
        
        try
        {
            await modules.DisableAsync(module.LoadId);
            context.Audit().Success();
            
            if (actor != null)
            {
                await chat.SuccessMessageAsync(_locale.PlayerLanguage.ModuleWasDisabled(module.ModuleInfo.Name), actor);
            }
        }
        catch (Exception ex)
        {
            context.Audit().Error();
            
            if (actor != null)
            {
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.FailedDisablingModule(ex.Message), actor);
            }
            
            throw;
        }
    }

    public Task ListModulesAsync(IPlayer actor)
    {
        var message = new TextFormatter();
        message.AddText(_locale.PlayerLanguage.LoadedModules);

        foreach (var module in modules.LoadedModules)
        {
            message.AddText(module.ModuleInfo.Name, style => style
                .WithColor(module.IsEnabled ? Color.Green : Color.Red)
            );

            message.AddText(", ");
        }

        return chat.InfoMessageAsync(message.ToString(), actor);
    }
}
