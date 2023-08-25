using System.Collections.Generic;

namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdGroup
{
    public int? id { get; set; }
    public string? type { get; set; }
    public string? name { get; set; }
    public int? min_size { get; set; } //not used
    public int? max_size { get; set; } //not used
    public int? event_id { get; set; }
    public int? is_type_tree { get; set; } //not used
    public string? created_at { get; set; }
    public string? updated_at { get; set; }
    public List<GdParticipant>? praticipants { get; set; }
}