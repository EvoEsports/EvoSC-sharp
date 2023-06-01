using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;
using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Validation;
using EvoSC.Manialinks.Validation;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace EvoSC.Manialinks;

public class ManialinkController : EvoScController<ManialinkInteractionContext>
{
    /// <summary>
    /// The model validation result of the current context.
    /// </summary>
    protected FormValidationResult ModelValidation { get; } = new();
    
    /// <summary>
    /// Whether the model of the current context is valid or not.
    /// </summary>
    protected bool IsModelValid => ModelValidation?.IsValid ?? false;
    
    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink) => Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManialinkData(new object()));
    
    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink, object data) => Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManialinkData(data));

    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(player, maniaLink, PrepareManialinkData(new object()));
    
    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink, object data) =>
        Context.ManialinkManager.SendManialinkAsync(player, maniaLink, PrepareManialinkData(data));
    
    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(players, maniaLink, PrepareManialinkData(new object()));

    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink, object data) =>
        Context.ManialinkManager.SendManialinkAsync(players, maniaLink, PrepareManialinkData(data));

    /// <summary>
    /// Show a manialink to all players which persists between player connections and will
    /// automatically show when new players connects.
    /// </summary>
    /// <param name="name">The name of the manialink to show.</param>
    /// <returns></returns>
    public Task ShowPersistentAsync(string name) => Context.ManialinkManager.SendPersistentManialinkAsync(name, PrepareManialinkData(new object()));
    
    /// <summary>
    /// Show a manialink to all players which persists between player connections and will
    /// automatically show when new players connects.
    /// </summary>
    /// <param name="name">Name of the manialink to show.</param>
    /// <param name="data">Data to be sent to the manialink template.</param>
    /// <returns></returns>
    public Task ShowPersistentAsync(string name, object data) => Context.ManialinkManager.SendPersistentManialinkAsync(name, PrepareManialinkData(data));
    
    /// <summary>
    /// Hide a manialink for all players.
    /// </summary>
    /// <param name="maniaLink">The name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideAsync(string maniaLink) => Context.ManialinkManager.HideManialinkAsync(maniaLink);

    /// <summary>
    /// Hide a manialink from a player.
    /// </summary>
    /// <param name="player">The player to hide the manialink from.</param>
    /// <param name="maniaLink">The name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideAsync(IOnlinePlayer player, string maniaLink) =>
        Context.ManialinkManager.HideManialinkAsync(player, maniaLink);

    /// <summary>
    /// Hide a manialink from a set of players.
    /// </summary>
    /// <param name="players">The players to hide the manialink from.</param>
    /// <param name="maniaLink">The name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.HideManialinkAsync(players, maniaLink);

    /// <summary>
    /// Validate the model of the current context.
    /// </summary>
    /// <returns></returns>
    public Task<FormValidationResult> ValidateModelAsync() => ValidateModelInternalAsync();

    /// <summary>
    /// Begins validation of the current entry model.
    /// </summary>
    /// <returns></returns>
    internal async Task<FormValidationResult> ValidateModelInternalAsync()
    {
        if (Context.ManialinkAction.EntryModel == null)
        {
            return ModelValidation;
        }
        
        var model = Context.ManialinkAction.EntryModel;
        
        ValidateProperties(model);
        await ValidateValidatableObjectModelAsync(model);

        return ModelValidation;
    }

    /// <summary>
    /// Runs validation on each property in the instance of an entry model.
    /// </summary>
    /// <param name="model">Instance of the entry model.</param>
    private void ValidateProperties(object model)
    {
        var modelProperties = model.GetType().GetProperties(
            BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.DeclaredOnly);

        foreach (var modelProp in modelProperties)
        {
            var attributes = modelProp.GetCustomAttributes();
            var propValue = modelProp.GetValue(model);

            foreach (var attribute in attributes)
            {
                if (attribute is not ValidationAttribute validationAttribute)
                {
                    continue;
                }

                var attrValidationResult = validationAttribute.GetValidationResult(propValue, new ValidationContext(model));

                if (attrValidationResult == ValidationResult.Success)
                {
                    AddModelValidationResult(attrValidationResult);
                }
                else
                {
                    AddModelValidationResult(new ValidationResult(attrValidationResult.ErrorMessage,
                        new[] {modelProp.Name}));
                }
            }
        }
    }

    /// <summary>
    /// Run the model-level validation methods on an entry model.
    /// </summary>
    /// <param name="model">The instance of the entry model.</param>
    private async Task ValidateValidatableObjectModelAsync(object? model)
    {
        if (model == null)
        {
            return;
            
        }
        
        IEnumerable<ValidationResult> validationResults;

        switch (model)
        {
            // sync validation
            case IValidatableObject validatableObject:
                validationResults = validatableObject.Validate(new ValidationContext(model));
                break;
            // async validation
            case IAsyncValidatableObject asyncValidatableObject:
                validationResults = await asyncValidatableObject.ValidateAsync(new ValidationContext(model));
                break;
            default:
                return;
        }

        foreach (var validationResult in validationResults)
        {
            AddModelValidationResult(validationResult);
        }
    }

    /// <summary>
    /// Utility that prepares manialink data before sending like setting up validation result so that
    /// the developer don't need to do this manually.
    /// </summary>
    /// <param name="userData">Any custom data from the developer.</param>
    /// <returns></returns>
    private dynamic PrepareManialinkData(object userData)
    {
        dynamic data = new ExpandoObject();
        data.Validation = ModelValidation;

        var dataDict = (IDictionary<string, object?>)data;
        foreach (var prop in userData.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var key = prop.Name;
            var value = prop.GetValue(userData, null);
            
            dataDict[key] = value;
        }

        return data;
    }

    /// <summary>
    /// Add a new validation result to the form validation object.
    /// </summary>
    /// <param name="validationResult">Result from a validation step.</param>
    private void AddModelValidationResult(ValidationResult? validationResult)
    {
        if (validationResult == null)
        {
            return;
        }
        
        if (validationResult == ValidationResult.Success)
        {
            ModelValidation.AddResult(new EntryValidationResult
            {
                Name = validationResult.MemberNames.FirstOrDefault() ?? "Invalid Value.",
                IsInvalid = false,
                Message = validationResult?.ErrorMessage ?? ""
            });
        }
        else
        {
            ModelValidation.AddResult(new EntryValidationResult
            {
                Name = validationResult.MemberNames.FirstOrDefault() ?? "",
                IsInvalid = true,
                Message = validationResult?.ErrorMessage ?? "Invalid Value."
            });
        }
    }

    /// <summary>
    /// Adds any pre-validation results which are added before the value-level validation is run.
    /// </summary>
    /// <param name="validationResults">Validation results to be added.</param>
    internal void AddEarlyValidationResults(IEnumerable<ValidationResult> validationResults)
    {
        foreach (var valResult in validationResults)
        {
            AddModelValidationResult(valResult);
        }
    }
}
