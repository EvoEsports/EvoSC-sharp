using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Config;

[Table(TableName)]
public class DbConfigOption
{
    public const string TableName = "ConfigOptions";
    
    [PrimaryKey]
    public string Key { get; set; }
    
    [Column]
    public string Value { get; set; }
}
