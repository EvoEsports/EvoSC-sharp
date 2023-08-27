﻿namespace EvoSC.Modules.Official.Scoreboard.Interfaces;

public interface IScoreboardService
{
    public Task ShowScoreboard(string playerLogin);
    public Task ShowScoreboard();
    public Task HideNadeoScoreboard();
    public Task ShowNadeoScoreboard();
    public Task SendRoundsInfo();
    public void LoadAndUpdateRoundsPerMap();
}