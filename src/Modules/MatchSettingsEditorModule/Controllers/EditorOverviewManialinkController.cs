using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;
using EvoSC.Modules.Official.MatchSettingsEditorModule.Permissions;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Controllers;

[Controller]
public class EditorOverviewManialinkController(
    IMatchSettingsEditorService matchSettingsEditor) : ManialinkController
{
    [ManialinkRoute(Permission = MatchSettingsEditorPermissions.EditMatchSettings)]
    public async Task EditMatchSettingsAsync(string name)
    {
        Context.AuditEvent.WithEventName("MatchSettingsEditor.OpenMatchSettingsEditor");
        try
        {
            await matchSettingsEditor.ShowEditorAsync(Context.Player, name);
            Context.AuditEvent.Success();
        }
        catch (Exception ex)
        {
            Context.AuditEvent
                .HavingProperties(new { ex.Message })
                .Error();
        }
    }

    /* [ManialinkRoute(Permission = MatchManagerPermissions.LoadMatchSettings)]
    public async Task LoadMatchSettingsAsync(string name)
    {
        try
        {
            await matchManager.LoadMatchSettingsAsync(name, Context.Player);
        }
        catch (Exception ex)
        {
            Context.AuditEvent
                .WithEventName(MatchManagerModule.Events.AuditEvents.MatchSettingsLoaded)
                .HavingProperties(new { ex.Message })
                .Error();
        }
    } */
}
