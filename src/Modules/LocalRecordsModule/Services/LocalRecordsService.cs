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
    
    public async Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapFromPosAsync()
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
        var records = await GetLocalsOfCurrentMapFromPosAsync();
        var topRecords = records.Take(Math.Min(settings.WidgetShowTop, settings.MaxWidgetRows));
        var playerRecords = GetRecordsForPlayer(records, topRecords, player);
        await manialinkManager.SendManialinkAsync(player, WidgetName,
            new { currentPlayer = player, records = playerRecords });
    }

    public async Task ShowWidgetToAllAsync()
    {
        var records = await GetLocalsOfCurrentMapFromPosAsync();
        var topRecords = records.Take(Math.Min(settings.WidgetShowTop, settings.MaxWidgetRows));
        var onlinePlayers = await playerManagerService.GetOnlinePlayersAsync();
        var transaction = manialinkManager.CreateTransaction();

        try
        {
            foreach (var player in onlinePlayers)
            {
                var playerRecords = GetRecordsForPlayer(records, topRecords, player);
                await transaction.SendManialinkAsync(player, WidgetName, new { currentPlayer = player, records = playerRecords });
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send local records widget");
        }
    }

    private IEnumerable<ILocalRecord> GetRecordsForPlayer(IEnumerable<ILocalRecord> records, IEnumerable<ILocalRecord> topRecords, IPlayer player)
    {
        var recordsAroundPlayer = GetRecordsAroundPlayer(player, records);
        return topRecords.UnionBy(recordsAroundPlayer, r => r.Position);
    }
    
    private IEnumerable<ILocalRecord> GetRecordsAroundPlayer(IPlayer player, IEnumerable<ILocalRecord> records)
    {
        var maxAroundRange = Math.Max(settings.MaxWidgetRows - settings.WidgetShowTop, 0);
        var nRecords = records.Count();
        var playerRecord = records.FirstOrDefault(r => r.Record.Player.Id == player.Id);
        var position = playerRecord?.Position ?? 0;

        if (playerRecord?.Position > 0)
        {
            var lower = Math.Max(position - maxAroundRange / 2, 0);
            var left = Math.Max(maxAroundRange - (nRecords - lower), 0);
            var intersect = Math.Max(settings.WidgetShowTop - lower, 0);
            return records
                .Skip(Math.Min(Math.Max(lower - left, 0), nRecords - 1))
                .Take(maxAroundRange + intersect);
        }

        return Array.Empty<ILocalRecord>();
    }
}
