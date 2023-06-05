using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.CurrentMapModule.Interfaces;


public interface ICurrentMapService
{
    
    
    /// <summary>
    /// Shows widget
    /// </summary>
    /// <returns></returns>
    Task ShowWidgetAsync();
    
    /// <summary>
    /// Shows Widget
    /// </summary>
    /// <param name="args">Information about the begin map event.</param>
    /// <returns>Task</returns>
    Task ShowWidgetAsync(MapGbxEventArgs args);
    
    /// <summary>
    /// Hides Widget
    /// </summary>
    /// <returns></returns>
    Task HideWidgetAsync();
}
