using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.ScoreboardModule.Interfaces;

namespace EvoSC.Modules.Official.ScoreboardModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ScoreboardStateService : IScoreboardStateService
{
    private readonly object _currentRoundNumberMutex = new();
    private int _currentRoundNumber = 1;

    public Task SetCurrentRoundNumberAsync(int roundNumber)
    {
        lock (_currentRoundNumberMutex)
        {
            _currentRoundNumber = roundNumber;
        }

        return Task.CompletedTask;
    }

    public Task<int> GetCurrentRoundNumberAsync()
    {
        lock (_currentRoundNumberMutex)
        {
            return Task.FromResult(_currentRoundNumber);
        }
    }
}
