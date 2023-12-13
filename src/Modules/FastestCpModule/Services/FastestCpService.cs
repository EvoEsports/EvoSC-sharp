using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.FastestCpModule.Interfaces;
using EvoSC.Modules.Official.FastestCpModule.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.FastestCpModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class FastestCpService(IPlayerManagerService playerManagerService, IManialinkManager manialinkManager,
        ILoggerFactory loggerFactory, ILogger<FastestCpService> logger)
    : IFastestCpService
{
    private FastestCpStore _fastestCpStore = new(loggerFactory.CreateLogger<FastestCpStore>());

    public async Task RegisterCpTimeAsync(WayPointEventArgs args)
    {
        var result = _fastestCpStore.RegisterTime(args.AccountId, args.CheckpointInRace, args.RaceTime);
        if (result)
        {
            await ShowWidgetAsync();
        }
    }

    public async Task ResetCpTimesAsync()
    {
        _fastestCpStore = new(loggerFactory.CreateLogger<FastestCpStore>());
        await manialinkManager.HideManialinkAsync("FastestCpModule.FastestCp");
        logger.LogDebug("Hiding fastest cp manialink for all users");
    }

    private async Task ShowWidgetAsync()
    {
        await manialinkManager.SendPersistentManialinkAsync("FastestCpModule.FastestCp",
            new { times = await GetCurrentBestCpTimesAsync() });
        logger.LogDebug("Updating fastest cp manialink for all users");
    }

    private async Task<PlayerCpTime[]> GetCurrentBestCpTimesAsync()
    {
        var fastestTimes = _fastestCpStore.GetFastestTimes();

        var playerNameCache = new Dictionary<string, string>();
        await Task.WhenAll(
            fastestTimes.Where(s => s != null)
                .Select(time => time!.AccountId)
                .Distinct()
                .Select(async id =>
                {
                    var player = await playerManagerService.GetOrCreatePlayerAsync(id);
                    playerNameCache[id] = player.StrippedNickName;
                }));

        return fastestTimes.Where(time => time != null).Select((time, index) =>
                new PlayerCpTime(playerNameCache[time!.AccountId], index, TimeSpan.FromMilliseconds(time.RaceTime)))
            .TakeLast(18).ToArray();
    }
}
