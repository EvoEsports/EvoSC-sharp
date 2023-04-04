using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.SetName.Controllers;

[Controller]
public class SetNameCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly IManialinkManager _manialinkses;

    public SetNameCommandsController(IManialinkManager manialinkses) => _manialinkses = manialinkses;

    [ChatCommand("setname", "Set a custom nickname.")]
    public async Task SetNameAsync()
    {
        await _manialinkses.SendManialinkAsync(Context.Player, "SetName.EditName",
            new {Nickname = Context.Player.NickName});
    } 
}
