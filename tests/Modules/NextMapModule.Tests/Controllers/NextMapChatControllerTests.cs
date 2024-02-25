using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.NextMapModule.Controllers;
using EvoSC.Modules.Official.NextMapModule.Interfaces;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Interfaces;
using NSubstitute;
using Xunit;

namespace EvoSC.Modules.Official.NextMapModule.Tests.Controllers;

public class NextMapChatControllerTests : CommandInteractionControllerTestBase<NextMapChatController>
{
    private readonly IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private readonly INextMapService _nextMapService = Substitute.For<INextMapService>();
    private readonly (IServerClient Client, IGbxRemoteClient Remote)
        _server = Mocking.NewServerClientMock();

    public NextMapChatControllerTests()
    {
        var locale = Mocking.NewLocaleMock(ContextService);
        InitMock(_actor, _nextMapService, _server.Client, locale);
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
        _nextMapService.GetNextMapAsync().Returns(Task.FromResult((IMap)map));

        await Controller.GetNextMapAsync();

        await _server.Client.Received().InfoMessageAsync(Arg.Any<string>());
    }
}
