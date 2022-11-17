namespace EvoSC.Common.Interfaces.Models;

public interface IGroup
{
    /// <summary>
    /// Database ID of the group.
    /// </summary>
    public int Id { get; }
    /// <summary>
    /// The title name of the group.
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// A summary describing the group.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Group icon displayed, should be a single character.
    /// </summary>
    public string? Icon { get; set; }
    /// <summary>
    /// The color of the group that is displayed, must be in TM color format without the dollar sign (RGB).
    /// </summary>
    public string? Color { get; set; }
    /// <summary>
    /// Whether this group has unrestricted permissions. If true, the group has all permissions available.
    /// </summary>
    public bool Unrestricted { get; set; }
    /// <summary>
    /// List of permissions assigned to this group.
    /// </summary>
    public List<IPermission> Permissions { get; }
}
