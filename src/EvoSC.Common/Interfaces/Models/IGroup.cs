namespace EvoSC.Common.Interfaces.Models;

public interface IGroup
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public string Icon { get; }
    public string Color { get; }
    public bool Unrestricted { get; }
    public List<IPermission> Permissions { get; }
}
