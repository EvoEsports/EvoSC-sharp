using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.MapListModule.Controllers;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.MapListModule.Tests.Controllers;

public class MapListCommandControllerTests : CommandInteractionControllerTestBase<MapListCommandController>
{
    private Mock<IMapListService> _mapListService = new();
    private Mock<IMapService> _mapService = new();
    private Mock<IChatService> _chat = new();
    private Mock<IOnlinePlayer> _player = new();
    
    public MapListCommandControllerTests()
    {
        InitMock(_player.Object, _mapListService, _mapService, _chat);
    }

    [Fact]
    public async Task MapList_Is_Shown()
    {
        await Controller.MapListAsync();

        _mapListService.Verify(m => m.OpenMapListAsync(_player.Object));
    }
}
