using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.LocalRecordsModule.Permissions;

namespace EvoSC.Modules.Official.LocalRecordsModule.Controllers;

[Controller]
public class LocalRecordsManialinkController(ILocalRecordsService localRecordsService, IServerClient server) : ManialinkController
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
        await server.InfoMessageAsync("Resetting local records, this may take a while ...");

        try
        {
            await localRecordsService.ResetLocalRecordsAsync();
        }
        catch (Exception ex)
        {
            await server.ErrorMessageAsync($"Failed to reset local records: {ex.Message}");
            Context.AuditEvent.Error();
            throw;
        }
        
        await server.SuccessMessageAsync("Local records successfully reset!");
        Context.AuditEvent.Success();

        await localRecordsService.ShowWidgetToAllAsync();
    }
}
