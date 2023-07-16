using System.Timers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MotdService : IMotdService
{
    public const string ErrorTextMotdNotLoaded = "Motd couldn't be fetched.";
    
    private readonly IManialinkManager _manialink;
    private readonly IHttpService _httpService;
    private readonly IMotdRepository _repository;
    private readonly ILogger<MotdService> _logger;

    private readonly Timer _motdUpdateTimer;
    
    private string _motdUrl;
    private int _timerInterval;

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
        Timer timer = (Timer)sender!;

        timer.Interval = _timerInterval;
        MotdText = GetMotd().Result;
        _logger.LogDebug($"Fetching ");
    }
    
    public void SetInterval(int interval)
    {
        _timerInterval = interval;
        _motdUpdateTimer.Interval = interval;
    }

    public void SetUrl(string url)
    {
        _motdUrl = url;
        MotdText = GetMotd().Result;
        if (!_motdUpdateTimer.Enabled)
            _motdUpdateTimer.Enabled = true; // re-enable the timer when the url updates.
    }

    public async Task ShowAsync(IPlayer player)
    {
        var isCheckboxChecked = (await _repository.GetEntryAsync(player))?.Hidden ?? false;
        await _manialink.SendManialinkAsync(player, "MotdModule.MotdTemplate", new { isChecked = isCheckboxChecked, text = MotdText });
    }
    
    public async Task<string> GetMotd()
    {
        try
        {
            return await _httpService.GetAsync(_motdUrl);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Motd couldn't be fetched from url \"{_motdUrl}\"");
            _logger.LogError(ex.Message);
            _motdUpdateTimer.Enabled = false; // disable the timer if the url is wrong.
        }
        return ErrorTextMotdNotLoaded;
    }

    public async Task<IMotdEntry?> GetEntryAsync(IPlayer player) 
        => await _repository.GetEntryAsync(player);

    public async Task InsertOrUpdateEntryAsync(IPlayer player, bool hidden)
        => await _repository.InsertOrUpdateEntryAsync(player, hidden);
}
