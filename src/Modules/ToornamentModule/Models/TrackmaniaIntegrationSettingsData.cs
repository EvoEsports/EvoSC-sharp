using System.Text.Json;
using System.Text.Json.Serialization;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Models;
public class TrackmaniaIntegrationSettingsData
{
    [JsonPropertyName("tracks_shuffle")]
    public bool TracksShuffle { get; set; } = false;

    [JsonPropertyName("game_mode")]
    public string GameMode { get; set; } = "time_attack";

    [JsonPropertyName("stage_number")]
    public int StageNumber { get; set; } = 0;

    [JsonPropertyName("group_number")]
    public int GroupNumber { get; set; } = 0;

    [JsonPropertyName("round_number")]
    public int RoundNumber { get; set; } = 0;

    [JsonPropertyName("scripts")]
    public ScriptData Scripts { get; set; } = new ScriptData();

    [JsonPropertyName("plugins")]
    public PluginData Plugins { get; set; } = new PluginData();

    public static TrackmaniaIntegrationSettingsData CreateFromObject(object x)
    {
        var settingsData = new TrackmaniaIntegrationSettingsData();
        if (x is JsonElement json)
        {
            settingsData = JsonSerializer.Deserialize<TrackmaniaIntegrationSettingsData>(json);
        }

        return settingsData;
    }

}
