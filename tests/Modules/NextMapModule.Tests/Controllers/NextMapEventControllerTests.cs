﻿using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.NextMapModule.Config;
using EvoSC.Modules.Official.NextMapModule.Controllers;
using EvoSC.Modules.Official.NextMapModule.Interfaces;
using EvoSC.Testing.Controllers;
using GbxRemoteNet.Events;
using Moq;
using Xunit;

namespace EvoSC.Modules.Official.NextMapModule.Tests.Controllers;

public class NextMapEventControllerTests : ControllerMock<NextMapEventController, IEventControllerContext>
{
    private const string Template = "NextMapModule.NextMap";

    private readonly Mock<INextMapService> _nextMapService = new();
    private readonly Mock<IManialinkManager> _manialinkManager = new();
    private readonly Mock<INextMapSettings> _settings = new();

    public NextMapEventControllerTests()
    {
        InitMock(_nextMapService, _manialinkManager, _settings);
    }

    [Fact]
    public async Task OnPodiumStart_Shows_Next_Map()
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
        _nextMapService.Setup(r => r.GetNextMapAsync()).Returns(Task.FromResult((IMap) map));

        await Controller.ShowNextMapOnPodiumStartAsync(new(), new PodiumEventArgs
        {
            Time = 0
        });
        
        _manialinkManager.Verify(m => m.SendManialinkAsync(Template, It.IsAny<object>()), Times.Once());
    }

    [Fact]
    public async Task OnBeginMap_Hides_Next_Map()
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
        _nextMapService.Setup(r => r.GetNextMapAsync()).Returns(Task.FromResult((IMap) map));
        
        await Controller.HideNextMapOnBeginMapAsync(null, new MapGbxEventArgs());
        _manialinkManager.Verify(r => r.HideManialinkAsync(Template), Times.Once);
    }
}
