using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.CurrentMapModule.Interfaces;
using GbxRemoteNet.Events;
using ISO3166;
using Microsoft.Extensions.Logging;


namespace EvoSC.Modules.Official.CurrentMapModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class CurrentMapService : ICurrentMapService
{
    private readonly ILogger<CurrentMapService> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IManialinkManager _manialinkManager;
    private readonly MapRepository _mapRepository;
    private readonly IServerClient _client;

    public CurrentMapService(IManialinkManager manialinkManager,
        ILoggerFactory loggerFactory, ILogger<CurrentMapService> logger,
        MapRepository mapRepository, IServerClient client)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
        _manialinkManager = manialinkManager;
        _mapRepository = mapRepository;
        _client = client;
    }

    private static string GetCountry(string Zone)
    {
        var zones = Zone.Split("|");
        if (zones.Length >= 2 )
        {
            return zones[2];
        }
        return "Other";
    }

    public async Task ShowWidget()
    {
        _logger.LogWarning("***Asd");
        var map = await _client.Remote.GetCurrentMapInfoAsync();
        var dbMap = await _mapRepository.GetMapByUidAsync(map.UId);
        List<Country> countries = Country.List.ToList();
        var country = countries.Find(country => country.Name == GetCountry(dbMap?.Author?.Zone ?? ""));
        
        await _manialinkManager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            new
            {
                map = dbMap,
                country = country?.ThreeLetterCode ?? "WOR"
            });
        _logger.LogDebug("Showing widget");
    }
    
    public async Task ShowWidget(MapGbxEventArgs args)
    {
        var dbMap = await _mapRepository.GetMapByUidAsync(args.Map.Uid);
        List<Country> countries = Country.List.ToList();
        var country = countries.Find(country => country.Name == GetCountry(dbMap?.Author?.Zone ?? ""));
        
        await _manialinkManager.SendPersistentManialinkAsync("CurrentMapModule.CurrentMapWidget",
            new
            {
                map = dbMap,
                country = country?.ThreeLetterCode ?? "WOR"
            });
        _logger.LogDebug("Showing widget");
    }

    public async Task HideWidget()
    {
        await _manialinkManager.HideManialinkAsync("CurrentMapModule.CurrentMapWidget");
        _logger.LogDebug("Hiding widget");
    }
}
