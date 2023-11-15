using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;

namespace EvoSC.Modules.snixtho.LiveMapModule.Controllers;

[Controller]
public class TestController : EvoScController<ICommandInteractionContext>
{
    private readonly IManialinkManager _manialinks;
    
    public TestController(IManialinkManager manialinks)
    {
        _manialinks = manialinks;
    }
    
    [ChatCommand("testpostracker", "yes")]
    public async Task TestPosTrackerAsync()
    {
        await _manialinks.SendManialinkAsync("LiveMapModule.PositionTracker");
    }
}
