using System.ComponentModel.DataAnnotations;
using RepoDb.Attributes;

namespace EvoSC.Common.Database.Models.Config;

[Map("ConfigOptions")]
public class DbConfigOption
{
    [Key]
    public string Key { get; set; }
    public string Value { get; set; }
}
