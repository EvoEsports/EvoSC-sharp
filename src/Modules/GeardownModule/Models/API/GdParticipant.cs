namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdParticipant
{
    public int? id { get; set; }
    public string? type { get; set; } //user|page
    public int? user_id { get; set; }
    public int? page_id { get; set; }
    public int event_id { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
    public GdUser? user { get; set; } //null in team formats
}
