using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Parsing;
using Moq;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests;

public class OpenPlanetModuleTests
{
    private Mock<IManialinkManager> _manialinks = new();
    private Mock<IOpenPlanetControlSettings> _settings = new();
    private Mock<IManialinkInteractionHandler> _manialinKInteractions = new();
    private Mock<IValueReaderManager> _valueReader = new();

    public OpenPlanetModuleTests()
    {
        _manialinKInteractions.Setup(m => m.ValueReader).Returns(_valueReader.Object);
    }
    
    [Fact]
    public async Task Reader_Is_Added_And_Detection_Manialink_Is_Sent_On_Enabled()
    {
        var module = new OpenPlanetModule(_manialinks.Object, _settings.Object, _manialinKInteractions.Object);

        await module.EnableAsync();

        _valueReader.Verify(m => m.AddReader(It.IsAny<OpenPlanetInfoValueReader>()), Times.Once);
        _manialinks.Verify(m => m.SendPersistentManialinkAsync("OpenPlanetModule.DetectOP", It.IsAny<It.IsAnyType>()), Times.Once);
    }
    
    [Fact]
    public async Task Reader_Is_Removed_And_Detection_Manialink_Hidden_On_Disable()
    {
        var module = new OpenPlanetModule(_manialinks.Object, _settings.Object, _manialinKInteractions.Object);

        await module.DisableAsync();

        _manialinks.Verify(m => m.HideManialinkAsync("OpenPlanetModule.DetectOP"), Times.Once);
        _valueReader.Verify(m => m.RemoveReaders(typeof(IOpenPlanetInfo)), Times.Once);
    }
}
