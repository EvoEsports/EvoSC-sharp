using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.PlayerRecords;

public enum PlayerRecordsEvent
{
    /// <summary>
    /// Event that is triggered when a player sets a personal record.
    /// </summary>
    [Identifier(Name = "PlayerRecords.PbUpdate")]
    PbRecord
}
