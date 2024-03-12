using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Maps;

[Table("Maps")]
public class DbMap : IMap
{
    [PrimaryKey, Identity]
    public long Id { get; set; }

    [Column]
    public string Uid { get; set; }

    [Column]
    public string FilePath { get; set; }
    
    [Column]
    public long AuthorId { get; set; }

    [Column]
    public bool Enabled { get; set; }

    [Column]
    public string Name { get; set; }

    [Column]
    public string? ExternalId { get; set; }

    [Column]
    public DateTime? ExternalVersion { get; set; }
    
    [Column]
    public MapProviders? ExternalMapProvider { get; set; }

    [Column]
    public DateTime CreatedAt { get; set; }

    [Column]
    public DateTime UpdatedAt { get; set; }
    
    [Association(ThisKey = nameof(AuthorId), OtherKey = nameof(DbPlayer.Id))]
    public DbPlayer DbAuthor { get; set; }

    public IPlayer? Author => DbAuthor;
    
    [Association(ThisKey = nameof(Id), OtherKey = nameof(DbMapDetails.MapId))]
    public DbMapDetails? DbDetails { get; set; }

    public IMapDetails? Details => DbDetails;
    
    public DbMap(){}

    public DbMap(IMap? map)
    {
        if (map == null)
        {
            return;
        }
        
        Id = map.Id;
        Uid = map.Uid;
        FilePath = "";
        AuthorId = map.Author?.Id ?? 0;
        Enabled = map.Enabled;
        Name = map.Name;
        ExternalId = map.ExternalId;
        ExternalVersion = map.ExternalVersion;
        ExternalMapProvider = map.ExternalMapProvider;
        CreatedAt = default;
        UpdatedAt = default;
        DbAuthor = new DbPlayer(map.Author);
    }
}
