namespace EvoSC.Commands.Interfaces;

public interface ICommandAlias
{
    public string Name { get; }
    public object[] DefaultArgs { get; }
}
