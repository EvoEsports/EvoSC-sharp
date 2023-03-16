using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Reflection;
using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Validation;
using EvoSC.Manialinks.Validation;
using EvoSC.Modules.Interfaces;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace EvoSC.Manialinks;

public class ManialinkController : EvoScController<ManialinkInteractionContext>
{
    protected FormValidationResult ModelValidation { get; } = new();
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
        Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManiailinkData(new object()), player);
    
    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink, object data) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManiailinkData(data), player);
    
    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManiailinkData(new object()), players);

    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink, object data) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, PrepareManiailinkData(data), players);

    public Task HideAsync(string maniaLink) => Context.ManialinkManager.HideManialinkAsync(maniaLink);

    public Task HideAsync(IOnlinePlayer player, string maniaLink) =>
        Context.ManialinkManager.HideManialinkAsync(maniaLink, player);

    public Task HideAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.HideManialinkAsync(maniaLink, players);

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
                    ModelValidation.AddResult(new EntryValidationResult
                    {
                        Name = modelProp.Name,
                        IsInvalid = false,
                        Message = attrValidationResult?.ErrorMessage ?? "Invalid Value."
                    });
                }
                else
                {
                    ModelValidation.AddResult(new EntryValidationResult
                    {
                        Name = modelProp.Name,
                        IsInvalid = true,
                        Message = attrValidationResult?.ErrorMessage ?? "Invalid Value."
                    });
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
}
