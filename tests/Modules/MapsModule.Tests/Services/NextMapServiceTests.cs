using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.Maps.Services;
using EvoSC.Testing;
using Microsoft.Extensions.Logging;
using Moq;

namespace EvoSC.Modules.Official.MapsModule.Tests.Services;

public class NextMapServiceTests
{
    private const string Template = "Maps.NextMap";

    private readonly Mock<ILogger<NextMapService>> _mockLogger = new();
    private readonly Mock<IMapService> _mockMapService = new();

    private readonly NextMapService _nextMapService;

    public NextMapServiceTests()
    {
        _nextMapService = new(_mockLogger.Object, _mockMapService.Object);
    }

    [Fact]
    public async Task GetNextMapAsync_Should_Get_Next_Map()
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
        _mockMapService.Setup(r => r.GetNextMapAsync()).ReturnsAsync(map);

        await _nextMapService.GetNextMapAsync();

        _mockMapService.Verify(r => r.GetNextMapAsync(), Times.Once);
    }

    [Fact]
    public async Task GetNextMapAsync_Should_Not_Get_Next_Map_If_Null()
    {
        _mockMapService.Setup(r => r.GetNextMapAsync()).ReturnsAsync((IMap?)null);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _nextMapService.GetNextMapAsync());
        _mockMapService.Verify(r => r.GetNextMapAsync(), Times.Once);
    }
}
