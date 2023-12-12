using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.CurrentMapModule.Interfaces;


public interface ICurrentMapService
{
    /// <summary>
    /// Shows the widget
    /// </summary>
    /// <returns></returns>
    Task ShowWidgetAsync();
    
    /// <summary>
    /// Shows the Widget
    /// </summary>
    /// <param name="args">Information about the begin map event.</param>
    /// <returns></returns>
    Task ShowWidgetAsync(MapGbxEventArgs args);
    
    /// <summary>
    /// Hides the Widget
    /// </summary>
    /// <returns></returns>
    Task HideWidgetAsync();
}
