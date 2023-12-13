using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.SetName.Interfaces;
using EvoSC.Modules.Official.SetName.Models;

namespace EvoSC.Modules.Official.SetName.Controllers;

[Controller]
public class SetNameController(ISetNameService setNameService, Locale locale) : ManialinkController
{
    public async Task EditNameAsync(SetNameEntryModel input)
    {
        if (!IsModelValid)
        {
            await ShowAsync(Context.Player, "SetName.EditName", new {input.Nickname, Locale = locale});
            return;
        }

        await setNameService.SetNicknameAsync(Context.Player, input.Nickname);
        await HideAsync(Context.Player, "SetName.EditName");

        Context.AuditEvent
            .Success()
            .WithEventName("EditNickname")
            .HavingProperties(new {OldNickname = Context.Player.NickName, NewNickname = input.Nickname});
    }

    public Task CancelAsync() => HideAsync(Context.Player, "SetName.EditName");
}
