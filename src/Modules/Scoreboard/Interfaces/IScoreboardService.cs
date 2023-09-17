namespace EvoSC.Modules.Official.Scoreboard.Interfaces;

public interface IScoreboardService
{
    public Task ShowScoreboard();
    public Task HideNadeoScoreboard();
    public Task ShowNadeoScoreboard();
    public Task SendRequiredAdditionalInfos();
    public Task LoadAndSendRequiredAdditionalInfos();
    public void SetCurrentRound(int round);
}
