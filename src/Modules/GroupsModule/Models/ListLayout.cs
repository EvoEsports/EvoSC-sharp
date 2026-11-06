namespace EvoSC.Modules.Official.GroupsModule.Models;

/// <summary>
/// Dimensions of the scrollable lists in the groups windows.
/// </summary>
public static class ListLayout
{
    /// <summary>Height of the scrollable area.</summary>
    public const double ListHeight = 54;

    /// <summary>Distance between the tops of two rows in the group lists.</summary>
    public const double GroupRowPitch = 7;

    /// <summary>Distance between the tops of two rows in the permission list.</summary>
    public const double PermissionRowPitch = 8;

    /// <summary>Empty space between two rows.</summary>
    public const double RowGap = 1;

    /// <summary>
    /// How far a list of rows has to scroll to show its last row, which is the content height minus the visible height.
    /// </summary>
    public static double ScrollHeight(int rowCount, double rowPitch) =>
        Math.Max(0, rowCount * rowPitch - RowGap - ListHeight);
}
