using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Events;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services;
using EvoSC.Testing;
using GbxRemoteNet;
using GbxRemoteNet.Interfaces;
using GbxRemoteNet.Structs;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Services;

public class PlayerCacheServiceTests
{
    private readonly Mock<ILogger<PlayerCacheService>> _logger = new();
    private readonly Mock<IEventManager> _eventManager = new();
    private readonly Mock<IPlayerRepository> _playerRepository = new();

    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat)
        _server = Mocking.NewServerClientMock();

    private readonly PlayerCacheService _playerCacheService;

    public PlayerCacheServiceTests()
    {
        _playerCacheService = new PlayerCacheService(_eventManager.Object, _server.Client.Object, _logger.Object,
            _playerRepository.Object);
    }

    [Fact]
    public async Task UpdatePlayerList_AnnouncesAlreadyOnlinePlayers()
    {
        var login = "snippen-Dv66ipzaSfqrJTXx2SGMlQ";
        var accountId = "0efeba8a-9cda-49fa-ab25-35f1d9218c95";

        var player = new DbPlayer
        {
            Id = 1,
            UbisoftName = "snippen",
            AccountId = accountId,
            NickName = "snippen"
        };

        _server.Remote.Setup(r => r.GetPlayerListAsync())
            .ReturnsAsync([
                new TmPlayerInfo { Login = "Server" },
                new TmPlayerInfo { Login = login }
            ]);

        _server.Remote.Setup(r => r.MultiCallAsync(It.IsAny<MultiCall>()))
            .ReturnsAsync([
                new GbxDynamicObject { { "Login", login }, { "Nickname", "snippen" } },
                new GbxDynamicObject { { "Login", login }, { "PlayerId", 1 }, { "TeamId", 0 } }
            ]);

        _playerRepository.Setup(r => r.GetPlayerByAccountIdAsync(It.IsAny<string>()))
            .ReturnsAsync(player);

        var raised = new List<PlayerJoinedEventArgs>();
        _eventManager.Setup(e => e.RaiseAsync(It.IsAny<Enum>(), It.IsAny<EventArgs>()))
            .Callback<Enum, EventArgs>((name, args) =>
            {
                if (name.Equals(PlayerEvents.PlayerJoined) && args is PlayerJoinedEventArgs joinedArgs)
                {
                    raised.Add(joinedArgs);
                }
            })
            .Returns(Task.CompletedTask);
        _eventManager.Setup(e => e.RaiseAsync(It.IsAny<string>(), It.IsAny<EventArgs>()))
            .Returns(Task.CompletedTask);

        await _playerCacheService.UpdatePlayerListAsync();

        // Players which were online before the controller connected never raise PlayerConnect, so the startup
        // sync has to announce them, otherwise nothing is ever sent to them.
        var joined = Assert.Single(raised);
        Assert.Equal(accountId, joined.Player.AccountId);
        Assert.False(joined.IsNewPlayer);
        Assert.Equal(accountId, Assert.Single(_playerCacheService.OnlinePlayers).AccountId);
    }
}
