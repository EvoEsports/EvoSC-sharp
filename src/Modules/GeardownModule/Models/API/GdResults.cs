namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdResults
{
    public int matchId { get; set; }
    public IEnumerable<GdResult> results { get; set; }
}
