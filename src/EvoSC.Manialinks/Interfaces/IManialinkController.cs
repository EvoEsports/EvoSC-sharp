using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Validation;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkController
{
    /// <summary>
    /// The model validation result of the current context.
    /// </summary>
    public FormValidationResult ModelValidation { get; }
    
    /// <summary>
    /// Whether the model of the current context is valid or not.
    /// </summary>
    public bool IsModelValid { get; }

    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink);

    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink, object data);

    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink);

    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink, object data);

    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink);

    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink, object data);

    /// <summary>
    /// Show a manialink to all players which persists between player connections and will
    /// automatically show when new players connects.
    /// </summary>
    /// <param name="name">The name of the manialink to show.</param>
    /// <returns></returns>
    public Task ShowPersistentAsync(string name);

    /// <summary>
    /// Show a manialink to all players which persists between player connections and will
    /// automatically show when new players connects.
    /// </summary>
    /// <param name="name">Name of the manialink to show.</param>
    /// <param name="data">Data to be sent to the manialink template.</param>
    /// <returns></returns>
    public Task ShowPersistentAsync(string name, object data);

    /// <summary>
    /// Hide a manialink for all players.
    /// </summary>
    /// <param name="maniaLink">The name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideAsync(string maniaLink);

    /// <summary>
    /// Hide a manialink from a player.
    /// </summary>
    /// <param name="player">The player to hide the manialink from.</param>
    /// <param name="maniaLink">The name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideAsync(IOnlinePlayer player, string maniaLink);

    /// <summary>
    /// Hide a manialink from a set of players.
    /// </summary>
    /// <param name="players">The players to hide the manialink from.</param>
    /// <param name="maniaLink">The name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideAsync(IEnumerable<IOnlinePlayer> players, string maniaLink);

    /// <summary>
    /// Validate the model of the current context.
    /// </summary>
    /// <returns></returns>
    public Task<FormValidationResult> ValidateModelAsync();
}
