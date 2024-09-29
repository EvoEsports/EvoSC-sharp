using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IScoresMessage: IStateMessage
{
    IEnumerable<PlayerScore?> Scores { get; set; }
    IEnumerable<TeamScore?> TeamScores { get; set; }
    int WinnerTeam { get; set; }
    string? WinnerPlayer { get; set; }
    ModeScriptSection Section { get; set; }
    bool UseTeams { get; set; }
}
