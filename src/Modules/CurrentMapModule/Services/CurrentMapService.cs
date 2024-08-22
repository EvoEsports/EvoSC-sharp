using System.Diagnostics.CodeAnalysis;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Config;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.CurrentMapModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class CurrentMapService(
    IManialinkManager manialinkManager,
    ILogger<CurrentMapService> logger,
    IMapRepository mapRepository,
    IServerClient client,
    ICurrentMapSettings settings
)
    : ICurrentMapService
{
    [ExcludeFromCodeCoverage(Justification = "GBXRemoteClient cannot be mocked.")]
    public async Task ShowWidgetAsync()
    {
        var map = await client.Remote.GetCurrentMapInfoAsync();
        await ShowManialinkAsync(map.UId, map.AuthorTime);
    }

    public async Task ShowWidgetAsync(MapGbxEventArgs args)
    {
        await ShowManialinkAsync(args.Map.Uid, args.Map.AuthorTime);
    }

    public async Task HideWidgetAsync()
    {
        await manialinkManager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget");
        logger.LogDebug("Hiding current map widget");
    }

    private async Task ShowManialinkAsync(string mapUId, int authorTime)
    {
        var dbMap = await mapRepository.GetMapByUidAsync(mapUId);

        if (dbMap == null)
        {
            return;
        }

        var authorNickName = dbMap.Author?.NickName ?? "Unknown";

        if (authorNickName == dbMap.Author?.AccountId)
        {
            var serverMap = await client.Remote.GetCurrentMapInfoAsync();
            authorNickName = serverMap.AuthorNickname.Length > 0 ? serverMap.AuthorNickname : serverMap.Author;
        }

        logger.LogDebug("Current map {mapName} by {author} ({authorTime}).", dbMap.Name, authorNickName, authorTime);
        
        await manialinkManager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            new { map = dbMap, mapAuthor = authorNickName, authorTime, settings });
        
        logger.LogDebug("Showing current map widget");
    }
}
