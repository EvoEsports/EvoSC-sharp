namespace EvoSC.Common.Interfaces.Models;

public interface IPermission
{
    public int Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
}
