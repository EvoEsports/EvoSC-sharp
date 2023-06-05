using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.CurrentMapModule.Interfaces;


public interface ICurrentMapService
{
    
    
    /// <summary>
    /// Shows widget
    /// </summary>
    /// <returns></returns>
    Task ShowWidget();
    
    /// <summary>
    /// Shows Widget
    /// </summary>
    /// <param name="args">Information about the begin map event.</param>
    /// <returns>Task</returns>
    Task ShowWidget(MapGbxEventArgs args);
    
    /// <summary>
    /// Hides Widget
    /// </summary>
    /// <returns></returns>
    Task HideWidget();
}
