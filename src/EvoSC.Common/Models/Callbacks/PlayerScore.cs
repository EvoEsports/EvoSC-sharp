using Newtonsoft.Json;

namespace EvoSC.Common.Models.Callbacks;

public class PlayerScore
{
    [JsonProperty("login")]
    public string Login { get; init; }
    
    [JsonProperty("accountid")]
    public string AccountId { get; init; }
    
    [JsonProperty("name")]
    public string Name { get; init; }
    
    [JsonProperty("rank")]
    public int Rank { get; init; }
    
    [JsonProperty("roundpoints")]
    public int RoundPoints { get; init; }
    
    [JsonProperty("mappoints")]
    public int MapPoints { get; init; }
    
    [JsonProperty("matchpoints")]
    public int MatchPoints { get; init; }
    
    [JsonProperty("bestracetime")]
    public int BestRaceTime { get; init; }
    
    [JsonProperty("bestracecheckpoints")]
    public IEnumerable<int> BestRaceCheckpoints { get; init; }
    
    [JsonProperty("bestlaptime")]
    public int BestLapTime { get; init; }
    
    [JsonProperty("bestlapcheckpoints")]
    public IEnumerable<int> BestLapCheckpoints { get; init; }
    
    [JsonProperty("prevracetime")]
    public int PreviousRaceTime { get; init; }
    
    [JsonProperty("prevracecheckpoints")]
    public IEnumerable<int> PreviousRaceCheckpoints { get; init; }
}
