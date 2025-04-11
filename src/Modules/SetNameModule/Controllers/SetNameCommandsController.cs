using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.SetNameModule.Controllers;

[Controller]
public class SetNameCommandsController(IManialinkManager manialinks, Locale locale)
    : EvoScController<ICommandInteractionContext>
{
    private readonly dynamic _locale = locale;

    [ChatCommand("setname", "[Command.SetName]")]
    public async Task SetNameAsync()
    {
        await manialinks.SendManialinkAsync(Context.Player, "SetNameModule.EditName",
            new { Nickname = Context.Player.NickName, Locale = _locale });
    }
}
