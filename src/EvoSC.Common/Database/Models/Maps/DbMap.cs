using System.ComponentModel.DataAnnotations;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Models.Maps;
using RepoDb.Attributes;

namespace EvoSC.Common.Database.Models.Maps;

[Map("Maps")]
public class DbMap : IMap
{
    [Key]
    public long Id { get; set; }

    public string Uid { get; set; }

    public string FilePath { get; set; }
    
    public long AuthorId { get; set; }

    public bool Enabled { get; set; }

    public string Name { get; set; }

    public string ExternalId { get; set; }

    public DateTime? ExternalVersion { get; set; }
    
    public MapProviders? ExternalMapProvider { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    
    public IPlayer? Author { get; set; }
}
