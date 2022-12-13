using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController2 : EvoScController<CommandInteractionContext>
{
    private readonly IModuleManager _modules;
    
    public ExampleController2(IModuleManager modules)
    {
        _modules = modules;
    }

    [ChatCommand("module", "Manage a module")]
    public async Task ManageModule(string action, string loadIdStr)
    {
        var loadId = Guid.Parse(loadIdStr);

        if (action == "enable")
        {
            await _modules.EnableAsync(loadId);
        }
        else if (action == "disable")
        {
            await _modules.DisableAsync(loadId);
        }
    }
}
