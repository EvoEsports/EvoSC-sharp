using System.Collections.Generic;

namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdMatch
{
    public int? id { get; set; }
    public string? name { get; set; }
    public int? status_id { get; set; }
    public string? status { get; set; }
    public int? group_id { get; set; } //not used
    public int? map_pool_id { get; set; } //not used
    public string? date { get; set; }
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
    public List<GdParticipant>? participants { get; set; }
    public List<GdFormat>? formats { get; set; }
    public GdGameServer? selectedGameServer { get; set; }
    public List<GdMapPoolOrder>? map_pool_orders { get; set; }
}
