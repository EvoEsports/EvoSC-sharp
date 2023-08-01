using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class UIDemoController : EvoScController<ICommandInteractionContext>
{
    private IManialinkManager _manialinks;
    
    public UIDemoController(IManialinkManager manialinks)
    {
        _manialinks = manialinks;
    }
    
    [ChatCommand("uidemo", "Show the UI demo window.")]
    public Task UIDemoAsync()
    {
        return _manialinks.SendManialinkAsync(Context.Player, "ExampleModule.UIDemo");
    }
}
