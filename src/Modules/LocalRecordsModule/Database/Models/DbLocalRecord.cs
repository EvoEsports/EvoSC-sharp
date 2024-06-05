using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.LocalRecordsModule.Database.Models;

[Table(TableName)]
public class DbLocalRecord : ILocalRecord
{
    public const string TableName = "LocalRecords";
    
    [PrimaryKey, Identity]
    public long Id { get; set; }
    
    [Column]
    public long MapId { get; set; }
    
    [Column]
    public long RecordId { get; set; }
    
    [Column]
    public int Position { get; set; }

    public IMap Map => DbMap;
    
    public IPlayerRecord Record => DbRecord;

    [Association(ThisKey = nameof(MapId), OtherKey = nameof(DbMap.Id))]
    public DbMap DbMap { get; set; }
    
    [Association(ThisKey = nameof(RecordId), OtherKey = nameof(DbPlayerRecord.Id))]
    public DbPlayerRecord DbRecord { get; set; }
}
