using System.Text.Json.Serialization;

namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdMatchToken
{
    [JsonPropertyName("matchId")]
    public int MatchId { get; set; }
    
    [JsonPropertyName("evoscToken")]
    public string EvoScToken { get; set; }
}
