using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.SetName.Controllers;

[Controller]
public class SetNameCommandsController : EvoScController<CommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;
    private readonly dynamic _locale;

    public SetNameCommandsController(IManialinkManager manialinks, Locale locale)
    {
        _manialinks = manialinks;
        _locale = locale;
    }
    
    [ChatCommand("setname", "[Command.SetName]")]
    public async Task SetNameAsync()
    {
        await _manialinks.SendManialinkAsync(Context.Player, "SetName.EditName",
            new
            {
                Nickname = Context.Player.NickName, 
                Locale = _locale
            });
    } 
}
