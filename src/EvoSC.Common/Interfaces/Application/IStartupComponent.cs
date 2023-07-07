namespace EvoSC.Common.Interfaces.Application;

public interface IStartupComponent
{
    /// <summary>
    /// Name of the component.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Dependencies required to execute this component. Keep in mind that actions are always
    /// executed after services.
    /// </summary>
    public List<string> Dependencies { get; }
}
