using System.ComponentModel.DataAnnotations;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Validation;
using Microsoft.Extensions.Logging;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace EvoSC.Modules.Official.ExampleModule;

[EntryModel]
public class ExampleFormModel
{
    [MinLength(1, ErrorMessage = "Must not be empty")]
    [MaxLength(10, ErrorMessage = "Too long")]
    public string Username { get; set; }
    
    [MinLength(1, ErrorMessage = "Must not be empty")]
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
