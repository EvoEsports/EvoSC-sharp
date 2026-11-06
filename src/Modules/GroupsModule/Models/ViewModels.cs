namespace EvoSC.Modules.Official.GroupsModule.Models;

/// <summary>
/// A group as shown in the group list.
/// </summary>
public record GroupListItem(
    int Id,
    string Title,
    string Icon,
    string Color,
    bool Unrestricted,
    bool IsBuiltIn,
    int MemberCount,
    int PermissionCount);

/// <summary>
/// A permission and whether a group has it, as shown in the group permissions window.
/// </summary>
public record PermissionToggle(int Id, string Name, string Description, bool Granted);

/// <summary>
/// A group and whether a player is in it, as shown in the player groups window.
/// </summary>
public record GroupToggle(int Id, string Title, string Icon, string Color, bool Unrestricted, bool Member);

/// <summary>
/// A player as shown in the player list.
/// </summary>
public record PlayerListItem(string AccountId, string NickName, string GroupTitles);
