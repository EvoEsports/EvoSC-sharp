namespace EvoSC.Common.Models.Audit;

public enum AuditEventStatus
{
    /// <summary>
    /// Informational event. Can be used for anything that is just a "notice".
    /// </summary>
    Info,
    
    /// <summary>
    /// Erroneous event. Can be used if something caused an error.
    /// </summary>
    Error,
    
    /// <summary>
    /// Successful event. Can be used for anything that caused a successful action or event.
    /// </summary>
    Success
}
