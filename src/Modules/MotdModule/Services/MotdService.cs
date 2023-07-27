using System.Timers;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Events;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace EvoSC.Modules.Official.MotdModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class MotdService : IMotdService, IDisposable
{
    private readonly IManialinkManager _manialink;
    private readonly IHttpService _httpService;
    private readonly IMotdRepository _repository;
    private readonly IMotdSettings _settings;
    private readonly ILogger<MotdService> _logger;
    private readonly IContextService _context;
    private readonly IPlayerManagerService _playerManager;

    private readonly Timer _motdUpdateTimer;
    
    private string _motdUrl;
    private int _timerInterval;
    private bool _isMotdLocal;

    private string MotdText { get; set; } = "";
    
    public bool IsDisposed { get; private set; }

    public MotdService(IManialinkManager manialink, IHttpService httpService, 
        IMotdRepository repository, IMotdSettings motdSettings, ILogger<MotdService> logger,
        IContextService context, IPlayerManagerService playerManager)
    {
        _manialink = manialink;
        _httpService = httpService;
        _repository = repository;
        _settings = motdSettings;
        _playerManager = playerManager;
        _motdUrl = motdSettings.MotdUrl;
        _timerInterval = motdSettings.MotdFetchInterval;
        _isMotdLocal = motdSettings.UseLocalMotd;
        _logger = logger;
        _context = context;
        
        _motdUpdateTimer = new Timer
        {
            Interval = 1,
            Enabled = !_isMotdLocal,
            AutoReset = true
        };
        if (_isMotdLocal)
        {
            MotdText = _settings.MotdLocalText;
        }

        _motdUpdateTimer.Elapsed += MotdUpdateTimerOnElapsed;
    }

    private void MotdUpdateTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Timer timer = (Timer)sender!;
        if (_isMotdLocal)
        {
            timer.Enabled = false;
            return;
        }

        timer.Interval = _timerInterval;
        MotdText = GetMotdAsync().Result;
        _logger.LogDebug($"Fetching Motd");
    }
    
    public void SetInterval(int interval, IPlayer player)
    {
        _timerInterval = interval;
        _motdUpdateTimer.Interval = interval;
        
        _context.Audit().Success()
            .WithEventName(AuditEvents.IntervalSet)
            .HavingProperties(new {Player = player})
            .Comment("Motd Timer Interval changed.");
    }

    public void SetLocalMotd(string text, IPlayer player)
    {
        var old = _settings.MotdLocalText;
        _settings.MotdLocalText = text;
        MotdText = text;
        
        _context.Audit().Success()
            .WithEventName(AuditEvents.LocalTextSet)
            .HavingProperties(new {Player = player, oldText = old, newText = text})
            .Comment("Local MotdText changed.");
    }

    public void SetMotdSource(bool local, IPlayer player)
    {
        var old = _settings.UseLocalMotd;
        _settings.UseLocalMotd = local;
        _isMotdLocal = local;
        if (local)
        {
            MotdText = _settings.MotdLocalText;
        }
        else
        {
            _motdUpdateTimer.Interval = 200;
            _motdUpdateTimer.Enabled = true;
        }
        
        _context.Audit().Success()
            .WithEventName(AuditEvents.IsLocalSet)
            .HavingProperties(new {Player = player, oldValue = old, newValue = local })
            .Comment("MotdSource changed.");
    }

    public void SetUrl(string url, IPlayer player)
    {
        var oldUri = _motdUrl;
        _motdUrl = url;
        MotdText = GetMotdAsync().Result;
        if (!_motdUpdateTimer.Enabled)
        {
            _motdUpdateTimer.Enabled = true; // re-enable the timer when the url updates.
        }
        
        _context.Audit().Success()
            .WithEventName(AuditEvents.UrlSet)
            .HavingProperties(new {Player = player, oldUri, newUri = url})
            .Comment("Local MotdText changed changed.");
    }

    public async Task ShowEditAsync(IPlayer player)
    {
        _context.Audit().Success()
            .WithEventName(AuditEvents.LocalTextEditOpened)
            .HavingProperties(new {Player = player})
            .Comment("Local Motd editor shown.");
        
        await _manialink.SendManialinkAsync(player, "MotdModule.MotdEdit", new { text = _settings.MotdLocalText });
    }

    public async Task ShowAsync(string login, bool explicitly)
        => await ShowAsync(await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(login)), explicitly);

    public async Task ShowAsync(IPlayer? player, bool explicitly)
    {
        bool? hidden = null;
        if (player is null)
        {
            return;
        }
        if (!explicitly)
        {
            var playerEntry = await _repository.GetEntryAsync(player);
            if (playerEntry is not null)
            {
                if (playerEntry.Hidden)
                {
                    return;
                }
                hidden = playerEntry.Hidden;
            }
        }
        var isCheckboxChecked = hidden ?? false;
        await _manialink.SendManialinkAsync(player, "MotdModule.MotdTemplate", new { isChecked = isCheckboxChecked, text = MotdText });
    }
    
    public async Task<string> GetMotdAsync()
    {
        try
        {
            return await _httpService.GetAsync(_motdUrl);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Motd couldn't be fetched from url {MotdUrl}. Falling back to local motd", _motdUrl);
            _motdUpdateTimer.Enabled = false; // disable the timer if the url is wrong.
            return _settings.MotdLocalText;
        }
    }

    public async Task<IMotdEntry?> GetEntryAsync(IPlayer player) 
        => await _repository.GetEntryAsync(player);

    public async Task InsertOrUpdateEntryAsync(IPlayer player, bool hidden)
        => await _repository.InsertOrUpdateEntryAsync(player, hidden);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        _motdUpdateTimer.Dispose();
        IsDisposed = true;
    }
}
