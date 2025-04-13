using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.LocalRecordsModule.Permissions;

namespace EvoSC.Modules.Official.LocalRecordsModule.Controllers;

[Controller]
public class CommandsController(IManialinkManager manialinks, ILocalRecordsService localRecordsService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("resetlocals", "Recalculates local records table on all maps based on pbs.",
        LocalRecordsPermission.ResetLocals)]
    public async Task ResetLocalsAsync()
    {
        await manialinks.SendManialinkAsync(Context.Player, "LocalRecordsModule.Dialogs.ConfirmResetDialog");
        Context.AuditEvent.Cancel();
    }
    
    [ChatCommand("transferlocals", "Command.Transfer", LocalRecordsPermission.TransferLocals)]
    public Task TransferLocalsAsync(long originMapId, long targetMapId) => localRecordsService.TransferLocalRecordsAsync(originMapId, targetMapId);
}
