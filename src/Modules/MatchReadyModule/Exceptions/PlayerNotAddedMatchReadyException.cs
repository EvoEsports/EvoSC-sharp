using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Players;

namespace EvoSC.Modules.Official.MatchReadyModule.Exceptions;

public class PlayerNotAddedMatchReadyException(IPlayer player) : MatchReadyException
{
    public IPlayer Player { get; private set; } = player;

    public override string Message => $"The player {player.StrippedNickName} with UID {player.AccountId} is not part of the player list.";
}
