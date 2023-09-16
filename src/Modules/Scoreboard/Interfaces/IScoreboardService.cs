namespace EvoSC.Modules.Official.Scoreboard.Interfaces;

public interface IScoreboardService
{
    public Task ShowScoreboard();
    public Task HideNadeoScoreboard();
    public Task ShowNadeoScoreboard();
    public Task SendRoundsInfo();
    public Task LoadAndUpdateRoundsPerMap();
    public void SetCurrentRound(int round);
}
