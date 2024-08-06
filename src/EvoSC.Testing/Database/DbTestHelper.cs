using EvoSC.Common.Database.Repository.Permissions;
using EvoSC.Common.Database.Repository.Players;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Structs;

namespace EvoSC.Testing.Database;

public static class DbTestHelper
{
    public static async Task<IPlayer> AddTestPlayer(IDbConnectionFactory factory, string accountId)
    {
        var logger = TestLoggerSetup.CreateLogger<PermissionRepository>();
        var permissionRepo = new PermissionRepository(factory, logger);
        var playerRepo = new PlayerRepository(factory, permissionRepo);
        
        return await playerRepo.AddPlayerAsync(accountId, new TmPlayerDetailedInfo
        {
            NickName = "TestAccount",
            Path = "World"
        });
    }

    public static Task<IPlayer> AddTestPlayer(IDbConnectionFactory factory) => AddTestPlayer(factory, "TestAccountId");
}
