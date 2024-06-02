using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models.Maps;

public class Map: IMap
{
    public long Id { get; set; }
    public string Uid { get; set; }
    public string Name { get; set; }
    public IPlayer Author { get; set; }
    public string FilePath { get; set; }
    public bool Enabled { get; set; }
    public string? ExternalId { get; set; }
    public DateTime? ExternalVersion { get; set; }
    public MapProviders? ExternalMapProvider { get; set; }
    
    public IMapDetails? Details { get; set; }

    public Map()
    {
    }

    public Map(DbMap dbMap) : this()
    {
        Id = dbMap.Id;
        Uid = dbMap.Uid;
        Name = dbMap.Name;
        Author = dbMap.Author;
        FilePath = dbMap.FilePath;
        Enabled = dbMap.Enabled;
        ExternalId = dbMap.ExternalId;
        ExternalVersion = dbMap.ExternalVersion;
        ExternalMapProvider = dbMap.ExternalMapProvider;
        Details = dbMap.Details;
    }
    
    public bool Equals(IMap? other) => other != null && Uid.Equals(other.Uid, StringComparison.Ordinal);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((IMap)obj);
    }

    public override int GetHashCode()
    {
        return Uid.GetHashCode();
    }
    
    public static IParsedMap Parse(string baseDirectory, IMap map) => new ParsedMap(baseDirectory, map);
}
