using System.Text.Json.Serialization;

namespace EvoSC.Modules.Official.MotdModule.Models;

public class ResponseData
{
    /// <summary>
    /// Id of the Entry.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }
    /// <summary>
    /// The message that will be displayed.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
    /// <summary>
    /// The server for which the message is destined.
    /// </summary>
    [JsonPropertyName("server")]
    public string Server { get; set; }
}

public class MotdResponse
{
    /// <summary>
    /// List of <see cref="ResponseData"/> containing the motds for (possibly multiple) servers.
    /// </summary>
    [JsonPropertyName("data")]
    public List<ResponseData> Data { get; set; }
}
