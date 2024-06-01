using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.MapListModule.Controllers;
using EvoSC.Modules.Official.MapListModule.Interfaces;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.MapListModule.Tests.Controllers;

public class MapListCommandControllerTests : CommandInteractionControllerTestBase<MapListCommandController>
{
    private Mock<IMapListService> _mapListService = new();
    private Mock<IOnlinePlayer> _player = new();
    
    public MapListCommandControllerTests()
    {
        InitMock(_player.Object, _mapListService);
    }

    [Fact]
    public async Task MapList_Is_Shown()
    {
        await Controller.MapListAsync();

        _mapListService.Verify(m => m.ShowMapListAsync(_player.Object));
    }
}
