using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.ServerManagementModule.Interfaces;
using EvoSC.Modules.Official.ServerManagementModule.Services;
using EvoSC.Testing;
using GbxRemoteNet.Interfaces;
using Moq;

namespace ServerManagementModule.Tests.Services;

public class ServerManagementServiceTests
{
    public (
        IServerManagementService ServerManagementService,
        (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote, Mock<IChatService> Chat) Server,
        Mock<IEventManager> EventManager
        ) NewServiceMock()
    {
        var server = Mocking.NewServerClientMock();
        var events = new Mock<IEventManager>();

        var serverManagementService = new ServerManagementService(server.Client.Object, events.Object);

        return (
            serverManagementService,
            server,
            events
        );
    }

    [Fact]
    public async Task SetPassword_Sets_Both_Player_And_Spectator_Password()
    {
        var mock = NewServiceMock();
        const string Password = "MyPassword123";

        await mock.ServerManagementService.SetPasswordAsync(Password);
        
        mock.Server.Remote.Verify(m => m.SetServerPasswordAsync(Password));
        mock.Server.Remote.Verify(m => m.SetServerPasswordForSpectatorAsync(Password));
    }

    [Fact]
    public async Task SetMaxPlayers_Sets_MaxPlayers()
    {
        var mock = NewServiceMock();
        const int MaxPlayers = 123;

        await mock.ServerManagementService.SetMaxPlayersAsync(MaxPlayers);
        
        mock.Server.Remote.Verify(m => m.SetMaxPlayersAsync(MaxPlayers));
    }
    
    [Fact]
    public async Task SetMaxSpectators_Sets_MaxSpectators()
    {
        var mock = NewServiceMock();
        const int MaxSpectators = 123;

        await mock.ServerManagementService.SetMaxSpectatorsAsync(MaxSpectators);
        
        mock.Server.Remote.Verify(m => m.SetMaxSpectatorsAsync(MaxSpectators));
    }
}
