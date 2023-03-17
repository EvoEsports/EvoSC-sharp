using System.ComponentModel.DataAnnotations;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Permissions.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Interfaces.Validation;
using Microsoft.Extensions.Logging;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace EvoSC.Modules.Official.ExampleModule;

[EntryModel]
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

    [ManialinkRoute(Route = "/test/login", Permission = MyPermissions.MyPerm1)]
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
