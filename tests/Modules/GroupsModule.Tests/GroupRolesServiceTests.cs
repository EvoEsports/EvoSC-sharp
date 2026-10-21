using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.GroupsModule.Database.Repository;
using EvoSC.Modules.Official.GroupsModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Database;
using GbxRemoteNet.Structs;
using Xunit;

namespace EvoSC.Modules.Official.GroupsModule.Tests;

public class GroupRolesServiceTests
{
    private const int SuperAdminGroupId = 1;
    private const int PlayerGroupId = 2;

    private readonly PermissionRepository _permissionRepository;
    private readonly PlayerRepository _playerRepository;
    private readonly GroupsRepository _groupsRepository;
    private readonly GroupRolesService _roles;

    public GroupRolesServiceTests()
    {
        var factory = TestDbSetup.CreateDb(typeof(AddPlayersTable).Assembly, typeof(GroupsModule).Assembly);

        _permissionRepository = new PermissionRepository(factory, TestLoggerSetup.CreateLogger<PermissionRepository>());
        _playerRepository = new PlayerRepository(factory, _permissionRepository);
        _groupsRepository = new GroupsRepository(factory);
        _roles = new GroupRolesService(_groupsRepository);
    }

    private async Task<IPlayer> AddPlayerAsync(string accountId, params int[] groupIds)
    {
        var player = await _playerRepository.AddPlayerAsync(accountId,
            new TmPlayerDetailedInfo { NickName = accountId, Path = "World" });

        foreach (var groupId in groupIds)
        {
            await _permissionRepository.AddPlayerToGroupAsync(player.Id, groupId);
        }

        return player;
    }

    [Fact]
    public async Task Player_In_Visible_Group_Has_Role()
    {
        var player = await AddPlayerAsync("admin", SuperAdminGroupId);
        await _groupsRepository.SetScoreboardVisibleAsync(SuperAdminGroupId, true);

        var roles = await _roles.GetRolesAsync([player]);

        var role = Assert.Single(roles).Value;
        Assert.Equal(SuperAdminGroupId, role.GroupId);
        Assert.Equal("Super Admin", role.Title);
        Assert.Equal("F00", role.Color);
    }

    [Fact]
    public async Task Player_In_Hidden_Group_Has_No_Role()
    {
        var player = await AddPlayerAsync("admin", SuperAdminGroupId);

        Assert.Empty(await _roles.GetRolesAsync([player]));
    }

    [Fact]
    public async Task Role_Is_Lowest_Id_Of_Visible_Groups()
    {
        var player = await AddPlayerAsync("admin", PlayerGroupId, SuperAdminGroupId);
        await _groupsRepository.SetScoreboardVisibleAsync(PlayerGroupId, true);
        await _groupsRepository.SetScoreboardVisibleAsync(SuperAdminGroupId, true);

        var roles = await _roles.GetRolesAsync([player]);

        Assert.Equal(SuperAdminGroupId, Assert.Single(roles).Value.GroupId);
    }

    [Fact]
    public async Task Hiding_Group_Removes_Role()
    {
        var player = await AddPlayerAsync("admin", SuperAdminGroupId);
        await _groupsRepository.SetScoreboardVisibleAsync(SuperAdminGroupId, true);
        await _groupsRepository.SetScoreboardVisibleAsync(SuperAdminGroupId, false);

        Assert.Empty(await _roles.GetRolesAsync([player]));
    }
}
