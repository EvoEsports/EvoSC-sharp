using System.Diagnostics.CodeAnalysis;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using EvoSC.Modules.Official.WorldRecordModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.CurrentMapModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class CurrentMapService : ICurrentMapService
{
    private readonly ILogger<CurrentMapService> _logger;
    private readonly IManialinkManager _manialinkManager;
    private readonly IMapRepository _mapRepository;
    private readonly IServerClient _client;
    private readonly IWorldRecordService _worldRecordService;

    public CurrentMapService(IManialinkManager manialinkManager, ILogger<CurrentMapService> logger,
        IMapRepository mapRepository, IServerClient client, IWorldRecordService worldRecordService)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        _mapRepository = mapRepository;
        _client = client;
        _worldRecordService = worldRecordService;
    }

    [ExcludeFromCodeCoverage(Justification = "GBXRemoteClient cannot be mocked.")]
    public async Task ShowWidgetAsync()
    {
        var map = await _client.Remote.GetCurrentMapInfoAsync();
        await ShowManialinkAsync(map.UId);
    }

    public async Task ShowWidgetAsync(MapGbxEventArgs args)
    {
        await ShowManialinkAsync(args.Map.Uid);
    }

    public async Task HideWidgetAsync()
    {
        await _manialinkManager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget");
        _logger.LogDebug("Hiding current map widget");
    }

    private async Task ShowManialinkAsync(string mapUId)
    {
        var dbMap = await _mapRepository.GetMapByUidAsync(mapUId);
        var author = "";
        var worldRecord = await _worldRecordService.GetRecordAsync();
        if (dbMap.Author.NickName == dbMap.Author.AccountId)
        {
            var serverMap = await _client.Remote.GetCurrentMapInfoAsync();
            author = serverMap.AuthorNickname.Length > 0 ? serverMap.AuthorNickname : serverMap.Author;
        }
        else
        {
            author = dbMap.Author?.NickName;
        }
        await _manialinkManager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            new
            {
                map = dbMap,
                mapauthor = author,
                record = worldRecord
            });
        _logger.LogDebug("Showing current map widget");
    }
}
