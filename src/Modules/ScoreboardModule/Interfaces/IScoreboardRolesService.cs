namespace EvoSC.Modules.Official.ScoreboardModule.Interfaces;

public interface IScoreboardRolesService
{
    /// <summary>
    /// Sends the manialink containing the roles of the online players, which are shown next to their names.
    /// </summary>
    public Task SendRolesManialinkAsync();
}
