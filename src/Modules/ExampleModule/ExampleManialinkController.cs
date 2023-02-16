using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Attributes;

namespace EvoSC.Modules.Official.ExampleModule;

public class ExampleFormModel
{
    public string MyValue { get; set; }
}

[Controller]
public class ExampleManialinkController : ManialinkController
{
    [ManialinkRoute(Route = "/a/{param1}")]
    public async Task HandleActionAsync(int a)
    {
        
    }
}
