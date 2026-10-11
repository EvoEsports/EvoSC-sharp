using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.PlayerRecords.Events
{
    /// <summary>
    /// Exported so other modules can subscribe to the personal best event without a reference to
    /// this module: both sides then agree on the same enum type, and therefore on the event name.
    /// </summary>
    [Export]
    public enum PlayerRecordsEvent
    {
        /// <summary>
        /// Event that is triggered when a player sets a personal record.
        /// </summary>
        [Identifier(Name = "PlayerRecords.PbUpdate")]
        PbRecord
    }
}
