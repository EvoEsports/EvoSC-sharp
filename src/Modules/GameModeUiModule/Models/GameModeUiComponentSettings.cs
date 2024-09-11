namespace EvoSC.Modules.Official.GameModeUiModule.Models;

public class GameModeUiComponentSettings(string name, bool visible, double x, double y, double scale)
{
    public string Name { get; init; } = name;
    public bool Visible { get; set; } = visible;
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    public double Scale { get; set; } = scale;
    public bool UpdateVisible { get; set; } = true;
    public bool UpdatePosition { get; set; } = true;
    public bool UpdateScale { get; set; } = true;
}
