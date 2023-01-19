using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Config;

[Table("ConfigOptions")]
public class DbConfigOption
{
    [PrimaryKey]
    public string Key { get; set; }
    
    [Column]
    public string Value { get; set; }
}
