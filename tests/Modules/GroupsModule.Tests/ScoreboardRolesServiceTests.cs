using EvoSC.Common.Database.Migrations;
using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Players;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Database.Repository;
using EvoSC.Modules.Official.GroupsModule.Services;
using EvoSC.Modules.Official.ScoreboardModule.Services;
using EvoSC.Testing;
using EvoSC.Testing.Database;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.GroupsModule.Tests;

public class ScoreboardRolesServiceTests
{
    [Fact]
    public async Task Roles_Manialink_Contains_Role_Of_Online_Player_By_Login()
    {
        var factory = TestDbSetup.CreateDb(typeof(AddPlayersTable).Assembly, typeof(GroupsModule).Assembly);
        var permissionRepository = new PermissionRepository(factory, TestLoggerSetup.CreateLogger<PermissionRepository>());
        var playerRepository = new PlayerRepository(factory, permissionRepository);
        var groupsRepository = new GroupsRepository(factory);

        var player = await playerRepository.AddPlayerAsync("0a1b2c3d-0000-4000-8000-000000000001",
            new TmPlayerDetailedInfo { NickName = "admin", Path = "World" });
        var other = await playerRepository.AddPlayerAsync("0a1b2c3d-0000-4000-8000-000000000002",
            new TmPlayerDetailedInfo { NickName = "other", Path = "World" });
        await permissionRepository.AddPlayerToGroupAsync(player.Id, 1);
        await groupsRepository.SetScoreboardVisibleAsync(1, true);

        var online = new[]
        {
            new OnlinePlayer(player) { State = PlayerState.Playing },
            new OnlinePlayer(other) { State = PlayerState.Playing }
        };

        var players = new Mock<IPlayerManagerService>();
        players.Setup(p => p.GetOnlinePlayersAsync()).ReturnsAsync(online);

        object? sentData = null;
        var manialinks = new Mock<IManialinkManager>();
        manialinks
            .Setup(m => m.SendPersistentManialinkAsync(It.IsAny<string>(), It.IsAny<object>()))
            .Callback<string, object>((_, data) => sentData = data);

        var service = new ScoreboardRolesService(players.Object, new GroupRolesService(groupsRepository),
            manialinks.Object, NullLogger<ScoreboardRolesService>.Instance);

        await service.SendRolesManialinkAsync();

        Assert.NotNull(sentData);
        var roles = (string)sentData!.GetType().GetProperty("roles")!.GetValue(sentData)!;
        Assert.StartsWith($"[\"{player.GetLogin()}\" => \"$F00", roles);
        Assert.EndsWith("Super Admin\"]", roles);
        Assert.DoesNotContain(other.GetLogin(), roles);
    }
}
