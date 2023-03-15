using System.Dynamic;
using System.Reflection;
using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Validation;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Manialinks;

public class ManialinkController : EvoScController<ManialinkInteractionContext>
{
    protected FormValidationResult? Validation { get; private set; }
    
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

    protected Task<FormValidationResult> ValidateAsync() => ValidateInternalAsync();
    protected async Task<bool> IsModelValidAsync() => (await ValidateAsync()).IsValid;

    internal Task<FormValidationResult> ValidateInternalAsync()
    {
        return Task.FromResult(new FormValidationResult());
    }

    private dynamic PrepareManiailinkData(object userData)
    {
        dynamic data = new ExpandoObject();

        if (Validation != null)
        {
            data.Validation = Validation;
        }

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
