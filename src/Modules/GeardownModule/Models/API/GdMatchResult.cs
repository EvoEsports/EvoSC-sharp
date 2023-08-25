namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdMatchResult
{
    public int? id { get; set; }
    public string? result { get; set; }
    public bool? is_total_result { get; set; }
    public int? match_id { get; set; }
    public bool? pending { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
}