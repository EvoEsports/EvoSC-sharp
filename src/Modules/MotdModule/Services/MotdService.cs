using System.Timers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using Microsoft.Extensions.Logging;
using ILogger = Castle.Core.Logging.ILogger;
using Timer = System.Timers.Timer;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MotdService : IMotdService
{
    private readonly IManialinkManager _manialink;
    private readonly IHttpService _httpService;
    private readonly IMotdRepository _repository;
    private readonly ILogger<MotdService> _logger;

    private readonly Timer _motdUpdateTimer;
    private readonly string _motdUrl;
    private readonly int _timerInterval;

    private string MotdText { get; set; } = "";

    public MotdService(IManialinkManager manialink, IHttpService httpService, 
        IMotdRepository repository, IMotdSettings motdSettings, ILogger<MotdService> logger)
    {
        _manialink = manialink;
        _httpService = httpService;
        _repository = repository;
        _motdUrl = motdSettings.MotdUrl;
        _timerInterval = motdSettings.MotdFetchInterval;
        _logger = logger;
        
        _motdUpdateTimer = new Timer()
        {
            Interval = 1,
            Enabled = true,
            AutoReset = true
        };
        _motdUpdateTimer.Elapsed += MotdUpdateTimerOnElapsed;
    }

    private void MotdUpdateTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        if (sender is not Timer timer)
            return;
        
        timer.Interval = _timerInterval;
        MotdText = GetMotd().Result;
        _logger.LogDebug("Timer fired");
    }

    public async Task ShowAsync(IPlayer player)
    {
        var isChecked = (await _repository.GetEntryAsync(player))?.Hidden ?? false;
        await _manialink.SendManialinkAsync(player, "MotdModule.MotdTemplate", new { isChecked = isChecked, text = MotdText });
    }
    
    public async Task<string> GetMotd()
    {
        return await _httpService.GetAsync(_motdUrl);
    }

    public async Task<IMotdEntry?> GetEntryAsync(IPlayer player) 
        => await _repository.GetEntryAsync(player);

    public async Task InsertOrUpdateEntryAsync(IPlayer player, bool hidden)
        => await _repository.InsertOrUpdateEntryAsync(player, hidden);
}
