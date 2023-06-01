using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.SetName.Controllers;

[Controller]
public class SetNameCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;

    public SetNameCommandsController(IManialinkManager manialinks) => _manialinks = manialinks;

    [ChatCommand("setname", "Set a custom nickname.")]
    public async Task SetNameAsync()
    {
        await _manialinks.SendManialinkAsync(Context.Player, "SetName.EditName",
            new {Nickname = Context.Player.NickName});
    } 
}
