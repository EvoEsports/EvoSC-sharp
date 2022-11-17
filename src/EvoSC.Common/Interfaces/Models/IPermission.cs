namespace EvoSC.Common.Interfaces.Models;

public interface IPermission
{
    /// <summary>
    /// The database ID of the permission.
    /// </summary>
    public int Id { get; }
    /// <summary>
    /// Unique name that identifies the permission.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Summary describing the permission.
    /// </summary>
    public string Description { get; set; }
}
