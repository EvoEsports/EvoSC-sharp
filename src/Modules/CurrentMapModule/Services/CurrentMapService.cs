using System.Diagnostics.CodeAnalysis;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using Microsoft.Extensions.Logging;
using GbxRemoteNet.Events;
using ISO3166;

namespace EvoSC.Modules.Official.CurrentMapModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class CurrentMapService : ICurrentMapService
{
    private readonly ILogger<CurrentMapService> _logger;
    private readonly IManialinkManager _manialinkManager;
    private readonly IMapRepository _mapRepository;
    private readonly IServerClient _client;

    public CurrentMapService(IManialinkManager manialinkManager,
        ILogger<CurrentMapService> logger,
        IMapRepository mapRepository, IServerClient client)
    {
        _logger = logger;
        _manialinkManager = manialinkManager;
        _mapRepository = mapRepository;
        _client = client;
    }

    [ExcludeFromCodeCoverage(Justification = "GBXRemoteClient cannot be mocked.")]
    public async Task ShowWidgetAsync()
    {
        var map = await _client.Remote.GetCurrentMapInfoAsync();
        await ShowManialinkAsync(map.UId);
    }

    public async Task ShowWidgetAsync(MapGbxEventArgs args)
    {
        await ShowManialinkAsync(args.Map.Uid);
    }

    public async Task HideWidgetAsync()
    {
        await _manialinkManager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget");
        _logger.LogDebug("Hiding current map widget");
    }

    private static string GetCountry(string Zone)
    {
        var zones = Zone.Split("|");
        if (zones.Length > 2)
        {
            return zones[2];
        }

        return "Other";
    }

    private async Task ShowManialinkAsync(string mapUId)
    {
        var dbMap = await _mapRepository.GetMapByUidAsync(mapUId);
        List<Country> countries = Country.List.ToList();
        var country = countries.Find(country => country.Name == GetCountry(dbMap?.Author?.Zone ?? ""));

        await _manialinkManager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            new { map = dbMap, country = country?.ThreeLetterCode ?? "WOR" });
        _logger.LogDebug("Showing current map widget");
    }
}
