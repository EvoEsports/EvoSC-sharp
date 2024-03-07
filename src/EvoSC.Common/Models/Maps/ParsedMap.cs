using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;
using GBX.NET;
using GBX.NET.Engines.Game;
using GbxRemoteNet.Structs;

namespace EvoSC.Common.Models.Maps;

internal class ParsedMap : IParsedMap
{
    public long Id { get; set; }
    public string Uid { get; set; }
    public string Name { get; set; }
    public IPlayer? Author { get; }
    public string FilePath { get; set; }
    public bool Enabled { get; set; }
    public string? ExternalId { get; set; }
    public DateTime? ExternalVersion { get; set; }
    public MapProviders? ExternalMapProvider { get; set; }
    
    public IRaceTime AuthorTime { get; init; }
    public IRaceTime GoldTime { get; init; }
    public IRaceTime SilverTime { get; init; }
    public IRaceTime BronzeTime { get; init; }
    public string Environment { get; init; }
    public string Mood { get; init; }
    public int Cost { get; init; }
    public bool MultiLap { get; init; }
    public int LapCount { get; init; }
    public string MapStyle { get; init; }
    public string MapType { get; init; }
    public int CheckpointCount { get; init; }

    public ParsedMap(){}

    public ParsedMap(TmMapInfo serverMap, IMap map)
    {
        Id = map.Id;
        Uid = map.Uid;
        Name = map.Name;
        Author = map.Author;
        FilePath = map.FilePath;
        Enabled = map.Enabled;
        ExternalId = map.ExternalId;
        ExternalVersion = map.ExternalVersion;

        AuthorTime = RaceTime.FromMilliseconds(serverMap.AuthorTime);
        GoldTime = RaceTime.FromMilliseconds(serverMap.GoldTime);
        SilverTime = RaceTime.FromMilliseconds(serverMap.SilverTime);
        BronzeTime = RaceTime.FromMilliseconds(serverMap.BronzeTime);

        Environment = serverMap.Environnement;
        Mood = serverMap.Mood;
        Cost = serverMap.CopperPrice;
        MultiLap = serverMap.LapRace;
        MapStyle = serverMap.MapStyle;
        MapType = serverMap.MapType;
        CheckpointCount = serverMap.NbCheckpoints;
        LapCount = serverMap.NbLaps;
    }
    
    public ParsedMap(string baseDirectory, IMap map)
    {
        Id = map.Id;
        Uid = map.Uid;
        Name = map.Name;
        Author = map.Author;
        FilePath = map.FilePath;
        Enabled = map.Enabled;
        ExternalId = map.ExternalId;
        ExternalVersion = map.ExternalVersion;

        var mapNode = ParseMap(baseDirectory, map.FilePath);
        
        AuthorTime = RaceTime.FromMilliseconds(mapNode.TMObjective_AuthorTime?.Milliseconds ?? 0);
        GoldTime = RaceTime.FromMilliseconds(mapNode.TMObjective_GoldTime?.Milliseconds ?? 0);
        SilverTime = RaceTime.FromMilliseconds(mapNode.TMObjective_SilverTime?.Milliseconds ?? 0);
        BronzeTime = RaceTime.FromMilliseconds(mapNode.TMObjective_BronzeTime?.Milliseconds ?? 0);
        Environment = mapNode.Decoration?.ToString() ?? "";
        Mood = "";
        Cost = mapNode.Cost ?? 0;
        LapCount = mapNode.TMObjective_NbLaps ?? 0;
        MapStyle = mapNode.MapStyle ?? "";
        MapType = mapNode.MapType ?? "";
        CheckpointCount = mapNode.NbCheckpoints ?? 0;
    }

    private CGameCtnChallenge ParseMap(string baseDirectory, string filePath)
    {
        var realPath = Path.Combine(baseDirectory, filePath);
        
        if (!File.Exists(realPath))
        {
            throw new FileNotFoundException("Did not find map file.", realPath);
        }
        
        var cb = GameBox.Parse<CGameCtnChallenge>(realPath);

        if (cb == null)
        {
            throw new InvalidOperationException($"Failed to parse map file: {realPath}");
        }

        return cb.Node;
    }
}
