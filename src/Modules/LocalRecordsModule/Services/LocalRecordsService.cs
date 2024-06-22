using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LocalRecordsModule.Services;

[Service]
public class LocalRecordsService(
    IMapService mapService,
    ILocalRecordRepository localRecordRepository,
    IPlayerManagerService playerManagerService,
    IManialinkManager manialinkManager,
    ILogger<LocalRecordsService> logger) : ILocalRecordsService
{
    private const string WidgetName = "LocalRecordsModule.LocalRecordsWidget";
    
    public async Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapAsync()
    {
        var currentMap = await mapService.GetCurrentMapAsync();

        if (currentMap == null)
        {
            throw new InvalidOperationException("Failed to get current map");
        }

        return await localRecordRepository.GetLocalRecordsOfMapByIdAsync(currentMap.Id);
    }

    public async Task ShowWidgetAsync(IPlayer player)
    {
        var records = await GetLocalsOfCurrentMapAsync();
        await manialinkManager.SendManialinkAsync(player, WidgetName, new { currentPlayer = player, records });
    }

    public async Task ShowWidgetToAllAsync()
    {
        var records = await GetLocalsOfCurrentMapAsync();
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        var transaction = manialinkManager.CreateTransaction();

        try
        {
            foreach (var player in onlinePlayers)
            {
                await transaction.SendManialinkAsync(player, WidgetName, new { currentPlayer = player, records });
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send local records widget");
        }
    }
}
