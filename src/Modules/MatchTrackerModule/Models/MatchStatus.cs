using LinqToDB.Mapping;

namespace EvoSC.Modules.Official.MatchTrackerModule.Models;

public enum MatchStatus
{
    [MapValue(Value = "Unknown")]
    Unknown,
    
    [MapValue(Value = "Started")]
    Started,
    
    [MapValue(Value = "Running")]
    Running,
    
    [MapValue(Value = "Ended")]
    Ended,
    
    [MapValue(Value = "Paused")]
    Paused,
    
    [MapValue(Value = "UnPaused")]
    UnPaused
}
