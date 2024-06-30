namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkTransaction : IManialinkOperations
{
    public Task CommitAsync();
}
