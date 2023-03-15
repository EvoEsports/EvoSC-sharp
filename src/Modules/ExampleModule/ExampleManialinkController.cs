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
    public Task HandleActionAsync(ExampleFormModel myModel)
    {
        _logger.LogInformation("Username: {User}, Password: {Pass}", myModel.Username, myModel.Password);

        var validation = new FormValidationResult();

        validation.AddResult(new ValidationResult
        {
            Name = "Username",
            IsInvalid = string.IsNullOrWhiteSpace(myModel.Username),
            Message = "Invalid username."
        });
        validation.AddResult(new ValidationResult
        {
            Name = "Password",
            IsInvalid = string.IsNullOrWhiteSpace(myModel.Password),
            Message = "Invalid password."
        });

        return ShowAsync("ExampleModule.MyManialink",
            new {Validation = validation, Username = myModel.Username, Password = myModel.Password});
    }
}
