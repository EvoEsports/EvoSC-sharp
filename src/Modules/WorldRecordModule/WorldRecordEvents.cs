using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.WorldRecordModule;

public enum WorldRecordEvents
{
    /// <summary>
    /// Teams and player scores reported.
    /// </summary>
    [Identifier(Name = "WorldRecord.NewRecord")]
    NewRecord,
}
