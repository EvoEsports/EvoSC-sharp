using System.Text.Json.Serialization;

namespace EvoSC.Common.Clients.Dtos;

// Should be expanded with more properties from TMX, however not necessary at time of implementation
public class MxMapInfoDto
{
    public string Username { get; set; }

    public string GbxMapName { get; set; }

    public string AuthorLogin { get; set; }

    [JsonPropertyName("TrackUID")]
    public string TrackUid { get; set; }
}
