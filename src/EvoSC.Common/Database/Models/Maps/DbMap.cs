using Dapper.Contrib.Extensions;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Database.Models.Maps;

[Table("Maps")]
public class DbMap : IMap
{
    [Key]
    public long Id { get; set; }

    public string Uid { get; set; }

    public string FilePath { get; set; }
    
    public long AuthorId { get; set; }

    public bool Enabled { get; set; }

    public string Name { get; set; }

    public long ManiaExchangeId { get; set; }

    public DateTime? ManiaExchangeVersion { get; set; }
    
    public long TrackmaniaIoId { get; set; }

    public DateTime? TrackmaniaIoVersion { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    [Computed]
    public IPlayer Author { get; set; }
}
