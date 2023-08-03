using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.Scoreboard.Controllers;

[Controller]
public class ScoreboardCommandsController : EvoScController<ICommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;
    private readonly dynamic _locale;

    public ScoreboardCommandsController(IManialinkManager manialinks, Locale locale)
    {
        _manialinks = manialinks;
        _locale = locale;
    }
    
    [ChatCommand("sb", "[Command.ShowScoreboard]")]
    public async Task ShowScoreboard()
    {
        await _manialinks.SendManialinkAsync(Context.Player, "Scoreboard.Scoreboard",
            new
            {
                Locale = _locale,
                MaxPlayers = 16
            });
    } 
}
