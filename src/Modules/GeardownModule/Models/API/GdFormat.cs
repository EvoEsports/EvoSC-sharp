using System.Collections.Generic;

namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdFormat
{
    public int? id { get; set; }
    public string? name { get; set; }
    public FormatType? type_id { get; set; }
    public string? description { get; set; }
    public List<GdMatchSetting>? match_settings { get; set; }
}