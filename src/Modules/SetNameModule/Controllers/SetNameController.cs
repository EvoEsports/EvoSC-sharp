using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.SetNameModule.Interfaces;
using EvoSC.Modules.Official.SetNameModule.Models;

namespace EvoSC.Modules.Official.SetNameModule.Controllers;

[Controller]
public class SetNameController(ISetNameService setNameService, Locale locale) : ManialinkController
{
    public async Task EditNameAsync(SetNameEntryModel input)
    {
        if (!IsModelValid)
        {
            await ShowAsync(Context.Player, "SetNameModule.EditName", new {input.Nickname, Locale = locale});
            return;
        }

        await setNameService.SetNicknameAsync(Context.Player, input.Nickname);
        await HideAsync(Context.Player, "SetNameModule.EditName");

        Context.AuditEvent
            .Success()
            .WithEventName("EditNickname")
            .HavingProperties(new {OldNickname = Context.Player.NickName, NewNickname = input.Nickname});
    }

    public Task CancelAsync() => HideAsync(Context.Player, "SetNameModule.EditName");
}
