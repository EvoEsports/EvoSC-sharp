using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Config;
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
    ILogger<LocalRecordsService> logger,
    ILocalRecordsSettings settings) : ILocalRecordsService
{
    private const string WidgetName = "LocalRecordsModule.LocalRecordsWidget";
    
    public async Task<ILocalRecord[]> GetLocalsOfCurrentMapFromPosAsync()
    {
        var currentMap = await mapService.GetCurrentMapAsync();

        if (currentMap == null)
        {
            throw new InvalidOperationException("Failed to get current map");
        }

        var records = (IEnumerable<ILocalRecord>)await localRecordRepository.GetLocalRecordsOfMapByIdAsync(currentMap.Id);
        return records.ToArray();
    }

    public async Task ShowWidgetAsync(IPlayer player)
    {
        var records = await GetLocalsOfCurrentMapFromPosAsync();
        var playerRecords = GetRecordsWithPlayer(player, records);
        await manialinkManager.SendManialinkAsync(player, WidgetName,
            new { currentPlayer = player, records = playerRecords });
    }

    public async Task ShowWidgetToAllAsync()
    {
        var records = await GetLocalsOfCurrentMapFromPosAsync();
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        var transaction = manialinkManager.CreateTransaction();

        try
        {
            foreach (var player in onlinePlayers)
            {
                var playerRecords = GetRecordsWithPlayer(player, records);
                await transaction.SendManialinkAsync(player, WidgetName, new { currentPlayer = player, records = playerRecords });
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send local records widget");
        }
    }

    private ILocalRecord[] GetRecordsWithPlayer(IPlayer player, ILocalRecord[] records)
    {
        if (records.Length == 0)
        {
            return records;
        }
        
        var playerRecord = records.FirstOrDefault(r => r.Record.Player.Id == player.Id);
        var topMaxRows = Math.Min(settings.MaxWidgetRows, records.Length);

        // if player doesn't exist, just return the top players
        if (playerRecord == null)
        {
            return records[..topMaxRows];
        }

        // if records around player overlaps with top players, just return first MaxRows records
        if (playerRecord.Position <= settings.MaxWidgetRows)
        {
            return records[..topMaxRows];
        }
        
        // return top records + records around the player
        var topRecords = records[..Math.Min(settings.WidgetShowTop, records.Length)];

        var maxRange = settings.MaxWidgetRows - settings.WidgetShowTop;
        var half = maxRange / 2;
        var r = maxRange % 2;

        var lower = playerRecord.Position - half;
        var upper = lower + maxRange;

        // if the range is outside the upper bound, let's start from the last record instead
        if (upper > records.Length)
        {
            lower -= upper - records.Length;
            upper = records.Length;

            lower++;
            upper++;
        }

        return [..topRecords, ..records[(lower - 1)..(upper - 1)]];
    }
}
