using EvoSC.Common.Database.Models;
using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IPermissionManager
{
    /// <summary>
    /// Check if a player has the given permission.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="permission">Permission the user requires.</param>
    /// <returns>True if the permission is assigned to the user, or if the user is in a unrestricted group.</returns>
    public Task<bool> HasPermissionAsync(IPlayer player, string permission);
    
    /// <summary>
    /// Get information about a permission from it's name.
    /// </summary>
    /// <param name="name">The name of the permission.</param>
    /// <returns>Permission information.</returns>
    public Task<IPermission?> GetPermission(string name);
    
    /// <summary>
    /// Add a new permission to the database.
    /// </summary>
    /// <param name="permission">The permission to add, with name and description.</param>
    /// <returns></returns>
    public Task AddPermission(IPermission permission);
    
    /// <summary>
    /// Update a permission in the database.
    /// </summary>
    /// <param name="permission">New information about the permission.</param>
    /// <returns></returns>
    public Task UpdatePermission(IPermission permission);
    
    /// <summary>
    /// Remove a permission from the database. This method cleans all relations to the permission in the database.
    /// </summary>
    /// <param name="name">The name of the permission to remove.</param>
    /// <returns></returns>
    public Task RemovePermission(string name);
    
    /// <summary>
    /// Remove a permission from the database. This method cleans all relations to the permission in the database.
    /// </summary>
    /// <param name="permission">The permission to remove.</param>
    /// <returns></returns>
    public Task RemovePermission(IPermission permission);
    
    /// <summary>
    /// Add a new user group to the database.
    /// </summary>
    /// <param name="group">Object containing information about the new group.</param>
    /// <returns></returns>
    public Task AddGroup(IGroup group);
    
    /// <summary>
    /// Remove a group from the database. This method cleans up any relations to the group.
    /// </summary>
    /// <param name="id">The ID of the group to remove.</param>
    /// <returns></returns>
    public Task RemoveGroup(int id);
    
    /// <summary>
    /// Remove a group from the database. This method cleans up any relations to the group.
    /// </summary>
    /// <param name="group">The group to remove.</param>
    /// <returns></returns>
    public Task RemoveGroup(IGroup group);
    
    /// <summary>
    /// Remove a group from the database. This method cleans up any relations to the group.
    /// </summary>
    /// <param name="group">The group to remove.</param>
    /// <returns></returns>
    public Task UpdateGroup(IGroup group);
    
    /// <summary>
    /// Get information about a group.
    /// </summary>
    /// <param name="id">ID of the group to get.</param>
    /// <returns></returns>
    public Task<IGroup?> GetGroup(int id);
    
    /// <summary>
    /// Add a player to a group, giving them all permissions that are assigned to the group.
    /// </summary>
    /// <param name="player">The player that should be added to the group.</param>
    /// <param name="group">The group to add to the player.</param>
    /// <returns></returns>
    public Task AddPlayerToGroup(IPlayer player, IGroup group);
    
    /// <summary>
    /// Remove a player from a group. This removes all permissions assigned to the player from the group.
    /// </summary>
    /// <param name="player">The player that should be removed from the group.</param>
    /// <param name="group">The group which the player should be removed from.</param>
    /// <returns></returns>
    public Task RemovePlayerFromGroup(IPlayer player, IGroup group);
    
    /// <summary>
    /// Add a permission to a group.
    /// </summary>
    /// <param name="group">The group to add the permission to.</param>
    /// <param name="permission">The permission to add.</param>
    /// <returns></returns>
    public Task AddPermissionToGroup(IGroup group, IPermission permission);
    
    /// <summary>
    /// Remove a permission from a group.
    /// </summary>
    /// <param name="group">The group to remove the permission from.</param>
    /// <param name="permission">The permission to remove.</param>
    /// <returns></returns>
    public Task RemovePermissionFromGroup(IGroup group, IPermission permission);
    
    /// <summary>
    /// Remove all permissions from a group.
    /// </summary>
    /// <param name="group">The group to clear permissions from.</param>
    /// <returns></returns>
    public Task ClearGroupPermissions(IGroup group);
}
