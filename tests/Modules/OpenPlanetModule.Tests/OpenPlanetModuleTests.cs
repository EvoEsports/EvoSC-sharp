using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Parsing;
using NSubstitute;

namespace EvoSC.Modules.Official.OpenPlanetModule.Tests;

public class OpenPlanetModuleTests
{
    private readonly IManialinkManager _manialinks = Substitute.For<IManialinkManager>();
    private readonly IOpenPlanetControlSettings _settings = Substitute.For<IOpenPlanetControlSettings>();
    private readonly IManialinkInteractionHandler _manialinKInteractions = Substitute.For<IManialinkInteractionHandler>();
    private readonly IValueReaderManager _valueReader = Substitute.For<IValueReaderManager>();

    public OpenPlanetModuleTests()
    {
        _manialinKInteractions.ValueReader.Returns(_valueReader);
    }
    
    [Fact]
    public async Task Reader_Is_Added_And_Detection_Manialink_Is_Sent_On_Enabled()
    {
        var module = new OpenPlanetModule(_manialinks, _settings, _manialinKInteractions);

        await module.EnableAsync();

        _valueReader.Received(1).AddReader(Arg.Any<OpenPlanetInfoValueReader>());
        await _manialinks.Received(1).SendPersistentManialinkAsync("OpenPlanetModule.DetectOP", Arg.Any<object>());
    }
    
    [Fact]
    public async Task Reader_Is_Removed_And_Detection_Manialink_Hidden_On_Disable()
    {
        var module = new OpenPlanetModule(_manialinks, _settings, _manialinKInteractions);

        await module.DisableAsync();

        _valueReader.Received(1).RemoveReaders(typeof(IOpenPlanetInfo));
        await _manialinks.Received(1).HideManialinkAsync("OpenPlanetModule.DetectOP");
    }
}
