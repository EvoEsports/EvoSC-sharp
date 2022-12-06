namespace EvoSC.Commands.Interfaces;

public interface ICommandAlias
{
    /// <summary>
    /// The name of the alias, this can be anything.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Any default arguments to pass to the command. The arguments here will fill from start to the end.
    /// This means that you don't need to provide the exact number of arguments. So if a command has 2 parameter
    /// but 1 is provided here, the user still need to provide 1 argument for the 2nd parameter.
    /// </summary>
    public object[] DefaultArgs { get; }
    
    /// <summary>
    /// If true, the chat message associated with this alias will not be displayed.
    /// </summary>
    public bool Hide { get; }
}
