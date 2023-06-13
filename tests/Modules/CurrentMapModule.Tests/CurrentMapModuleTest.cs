using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using Moq;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapModuleTest
{
    private readonly CurrentMapModule _module;
    private readonly Mock<ICurrentMapService> _serviceMock = new();

    public CurrentMapModuleTest()
    {
        _module = new CurrentMapModule(_serviceMock.Object);
    }

    [Fact]
    async Task Should_Enable_Async()
    {
        await _module.EnableAsync();

        _serviceMock.Verify(service => service.ShowWidgetAsync());
    }

    [Fact]
    async Task Should_Disable_Async()
    {
        await _module.DisableAsync();

        _serviceMock.Verify(service => service.HideWidgetAsync());
    }
}
