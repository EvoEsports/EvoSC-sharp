using EvoSC.Common.Models;
using EvoSC.Common.Models.Callbacks;
using EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Models.StateMessages;

public class ScoresMessage : IScoresMessage
{
    public string ClientId { get; set; }
    public DateTime Timestamp { get; set; }
    public IEnumerable<PlayerScore?> Scores { get; set; }
    public IEnumerable<TeamScore?> TeamScores { get; set; }
    public int WinnerTeam { get; set; }
    public string? WinnerPlayer { get; set; }
    public ModeScriptSection Section { get; set; }
    public bool UseTeams { get; set; }
}
