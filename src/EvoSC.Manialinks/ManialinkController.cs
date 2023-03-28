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
    public Task ShowAsync(string maniaLink) => Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManiailinkData(new object()));
    
    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink, object data) => Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManiailinkData(data));

    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(player, maniaLink, PrepareManiailinkData(new object()));
    
    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink, object data) =>
        Context.ManialinkManager.SendManialinkAsync(player, maniaLink, PrepareManiailinkData(data));
    
    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(players, maniaLink, PrepareManiailinkData(new object()));

    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink, object data) =>
        Context.ManialinkManager.SendManialinkAsync(players, maniaLink, PrepareManiailinkData(data));

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

    internal async Task<FormValidationResult> ValidateModelInternalAsync()
    {
        if (Context.ManialinkAction.EntryModel == null)
        {
            return ModelValidation;
        }
        
        var model = Context.ManialinkAction.EntryModel;
        
        ValidateProperties(model);
        await ValidateValidatableObjectModel(model);

        return ModelValidation;
    }

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

    private async Task ValidateValidatableObjectModel(object? model)
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

    private dynamic PrepareManiailinkData(object userData)
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

    internal void AddEarlyValidationResults(IEnumerable<ValidationResult> validationResults)
    {
        foreach (var valResult in validationResults)
        {
            AddModelValidationResult(valResult);
        }
    }
}
