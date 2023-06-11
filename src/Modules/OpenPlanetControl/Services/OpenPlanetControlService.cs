using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Models;
using FluentMigrator.Runner;
using GbxRemoteNet;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.OpenPlanetControl.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class OpenPlanetControlService : IOpenPlanetControlService
{
    private readonly ILogger<OpenPlanetControlService> _logger;
    private readonly IManialinkManager _manialinkManager;
    private readonly IServerClient _server;
    private readonly int _kickTimeout = 30;
    private Dictionary<string, OpenPlanetInfo> _players { get; set; }

    public OpenPlanetControlService(ILogger<OpenPlanetControlService> logger, IManialinkManager manialinkManager,
        IServerClient server)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        _players = new Dictionary<string, OpenPlanetInfo>();
        _server = server;
    }

    public async Task OnEnableAsync()
    {
        _logger.LogInformation("OpenPlanetControl enabled");
        await _manialinkManager.SendPersistentManialinkAsync("OpenPlanetControl.DetectOP", new { });
    }

    public async Task OnDisableAsync()
    {
        await _manialinkManager.HideManialinkAsync("OpenPlanetControl.DetectOP");
        _logger.LogInformation("OpenPlanetControl disabled");
    }

    public async Task OnDetectAsync(string login, string data)
    {
        _logger.LogEmphasized("OnDetect");
        var info = new OpenPlanetInfo(data);

        if (!info.isOpenPlanet) return;
        _players.Remove(login);
        _players.Add(login, info);

        if (info.signatureMode != "COMPETITION")
        {
            await _manialinkManager.SendManialinkAsync("OpenPlanetControl.Warning",
                new { Mode = info.signatureMode, AllowedModeText = "COMPETITION", KickTimeout = _kickTimeout});
            return;
        }

        await _manialinkManager.HideManialinkAsync("OpenPlanetControl.Warning");
    }

    public void RemovePlayerByLogin(string login)
    {
        _players.Remove(login);
    }

    public async Task KickAsync(string login)
    {
        await _server.Remote.KickAsync(login, "Incompatible Openplanet signature mode");
    }
}
