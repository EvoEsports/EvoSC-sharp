using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.LocalRecordsModule.Permissions;

namespace EvoSC.Modules.Official.LocalRecordsModule.Controllers;

[Controller]
public class LocalRecordsManialinkController(ILocalRecordsService localRecordsService) : ManialinkController
{
    [ManialinkRoute(Permission = LocalRecordsPermission.ResetLocals)]
    public async Task ConfirmResetAsync(bool confirmed)
    {
        await HideAsync("LocalRecordsModule.Dialogs.ConfirmResetDialog");
        
        if (!confirmed)
        {
            return;
        }
        
        Context.AuditEvent.WithEventName(AuditEvents.ResetAll);
        await Context.Chat.InfoMessageAsync("Resetting local records, this may take a while ...");

        try
        {
            await localRecordsService.ResetLocalRecordsAsync();
        }
        catch (Exception ex)
        {
            await Context.Chat.ErrorMessageAsync($"Failed to reset local records: {ex.Message}");
            Context.AuditEvent.Error();
            throw;
        }
        
        await Context.Chat.SuccessMessageAsync("Local records successfully reset!");
        Context.AuditEvent.Success();

        await localRecordsService.ShowWidgetToAllAsync();
    }
}
