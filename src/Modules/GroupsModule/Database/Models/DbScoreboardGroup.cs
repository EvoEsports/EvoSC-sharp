using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.GroupsModule.Database.Models;

/// <summary>
/// A group that is shown as the role of its players on the scoreboard. Groups without a row here are not shown.
/// </summary>
[Table(TableName)]
public class DbScoreboardGroup
{
    public const string TableName = "ScoreboardGroups";

    [PrimaryKey]
    public int GroupId { get; set; }
}
