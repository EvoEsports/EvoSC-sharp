using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.UiControlModule.Interfaces;

public interface IUiControlService
{
    /// <summary>
    /// Show the UI Control menu to the player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Task DisplayMenuAsync(IOnlinePlayer player);

    /// <summary>
    /// Returns a list of template names that can be hidden by the player.
    /// </summary>
    /// <returns></returns>
    public List<string> GetTemplateNames();

    /// <summary>
    /// Persists the hidden manialinks for the player.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="hiddenManialinks"></param>
    /// <returns></returns>
    public Task SaveSettingsAsync(IOnlinePlayer player, IEnumerable<string> hiddenManialinks);
}
