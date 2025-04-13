using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Permissions;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Controllers;

[Controller]
public class MatchSettingsEditorCommandController(IMatchSettingsEditorService editorService) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("mse", "Open the matchsettings editor.", MatchSettingsEditorPermissions.CanOpen)]
    public async Task OpenMatchSettingsEditorAsync()
    {
        await editorService.ShowEditorAsync(Context.Player);

        Context.AuditEvent
            .WithEventName("MatchSettingsEditor.OpenMatchSettingsEditor")
            .Success();
    }
}
