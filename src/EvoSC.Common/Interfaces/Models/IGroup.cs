namespace EvoSC.Common.Interfaces.Models;

public interface IGroup
{
    public int Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool Unrestricted { get; set; }
    public List<IPermission> Permissions { get; }
}
