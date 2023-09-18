namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdGameServer
{
    public int? id { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
    public string? name { get; set; }
    public string? url { get; set; } // can be used as join instructions as well
    public int? game_id { get; set; }
    public int? event_id { get; set; }
    public int? pending { get; set; }
}