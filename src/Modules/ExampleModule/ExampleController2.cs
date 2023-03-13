using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController2 : EvoScController<CommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;
    
    public ExampleController2(IManialinkManager manialinks)
    {
        _manialinks = manialinks;
    }

    [ChatCommand("show", "Show a manialink")]
    public async Task ShowManialink()
    {
        await _manialinks.SendManialinkAsync("ExampleModule.MyManialink",
            new {MyObject = new MyObjectThings {Message = "Hello there!"}});
    }
    
    [ChatCommand("hide", "Hide a manialink")]
    public async Task HideManialink()
    {
        await _manialinks.HideManialinkAsync("ExampleModule.MyManialink");
    }
}
