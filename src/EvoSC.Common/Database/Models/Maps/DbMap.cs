using Dapper.Contrib.Extensions;

namespace EvoSC.Common.Database.Models;

[Table("Maps")]
public class DbMap
{
    [Key]
    public int Id { get; set; }

    public string Uid { get; set; }

    public string FilePath { get; set; }

    public bool Enabled { get; set; }

    public string Name { get; set; }

    public int ManiaExchangeId { get; set; }

    public DateTime? ManiaExchangeVersion { get; set; }
    
    public int TrackmaniaIoId { get; set; }

    public DateTime? TrackmaniaIoVersion { get; set; }

    public DbPlayer Player { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public IEnumerable<DbMapFavorite> FavoritedMaps { get; set; }

    public IEnumerable<DbPersonalBest> PersonalBests { get; set; }

    public IEnumerable<DbMapRecord> MapRecords { get; set; }

    public IEnumerable<DbMapKarma> MapKarmas { get; set; }

    public DbMapStatistic MapStatistic { get; set; }
}
