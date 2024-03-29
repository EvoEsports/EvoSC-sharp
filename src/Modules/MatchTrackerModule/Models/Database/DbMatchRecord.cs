﻿using EvoSC.Modules.Official.MatchTrackerModule.Migrations;
using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MatchTrackerModule.Models.Database;

[Table(AddMatchRecordsTable.TableName)]
public class DbMatchRecord
{
    [PrimaryKey, Identity]
    public long Id { get; set; }
    
    [Column]
    public Guid TimelineId { get; set; }

    [Column]
    public MatchStatus Status { get; set; }
    
    [Column]
    public string Report { get; set; }
    
    [Column]
    public DateTime Timestamp { get; set; }
}
