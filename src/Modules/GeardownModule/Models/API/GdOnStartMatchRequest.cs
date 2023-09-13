namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdOnStartMatchRequest
{
    public string matchToken { get; set; }
    public string join { get; set; } // Join Instructions. e.g. Club Name | Server #1
}