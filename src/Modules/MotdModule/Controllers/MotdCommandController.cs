using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Official.MotdModule.Interfaces;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdCommandController : EvoScController<ICommandInteractionContext>
{
    private readonly IMotdSettings _motdSettings;
    
    public MotdCommandController(IMotdSettings motdSettings)
    {
        _motdSettings = motdSettings;
    }

    [ChatCommand("motdsetinterval", "[Command.MotdSetInterval]")]
    public void SetFetchIntervalAsync(int interval)
    {
        _motdSettings.MotdFetchInterval = interval;
    } 
    
    [ChatCommand("motdseturl", "[Command.MotdSetUrl]")]
    public void SetUrlAsync(string url)
    {
        _motdSettings.MotdUrl = url;
    } 

}
