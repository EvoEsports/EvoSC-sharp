using System.Globalization;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Util;
using EvoSC.Modules.Official.Maps.Interfaces;
using GbxRemoteNet;
using ManiaExchange.ApiClient;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GeardownService : IGeardownService
{
    private readonly IMapService _maps;
    private readonly IMatchSettingsService _matchSettings;
    private readonly IGeardownApiService _geardownApi;
    private readonly IMxMapService _mxMapService;
    private readonly IServerClient _server;

    public GeardownService(IGeardownApiService geardownApi, IMapService maps, IMatchSettingsService matchSettings,
        IMxMapService mxMapService, IServerClient server)
    {
        _geardownApi = geardownApi;
        _maps = maps;
        _matchSettings = matchSettings;
        _mxMapService = mxMapService;
        _server = server;
    }

    private async Task<IMap> DownloadAndAddMap(GdMapPoolOrder mapPoolMap)
    {
        var tmxApi = new MxTmApi("EvoSC# Geardown Integration");
        var tmxId = (long?)mapPoolMap.mx_map_id ?? 0;

        var mapInfo = await tmxApi.GetMapInfoAsync(tmxId);

        if (mapInfo == null)
        {
            throw new InvalidOperationException(
                $"Failed to get map info for the map with pool ID {mapPoolMap.map_pool_id} and TMX ID {tmxId}.");
        }
        
        var mapFile = await tmxApi.DownloadMapAsync(tmxId, "");

        if (mapFile == null)
        {
            throw new InvalidOperationException(
                $"Failed to download the map with pool ID {mapPoolMap.map_pool_id} and TMX ID {tmxId}.");
        }

        var mapMetaInfo = new MapMetadata
        {
            MapUid = mapInfo.TrackUID,
            MapName = mapInfo.GbxMapName,
            AuthorId = mapInfo.AuthorLogin,
            AuthorName = mapInfo.Username,
            ExternalId = mapInfo.TrackID.ToString(),
            ExternalVersion = Convert.ToDateTime(mapInfo.UpdatedAt, NumberFormatInfo.InvariantInfo).ToUniversalTime(),
            ExternalMapProvider = MapProviders.ManiaExchange
        };

        var mapStream = new MapStream(mapMetaInfo, mapFile);
        return await _maps.AddMapAsync(mapStream);
    }
    
    private async Task<IEnumerable<IMap>> GetMapsAsync(GdMatch match)
    {
        if (match.map_pool_orders == null || match.map_pool_orders.Count == 0)
        {
            throw new InvalidOperationException("No maps found for this match. Did you add them on geardown?");
        }
        
        var maps = new List<IMap>();
        
        foreach (var mapPoolMap in match.map_pool_orders)
        {
            if (mapPoolMap.mx_map_id == null)
            {
                throw new InvalidOperationException($"The map with pool ID {mapPoolMap.map_pool_id} does not have a TMX id.");
            }

            var map = await _maps.GetMapByExternalIdAsync(mapPoolMap.mx_map_id.ToString() ?? "") ??
                      await DownloadAndAddMap(mapPoolMap);

            maps.Add(map);
        }

        return maps;
    }

    private async Task<string> CreateMatchSettingsAsync(GdMatch match, IEnumerable<IMap> maps)
    {
        if (match.formats == null || match.formats.Count == 0)
        {
            throw new InvalidOperationException("No formats has been assigned to this match, did you create one at geardown?");
        }
        
        var format = match.formats.First();
        var name = $"geardown_{format.id}";
        await _matchSettings.CreateMatchSettingsAsync(name, builder =>
        {
            var mode = format.type_id switch
            {
                FormatType.Rounds => DefaultModeScriptName.Rounds,
                FormatType.Cup => DefaultModeScriptName.Cup,
                FormatType.TimeAttack => DefaultModeScriptName.TimeAttack,
                FormatType.Knockout => DefaultModeScriptName.Knockout,
                FormatType.Laps => DefaultModeScriptName.Laps,
                FormatType.Team => DefaultModeScriptName.Teams,
                _ => DefaultModeScriptName.TimeAttack
            };

            builder.WithMode(mode);
            builder.WithModeSettings(s =>
            {
                if (format.match_settings == null)
                {
                    return;
                }
                
                foreach (var setting in format.match_settings)
                {
                    s[setting.key] = MatchSettingsTypeUtils.ConvertToCorrectType(setting.key, setting.value);
                }
            });

            builder.WithMaps(maps);
        });

        return name;
    }

    private async Task WhitelistPlayers(GdMatch match)
    {
        foreach (var participant in match.participants)
        {
            // todo
        }
    }

    private async Task WhitelistSpectators()
    {
        
    }
    
    public async Task SetupServerAsync(string matchToken)
    {
        var match = await _geardownApi.Matches.GetMatchDataByTokenAsync(matchToken);

        if (match == null)
        {
            throw new InvalidOperationException("Failed to fetch match from geardown API.");
        }
        
        // 1. get the maps
        // 2. create the match settings
        // 3. whitelist match participant
        // 4. whitelist admins and spectators
        // 5. Load new match settings

        var maps = (await GetMapsAsync(match)).ToArray();

        if (!maps.Any())
        {
            throw new InvalidOperationException("Did not find any maps for this match.");
        }
        
        var matchSettingsName = await CreateMatchSettingsAsync(match, maps);

        // todo: uncomment this
        /* await _server.Remote.SetMaxPlayersAsync(0);
        await WhitelistPlayers(match);
        await WhitelistSpectators(); */

        await _matchSettings.LoadMatchSettingsAsync(matchSettingsName);
    }
}
