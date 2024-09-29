using System.Text.Json.Serialization;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Models;
public class ScriptData
{
    [JsonPropertyName("S_NbOfWinners")]
    public int S_NbOfWinners { get; set; } = 1;

    [JsonPropertyName("S_PointsLimit")]
    public int S_PointsLimit { get; set; } = 50;

    [JsonPropertyName("S_PointsRepartition")]
    public string S_PointsRepartition { get; set; } = "10,6,4,3,2,1";

    [JsonPropertyName("S_RespawnBehaviour")]
    public int S_RespawnBehaviour { get; set; } = 0;

    [JsonPropertyName("S_RoundsPerMap")]
    public int S_RoundsPerMap { get; set; } = -1;

    [JsonPropertyName("S_FinishTimeout")]
    public int S_FinishTimeout { get; set; } = -1;

    [JsonPropertyName("S_DelayBeforeNextMap")]
    public int S_DelayBeforeNextMap { get; set; } = 2000;

    [JsonPropertyName("S_WarmUpNb")]
    public int S_WarmUpNb { get; set; } = 0;

    [JsonPropertyName("S_WarmUpDuration")]
    public int S_WarmUpDuration { get; set; } = 0;

    [JsonPropertyName("S_MapsPerMatch")]
    public int S_MapsPerMatch { get; set; } = -1;

    [JsonPropertyName("S_UseTieBreak")]
    public bool S_UseTieBreak { get; set; } = false;
}
