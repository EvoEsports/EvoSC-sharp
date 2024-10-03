namespace EvoSC.Modules.Nsgr.ContactAdminModule.Interfaces;

public interface IContactAdminService
{
    /// <summary>
    /// Shows the widget
    /// </summary>
    /// <returns></returns>
    Task ShowWidgetAsync();

    /// <summary>
    /// Hides the Widget
    /// </summary>
    /// <returns></returns>
    Task HideWidgetAsync();
    
    /// <summary>
    /// Posts a help message containing the Trackmania server name to the specified Webhook
    /// </summary>
    /// <returns></returns>
    Task ContactAdminAsync();
}
