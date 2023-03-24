using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Modules.Official.SetName.Models;

namespace EvoSC.Modules.Official.SetName.Controllers;

[Controller]
public class SetNameController : ManialinkController
{
    public Task EditNameAsync(SetNameEntryModel input)
    {
        if (!IsModelValid)
        {
            return ShowAsync(Context.Player, "SetName.EditName", new {input.Nickname});
        }

        Context.AuditEvent
            .WithEventName("EditNickname")
            .HavingProperties(new {OldNickname = Context.Player.NickName, NewNickname = input.Nickname});

        return Task.CompletedTask;
    }
}
