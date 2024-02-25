using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using NSubstitute;

namespace EvoSC.Modules.Official.CurrentMapModule.Tests;

public class CurrentMapModuleTest
{
    private readonly CurrentMapModule _module;
    private readonly ICurrentMapService _serviceMock = Substitute.For<ICurrentMapService>();

    public CurrentMapModuleTest()
    {
        _module = new CurrentMapModule(_serviceMock);
    }

    [Fact]
    async Task Should_Enable_Async()
    {
        await _module.EnableAsync();

        await _serviceMock.Received().ShowWidgetAsync();
    }

    [Fact]
    async Task Should_Disable_Async()
    {
        await _module.DisableAsync();

        await _serviceMock.Received().HideWidgetAsync();
    }
}
