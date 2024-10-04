﻿using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.RoundRankingModule.Interfaces;

namespace EvoSC.Modules.Official.RoundRankingModule;

[Module(IsInternal = true)]
public class RoundRankingModule(IRoundRankingService roundRankingService) : EvoScModule, IToggleable
{
    public async Task EnableAsync()
    {
        await roundRankingService.UpdatePointsRepartitionAsync();
        await roundRankingService.FetchAndCacheTeamInfoAsync();
        await roundRankingService.DetectModeAsync();
        await roundRankingService.DisplayRoundRankingWidgetAsync();
    }

    public Task DisableAsync() => Task.CompletedTask;
}
