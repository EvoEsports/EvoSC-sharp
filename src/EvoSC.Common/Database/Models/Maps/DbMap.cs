using System.ComponentModel.DataAnnotations.Schema;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models.Maps;

namespace EvoSC.Common.Database.Models;

[Dapper.Contrib.Extensions.Table("Maps")]
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

    [ForeignKey("FK_Maps_Players")]
    public DbPlayer Player { get; set; }

    /*[ForeignKey("MapFavorite")]
    public IEnumerable<DbMapFavorite>? FavoritedMaps { get; set; }

    [ForeignKey("PersonalBests")]
    public IEnumerable<DbPersonalBest>? PersonalBests { get; set; }

    [ForeignKey("MapRecords")]
    public IEnumerable<DbMapRecord>? MapRecords { get; set; }

    [ForeignKey("MapKarmas")]
    public IEnumerable<DbMapKarma>? MapKarmas { get; set; }

    [ForeignKey("MapStatistics")]
    public DbMapStatistic? MapStatistic { get; set; }*/
}
