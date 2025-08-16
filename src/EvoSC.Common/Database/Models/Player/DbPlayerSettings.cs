using EvoSC.Common.Interfaces.Models;
using LinqToDB.Common;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Player;

[Table(TableName)]
public class DbPlayerSettings : IPlayerSettings
{
    public const string TableName = "PlayerSettings";

    [Column] public long PlayerId { get; set; }

    [Column] public string DisplayLanguage { get; set; }

    [Column(nameof(HiddenManialinks))]
    public string? DbHiddenManialinks { get; set; }
    
    public string? HiddenManialinks  {
         get => DbHiddenManialinks != null ? DbHiddenManialinks.Split(",").ToList()  : [];
         set {
               if (hiddenManialinks.IsNullOrEmpty())
              {
                  DbHiddenManialinks = null;
                  return;
              }

              DbHiddenManialinks = string.Join(",", value);
         }
    }
}
