using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Config;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Models;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.OpenPlanetControl.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class OpenPlanetControlService : IOpenPlanetControlService
{
    private readonly ILogger<OpenPlanetControlService> _logger;
    private readonly IManialinkManager _manialinkManager;
    private readonly IServerClient _server;
    private readonly IOpenPlanetControlSettings _settings;
    public Dictionary<string, OpenPlanetInfo> players { get; set; }

    public OpenPlanetControlService(ILogger<OpenPlanetControlService> logger,
        IManialinkManager manialinkManager,
        IServerClient server, IOpenPlanetControlSettings settings)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        players = new Dictionary<string, OpenPlanetInfo>();
        _server = server;
        _settings = settings;
    }

    public async Task OnEnableAsync()
    {
        await _manialinkManager.SendPersistentManialinkAsync("OpenPlanetControl.DetectOP", new { });
    }

    public async Task OnDisableAsync()
    {
        await _manialinkManager.HideManialinkAsync("OpenPlanetControl.DetectOP");
        _logger.LogInformation("OpenPlanetControl disabled");
    }

    public async Task OnDetectAsync(string login, string data)
    {
        var info = new OpenPlanetInfo(data);
        players.Remove(login);
        if (!info.isOpenPlanet) return;

        players.Add(login, info);
        var allowedTypes = _settings.AllowedTypes;
        if (!allowedTypes.Contains(info.signatureMode))
        {
            await _manialinkManager.SendManialinkAsync("OpenPlanetControl.Warning",
                new
                {
                    Mode = info.signatureMode,
                    AllowedModeText = string.Join("$fff, $9df", allowedTypes),
                    KickTimeout = _settings.KickTimeout
                });
            return;
        }

        await _manialinkManager.HideManialinkAsync("OpenPlanetControl.Warning");
    }

    public void RemovePlayerByLogin(string login)
    {
        players.Remove(login);
    }

    public async Task KickAsync(string login)
    {
        await _server.Remote.KickAsync(login, "Incompatible Openplanet signature mode");
    }
}
