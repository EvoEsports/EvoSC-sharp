namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdUser
{
    public int? id { get; set; }
    public string? nickname { get; set; }
    public string? tm_nickname { get; set; }
    public string? nat { get; set; } //nationality
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
}