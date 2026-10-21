using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Validation;
using EvoSC.Modules.Official.GroupsModule.Models;

namespace EvoSC.Modules.Official.GroupsModule.Interfaces;

public interface IGroupsService
{
    /// <summary>
    /// Show the list of all groups.
    /// </summary>
    public Task ShowGroupListAsync(IPlayer player);

    /// <summary>
    /// Show the form for creating a group, or editing one if a group ID is given.
    /// </summary>
    /// <param name="player">The player to show the form to.</param>
    /// <param name="groupId">The group to edit, or null to create a new one.</param>
    /// <param name="input">Values to fill in, e.g. after a failed validation. Defaults to the values of the group.</param>
    /// <param name="validation">Validation results of the input.</param>
    public Task ShowGroupEditorAsync(IPlayer player, int? groupId, GroupEditModel? input = null,
        FormValidationResult? validation = null);

    /// <summary>
    /// Show the window for choosing which permissions a group has.
    /// </summary>
    public Task ShowGroupPermissionsAsync(IPlayer player, int groupId);

    /// <summary>
    /// Ask a player to confirm deleting a group.
    /// </summary>
    public Task ShowDeleteGroupDialogAsync(IPlayer player, int groupId);

    /// <summary>
    /// Show a page of the players that have been on the server, with their groups.
    /// </summary>
    public Task ShowPlayerListAsync(IPlayer player, string? search, int page);

    /// <summary>
    /// Show the window for choosing which groups a player is in.
    /// </summary>
    public Task ShowPlayerGroupsAsync(IPlayer player, string accountId, string? search, int page);

    /// <summary>
    /// Create a group, or update it if a group ID is given.
    /// </summary>
    /// <returns>The ID of the group, or null if it could not be saved. The actor is told why.</returns>
    public Task<int?> SaveGroupAsync(IPlayer actor, int? groupId, GroupEditModel input);

    /// <summary>
    /// Delete a group.
    /// </summary>
    /// <returns>True if the group was deleted. If not, the actor is told why.</returns>
    public Task<bool> DeleteGroupAsync(IPlayer actor, int groupId);

    /// <summary>
    /// Give a group a permission if it does not have it, otherwise take it away.
    /// </summary>
    /// <returns>The name of the permission and whether the group has it now, or null if there was no such group or permission.</returns>
    public Task<(string Name, bool Granted)?> TogglePermissionAsync(int groupId, int permissionId);

    /// <summary>
    /// Add a player to a group if they are not in it, otherwise remove them from it.
    /// </summary>
    /// <returns>Whether the player is in the group now, or null if it could not be changed. The actor is told why.</returns>
    public Task<bool?> TogglePlayerGroupAsync(IPlayer actor, string accountId, int groupId);
}
