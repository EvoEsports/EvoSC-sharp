using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Extensions;

public static class HiddenManialinksPlayerExtensions
{
    /// <summary>
    /// Checks whether the given template should be hidden for the player.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="templateName"></param>
    /// <returns></returns>
    public static bool ManialinkIsHidden(this IPlayer player, string templateName)
    {
        return player.Settings.HiddenManialinks.Contains(templateName);
    }
}
