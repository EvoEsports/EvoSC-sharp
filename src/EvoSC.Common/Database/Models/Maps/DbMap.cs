using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Player;

namespace EvoSC.Common.Database.Models.Maps;

[Table("Maps")]
public class DbMap
{
    [Key]
    public long Id { get; set; }

    public string Uid { get; set; }
    
    public long Author { get; set; }

    public string FilePath { get; set; }

    public bool Enabled { get; set; }

    public string Name { get; set; }

    public long? ManiaExchangeId { get; set; }

    public DateTime? ManiaExchangeVersion { get; set; }
    
    public long? TrackmaniaIoId { get; set; }

    public DateTime? TrackmaniaIoVersion { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
