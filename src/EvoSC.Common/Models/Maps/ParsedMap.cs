using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;
using GBX.NET;
using GBX.NET.Engines.Game;
using GbxRemoteNet.Structs;

namespace EvoSC.Common.Models.Maps;

internal class ParsedMap : IParsedMap
{
    public IRaceTime AuthorTime { get; set; }
    public IRaceTime GoldTime { get; set; }
    public IRaceTime SilverTime { get; set; }
    public IRaceTime BronzeTime { get; set; }
    public string Environment { get; set; }
    public string Mood { get; set; }
    public int Cost { get; set; }
    public bool MultiLap { get; set; }
    public int LapCount { get; set; }
    public string MapStyle { get; set; }
    public string MapType { get; set; }
    public int CheckpointCount { get; set; }
    public IMap Map { get; set; }

    public ParsedMap(){}

    public ParsedMap(TmMapInfo serverMap, IMap map)
    {
        Map = map;

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
        Map = map;
        var mapNode = ParseMap(baseDirectory, map.FilePath);
        
        AuthorTime = RaceTime.FromMilliseconds(mapNode.AuthorTime?.Milliseconds ?? 0);
        GoldTime = RaceTime.FromMilliseconds(mapNode.GoldTime?.Milliseconds ?? 0);
        SilverTime = RaceTime.FromMilliseconds(mapNode.SilverTime?.Milliseconds ?? 0);
        BronzeTime = RaceTime.FromMilliseconds(mapNode.BronzeTime?.Milliseconds ?? 0);
        Environment = mapNode.Decoration?.ToString() ?? "";
        Mood = "";
        Cost = mapNode.Cost;
        LapCount = mapNode.NbLaps;
        MapStyle = mapNode.MapStyle ?? "";
        MapType = mapNode.MapType ?? "";
        CheckpointCount = mapNode.NbCheckpoints;
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
