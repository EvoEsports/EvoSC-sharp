using System.ComponentModel.DataAnnotations;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Interfaces.Validation;
using Microsoft.Extensions.Logging;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace EvoSC.Modules.Official.ExampleModule;

[EntryModel]
public class ExampleFormModel : IAsyncValidatableObject
{
    private readonly ILogger<ExampleFormModel> _logger;
    
    public ExampleFormModel(ILogger<ExampleFormModel> logger)
    {
        _logger = logger;
    }
    
    public string Username { get; set; }
    public string Password { get; set; }

    public Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();
        
        if (string.IsNullOrEmpty(Username))
        {
            results.Add(new ValidationResult("field is empty", new[] {nameof(Username)}));
        }
        
        if (string.IsNullOrEmpty(Password))
        {
            results.Add(new ValidationResult("field is empty.", new[] {nameof(Password)}));
        }
        
        _logger.LogInformation("done with validation!");

        return Task.FromResult(results.AsEnumerable());
    }
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
