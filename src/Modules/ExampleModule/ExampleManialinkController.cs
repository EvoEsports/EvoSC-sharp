using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using SimpleInjector;

namespace EvoSC.Modules.Official.ExampleModule;

[EntryModel]
public class ExampleFormModel
{
    public string MyValue { get; set; }
}

[Controller]
public class ExampleManialinkController : ManialinkController
{
    private readonly IServerClient _server;
    
    public ExampleManialinkController(IServerClient server)
    {
        _server = server;
    }
    
    public async Task HandleActionAsync(int b, ExampleFormModel myModel)
    {
        await _server.SuccessMessageAsync($"You entered: {myModel.MyValue}", Context.Player);
    }
}
