using Newtonsoft.Json;

namespace EvoSC.Common.Models.Callbacks;

public class TeamScore
{
    [JsonProperty("id")]
    public int Id { get; init; }
    
    [JsonProperty("name")]
    public string Name { get; init; }
    
    [JsonProperty("roundpoints")]
    public int RoundPoints { get; init; }
    
    [JsonProperty("mappoints")]
    public int MapPoints { get; init; }
    
    [JsonProperty("matchpoints")]
    public int MatchPoints { get; init; }
}
