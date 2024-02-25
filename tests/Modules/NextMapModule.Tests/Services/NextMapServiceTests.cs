using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.NextMapModule.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace EvoSC.Modules.Official.NextMapModule.Tests.Services;

public class NextMapServiceTests
{
    private readonly ILogger<NextMapService> _mockLogger = Substitute.For<ILogger<NextMapService>>();
    private readonly IMapService _mockMapService = Substitute.For<IMapService>();

    private readonly NextMapService _nextMapService;

    public NextMapServiceTests()
    {
        _nextMapService = new(_mockLogger, _mockMapService);
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
        _mockMapService.GetNextMapAsync().Returns(map);

        await _nextMapService.GetNextMapAsync();

        await _mockMapService.Received(1).GetNextMapAsync();
    }

    [Fact]
    public async Task GetNextMapAsync_Should_Not_Get_Next_Map_If_Null()
    {
        _mockMapService.GetNextMapAsync().Returns((IMap?)null);
        await Assert.ThrowsAsync<InvalidOperationException>(() => _nextMapService.GetNextMapAsync());
        await _mockMapService.Received(1).GetNextMapAsync();
    }
}
