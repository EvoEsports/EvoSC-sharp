namespace EvoSC.Modules.Official.ASayModule.Interfaces;

public interface IASayService
{
    /// <summary>
    /// Displays an announcement banner containing a user-defined message.
    /// </summary>
    /// <param name="text">The text that should be displayed as part of the announcement banner.</param>
    /// <returns>Returns a completed task</returns>
    Task ShowAnnouncementAsync(string text);
    
    /// <summary>
    /// Hides an existing announcement banner.
    /// </summary>
    /// <returns>Returns a completed task.</returns>
    Task HideAnnouncementAsync();
}
