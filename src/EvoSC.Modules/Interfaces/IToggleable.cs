namespace EvoSC.Modules;

public interface IToggleable
{
    public Task Enable();
    public Task Disable();
}
