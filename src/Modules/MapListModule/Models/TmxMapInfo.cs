namespace EvoSC.Modules.Official.MapListModule.Models;

/// <summary>
/// Summary of a map found on Trackmania Exchange.
/// </summary>
public class TmxMapInfo
{
    public int Id { get; init; }
    public string Uid { get; init; } = "";
    public string Name { get; init; } = "";
    public string Authors { get; init; } = "";
    public List<string> Tags { get; init; } = [];
    public string Difficulty { get; init; } = "";
    public TimeSpan Length { get; init; }
    public int Awards { get; init; }
    public DateTime? UploadedAt { get; init; }

    public string ThumbnailUrl => $"http://trackmania.exchange/mapthumb/{Id}";
}

/// <summary>
/// One page of search results from Trackmania Exchange.
/// </summary>
public class TmxSearchResult
{
    public List<TmxMapInfo> Maps { get; init; } = [];
    public bool HasMore { get; init; }
}

/// <summary>
/// A player's current state in the "add map" dialog.
/// </summary>
public class TmxSearchState
{
    public string Query { get; set; } = "";
    public int Page { get; set; }
    public TmxSearchResult Result { get; set; } = new();
    public int? SelectedId { get; set; }
    public string? Message { get; set; }
}
