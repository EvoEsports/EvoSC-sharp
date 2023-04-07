using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[FormEntryModel]
public class ExampleFormModel
{
    public int Username { get; set; }
    public string Password { get; set; }
}

[Controller]
public class ExampleManialinkController : ManialinkController
{
    private readonly ILogger<ExampleManialinkController> _logger;
    
    public ExampleManialinkController(ILogger<ExampleManialinkController> logger)
    {
        _logger = logger;
    }

    public async Task HandleActionAsync(ExampleFormModel myModel)
    {
        if (IsModelValid)
        {
            _logger.LogInformation("Username: {User}, Password: {Pass}", myModel.Username, myModel.Password);
            await HideAsync(Context.Player, "ExampleModule.MyManialink");
        }
        else
        {
            _logger.LogInformation("Form submission is not valid!");
            await ShowAsync(Context.Player, "ExampleModule.MyManialink", new {myModel.Username, myModel.Password});
        }
    }
    
    public async Task HandleAction2Async(ExampleFormModel myModel)
    {
        if (IsModelValid)
        {
            _logger.LogInformation("Username: {User}, Password: {Pass}", myModel.Username, myModel.Password);
            await HideAsync(Context.Player, "ExampleModule.MyManialink");
        }
        else
        {
            _logger.LogInformation("Form submission is not valid!");
            await ShowAsync(Context.Player, "ExampleModule.MyManialink", new {myModel.Username, myModel.Password});
        }
    }
}
