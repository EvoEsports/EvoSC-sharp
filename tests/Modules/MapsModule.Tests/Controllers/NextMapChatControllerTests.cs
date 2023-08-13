using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.Maps.Controllers;
using EvoSC.Modules.Official.Maps.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using Moq;

namespace EvoSC.Modules.Official.MapsModule.Tests.Controllers;

public class NextMapChatControllerTests : CommandInteractionControllerTestBase<NextMapChatController>
{
    private readonly Mock<IOnlinePlayer> _actor = new();
    private readonly Mock<INextMapService> _nextMapService = new();
    private readonly (Mock<IServerClient> Client, Mock<IGbxRemoteClient> Remote)
        _server = Mocking.NewServerClientMock();

    private readonly Locale _locale;

    public NextMapChatControllerTests()
    {
        _locale = Mocking.NewLocaleMock(ContextService.Object);
        InitMock(_actor.Object, _nextMapService, _server.Client, _locale);
    }

    [Fact]
    public async Task Get_Next_Map_Sends_Chat_Message()
    {
        var map = new DbMap
        {
            AuthorId = 1,
            Enabled = true,
            Id = 123,
            ExternalId = "1337",
            Name = "snippens dream",
            Uid = "Uid"
        };
        _nextMapService.Setup(m => m.GetNextMapAsync()).Returns(Task.FromResult((IMap)map));

        await Controller.GetNextMapAsync();

        _server.Client.Verify(c => c.InfoMessageAsync(It.IsAny<string>()));
    }
}
