using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.MatchManagerModule.Config;

[Settings]
public interface ITrackerSettings
{
    [Option(DefaultValue = false), Description("Whether to start tracking matches automatically.")]
    public bool AutomaticTracking { get; set; }

    [Option(DefaultValue = true),
     Description("Whether to automatically end a match when the EndMatch section is detected.")]
    public bool AutomaticMatchEnd { get; set; }

    [Option(DefaultValue = true),
     Description("Whether to store match state changes immediately instead of waiting until the match ends.")]
    public bool ImmediateStoring { get; set; }

    [Option(DefaultValue = true), Description("Record end of map states.")]
    public bool RecordEndMap { get; set; }

    [Option(DefaultValue = true), Description("Record end of match states.")]
    public bool RecordEndMatch { get; set; }

    [Option(DefaultValue = true), Description("Record end of round states.")]
    public bool RecordEndRound { get; set; }

    [Option(DefaultValue = false), Description("Record pre end of match states.")]
    public bool RecordEndMatchEarly { get; set; }

    [Option(DefaultValue = false), Description("Keep pre end of round states.")]
    public bool RecordPreEndRound { get; set; }
}
