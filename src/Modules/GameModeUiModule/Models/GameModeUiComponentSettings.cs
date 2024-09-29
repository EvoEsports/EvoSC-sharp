namespace EvoSC.Modules.Official.GameModeUiModule.Models;

public class GameModeUiComponentSettings(string name, bool visible, double x, double y, double scale)
{
    /// <summary>
    /// The Name/ID of the game mode UI component.
    /// </summary>
    public string Name { get; init; } = name;
    
    /// <summary>
    /// Sets whether the component should be visible.
    /// </summary>
    public bool Visible { get; set; } = visible;
    
    /// <summary>
    /// Sets the X position of the component.
    /// </summary>
    public double X { get; set; } = x;
    
    /// <summary>
    /// Sets the Y position of the component.
    /// </summary>
    public double Y { get; set; } = y;
    
    /// <summary>
    /// Sets the scale of the component.
    /// </summary>
    public double Scale { get; set; } = scale;
    
    /// <summary>
    /// Sets whether the visibility of the component should be overwritten.
    /// </summary>
    public bool UpdateVisible { get; set; } = true;
    
    /// <summary>
    /// Sets whether the position of the component should be overwritten.
    /// </summary>
    public bool UpdatePosition { get; set; } = true;
    
    /// <summary>
    /// Sets whether the scale of the component should be overwritten.
    /// </summary>
    public bool UpdateScale { get; set; } = true;
}
