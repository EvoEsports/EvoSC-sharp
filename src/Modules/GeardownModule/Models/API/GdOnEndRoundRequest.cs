using EvoSC.Common.Remote.EventArgsModels;

namespace EvoSC.Modules.Evo.GeardownModule.Models.API;

public class GdOnEndRoundRequest
{
    public string matchToken { get; set; }
    public ScoresEventArgs eventData { get; set; }
}