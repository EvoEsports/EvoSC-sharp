using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MotdModule.Interfaces;

public interface IMotdService
{
    /// <summary>
    /// Shows the MOTD to the player.
    /// </summary>
    /// <param name="player">The <see cref="IPlayer"/> on which the Motd should open.</param>
    /// <param name="explicitly">If false, checks if the player has a <see cref="IMotdEntry"/> and if they previously hid it, will not open if they did.</param>
    /// <returns></returns>
    public Task ShowAsync(IPlayer player, bool explicitly);
    /// <summary>
    /// Shows the MOTD to the player.
    /// </summary>
    /// <param name="login">The login of the <see cref="IPlayer"/> on which the Motd should open.</param>
    /// <param name="explicitly">If false, checks if the player has a <see cref="IMotdEntry"/> and if they previously hid it, will not open if they did.</param>
    /// <returns></returns>
    public Task ShowAsync(string login, bool explicitly);
    /// <summary>
    /// Shows the edit ui for changing the local motd text
    /// </summary>
    /// <param name="player">The <see cref="IPlayer"/> on which the editor should open.</param>
    /// <returns></returns>
    public Task ShowEditAsync(IPlayer player);
    /// <summary>
    /// Sets the motd source to either local or remote.
    /// </summary>
    /// <param name="local">Indicates if the Motd should be fetched from a server or from local settings.</param>
    /// <param name="player">The <see cref="IPlayer"/> for logging.</param>
    public void SetMotdSource(bool local, IPlayer player);
    /// <summary>
    /// Sets the locally stored motd
    /// </summary>
    /// <param name="text">The new Motd text</param>
    /// <param name="player">The <see cref="IPlayer"/> for logging.</param>
    public void SetLocalMotd(string text, IPlayer player);
    /// <summary>
    /// Gets the Motd either from a server or from the locally stored settings.
    /// </summary>
    /// <returns>Task containing a string of the motd text</returns>
    public Task<string> GetMotdAsync();
    /// <summary>
    /// Gets the <see cref="IMotdEntry"/> of the specified player.
    /// </summary>
    /// <param name="player">The <see cref="IPlayer"/> of which the <see cref="IMotdEntry"/> should be fetched.</param>
    /// <returns>The <see cref="IMotdEntry"/> of the player.</returns>
    public Task<IMotdEntry?> GetEntryAsync(IPlayer player);
    /// <summary>
    /// Inserts, if the entry doesn't exists, or updates the entry that indicates whether or not the motd should be hidden.
    /// </summary>
    /// <param name="player">The <see cref="IPlayer"/> that should be updated.</param>
    /// <param name="hidden">Whether or not the motd should be hidden.</param>
    /// <returns></returns>
    public Task InsertOrUpdateEntryAsync(IPlayer player, bool hidden);
    /// <summary>
    /// Sets the interval for fetching the motd from the server.
    /// </summary>
    /// <param name="interval">Time in milliseconds.</param>
    /// <param name="player">The <see cref="IPlayer"/> for logging.</param>
    public void SetInterval(int interval, IPlayer player);
    /// <summary>
    /// Sets the url for fetching the motd.
    /// </summary>
    /// <param name="url">The url to fetch from.</param>
    /// <param name="player">The <see cref="IPlayer"/> for logging.</param>
    public void SetUrl(string url, IPlayer player);
}
