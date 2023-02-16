using EvoSC.Common.Controllers.Attributes;

namespace EvoSC.Modules.Official.ExampleModule;

public class ExampleFormModel
{
    public string MyValue { get; set; }
}

[Controller]
public class ExampleManialinkController : ManialinkController
{
    public async Task HandleActionAsync(int param)
    {
        
    }
}
