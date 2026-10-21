namespace EvoSC.Modules.Official.GroupsModule.Models;

/// <summary>
/// The group of a player that is shown to others as their role.
/// </summary>
/// <param name="GroupId">ID of the group.</param>
/// <param name="Title">Title of the group.</param>
/// <param name="Icon">Icon of the group, empty if it has none.</param>
/// <param name="Color">Color of the group in TM color format without the dollar sign.</param>
public record PlayerRole(int GroupId, string Title, string Icon, string Color);
