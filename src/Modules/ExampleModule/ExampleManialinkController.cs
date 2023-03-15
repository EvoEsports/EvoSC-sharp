using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Validation;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[EntryModel]
public class ExampleFormModel
{
    public string Username { get; set; }
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
    
    [ManialinkRoute(Route = "/test/login")]
    public async Task HandleActionAsync(ExampleFormModel myModel)
    {   
        _logger.LogInformation("Username: {User}, Password: {Pass}", myModel.Username, myModel.Password);

        if (await IsModelValidAsync())
        {
            _logger.LogInformation("Form submission valid!");
        }
        else
        {
            _logger.LogInformation("Form submission is not valid!");
        }

        await ShowAsync("ExampleModule.MyManialink", new
        {
            Validation = Validation,
            Username = myModel.Username, 
            Password = myModel.Password
        });
    }
}
