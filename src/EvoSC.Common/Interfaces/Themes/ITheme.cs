namespace EvoSC.Common.Interfaces.Themes;

public interface ITheme
{
    public Task ConfigureAsync();
    public Dictionary<string, object> ThemeOptions { get; }
    
    public Dictionary<string, string> ComponentReplacements { get; }
}
