using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController2 : EvoScController<ICommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;
    private readonly IThemeManager _themeManager;
    
    public ExampleController2(IManialinkManager manialinks, IThemeManager themeManager)
    {
        _manialinks = manialinks;
        _themeManager = themeManager;
    }

    [ChatCommand("show", "Show a manialink")]
    public async Task ShowManialink()
    {
        await _manialinks.SendManialinkAsync(Context.Player, "ExampleModule.MyManialink");
    }
    
    [ChatCommand("hide", "Hide a manialink")]
    public async Task HideManialink()
    {
        await _manialinks.HideManialinkAsync(Context.Player, "ExampleModule.MyManialink");
    }
}
