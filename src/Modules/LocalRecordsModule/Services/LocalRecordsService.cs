using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Util;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Config;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Database;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.LocalRecordsModule.Services;

[Service]
public class LocalRecordsService(
    IMapService mapService,
    ILocalRecordRepository localRecordRepository,
    IPlayerManagerService playerManagerService,
    IManialinkManager manialinkManager,
    ILogger<LocalRecordsService> logger,
    ILocalRecordsSettings settings,
    IChatService server,
    IThemeManager themeManager,
    IPlayerRecordsRepository playerRecordsRepository) : ILocalRecordsService
{
    private const string WidgetName = "LocalRecordsModule.LocalRecordsWidget";

    public async Task<ILocalRecord[]> GetLocalsOfCurrentMapAsync()
    {
        var currentMap = await mapService.GetCurrentMapAsync();

        if (currentMap == null)
        {
            throw new InvalidOperationException("Failed to get current map");
        }

        IEnumerable<ILocalRecord> records = await localRecordRepository.GetLocalRecordsOfMapByIdAsync(currentMap.Id);
        return records.ToArray();
    }

    public async Task ShowWidgetAsync(IPlayer player)
    {
        var records = await GetLocalsOfCurrentMapAsync();
        var playerRecords = GetRecordsWithPlayer(player, records);
        await manialinkManager.SendManialinkAsync(player, WidgetName,
            new { currentPlayer = player, records = playerRecords });
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

    public async Task UpdatePbAsync(IPlayerRecord record)
    {
        var oldRecord = await localRecordRepository.GetRecordOfPlayerInMapAsync(record.Player, record.Map);
        var localRecord = await localRecordRepository.AddOrUpdateRecordAsync(record.Map, record);

        if (localRecord == null)
        {
            // player did not get a local record good enough to be registered
            return;
        }

        var localRaceTime = RaceTime.FromMilliseconds(localRecord.Record.Score).ToString();

        if (localRaceTime == null)
        {
            throw new InvalidOperationException($"Failed to convert {localRecord.Record.Score} to race time");
        }

        if (oldRecord == null)
        {
            await server.InfoMessageAsync(new TextFormatter()
                .AddText(record.Player.NickName)
                .AddText(" gained the ")
                .AddText($"{localRecord.Position}.", s => s.WithColor(themeManager.Theme.Info))
                .AddText(" local record ")
                .AddText(localRaceTime, s => s.WithColor(themeManager.Theme.Info))
                .ToString());
            await ShowWidgetToAllAsync();
            return;
        }

        if (record.Score < oldRecord.Record.Score)
        {
            var timeDifference = RaceTime.FromMilliseconds(oldRecord.Record.Score - record.Score);
            var timeDifferenceStr = timeDifference.ToString();

            if (timeDifferenceStr == null)
            {
                throw new InvalidOperationException($"Failed to convert {timeDifference} to race time difference");
            }

            if (localRecord.Position < oldRecord.Position)
            {
                await server.InfoMessageAsync(new TextFormatter()
                    .AddText(record.Player.NickName)
                    .AddText(" claimed ")
                    .AddText($"{localRecord.Position}.", s => s.WithColor(themeManager.Theme.Info))
                    .AddText(" (from ")
                    .AddText($"{oldRecord.Position}.", s => s.WithColor(themeManager.Theme.Info))
                    .AddText(") local record ")
                    .AddText(localRaceTime, s => s.WithColor(themeManager.Theme.Info))
                    .AddText(" (-")
                    .AddText(timeDifferenceStr, s => s.WithColor(themeManager.Theme.Info))
                    .AddText(")")
                    .ToString());
            }
            else
            {
                await server.InfoMessageAsync(new TextFormatter()
                    .AddText(record.Player.NickName)
                    .AddText(" improved their ")
                    .AddText($"{localRecord.Position}.", s => s.WithColor(themeManager.Theme.Info))
                    .AddText(" local record ")
                    .AddText(localRaceTime, s => s.WithColor(themeManager.Theme.Info))
                    .AddText(" (-")
                    .AddText(timeDifferenceStr, s => s.WithColor(themeManager.Theme.Info))
                    .AddText(")")
                    .ToString());
            }

            await ShowWidgetToAllAsync();
        }
        else if (record.Score == localRecord.Record.Score)
        {
            await server.InfoMessageAsync(new TextFormatter()
                .AddText(record.Player.NickName)
                .AddText(" equaled their ")
                .AddText($"{localRecord.Position}.", s => s.WithColor(themeManager.Theme.Info))
                .AddText(" local record ")
                .AddText(localRaceTime, s => s.WithColor(themeManager.Theme.Info))
                .ToString());
        }
    }

    public async Task ResetLocalRecordsAsync()
    {
        var maps = await mapService.GetCurrentMapListAsync();

        foreach (var map in maps)
        {
            var mapPbs = await playerRecordsRepository.GetRecordsOfMapAsync(map.Id);

            await localRecordRepository.DeleteRecordsAsync(map);
            await localRecordRepository.AddRecordsAsync(map, mapPbs);
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
