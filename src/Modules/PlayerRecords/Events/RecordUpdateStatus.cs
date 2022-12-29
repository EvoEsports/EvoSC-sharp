namespace EvoSC.Modules.Official.PlayerRecords.Events;

public enum RecordUpdateStatus
{
    /// <summary>
    /// The record is worse than the previous one and is not updated.
    /// </summary>
    NotUpdated,
    /// <summary>
    /// The record is completely new.
    /// </summary>
    New,
    /// <summary>
    /// The record got updated as it was an improvement.
    /// </summary>
    Updated,
    /// <summary>
    /// The record did not get updated because it is the same as the previous one.
    /// </summary>
    Equal
}
