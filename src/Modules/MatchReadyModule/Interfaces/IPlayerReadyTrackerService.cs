using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IPlayerReadyTrackerService
{
    public IEnumerable<IPlayer> ReadyPlayers { get; }
    public int RequiredPlayers { get; }
    
    public void SetIsReady(IPlayer player, bool isReady);
    public void SetRequiredPlayers(int count);
}
