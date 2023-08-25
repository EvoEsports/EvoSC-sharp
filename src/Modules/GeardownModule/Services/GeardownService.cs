using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Official.Maps.Interfaces;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GeardownService : IGeardownService
{
    private readonly IMapService _maps;
    private readonly IMatchSettingsService _matchSettings;
    private readonly IGeardownApiService _geardownApi;
    private readonly IMxMapService _mxMapService;

    public GeardownService(IGeardownApiService geardownApi, IMapService maps, IMatchSettingsService matchSettings, IMxMapService mxMapService)
    {
        _geardownApi = geardownApi;
        _maps = maps;
        _matchSettings = matchSettings;
        _mxMapService = mxMapService;
    }

    private async Task<IEnumerable<IMap>> GetMapsAsync(GdMatch match)
    {
        var maps = new List<IMap>();
        
        foreach (var map in match.map_pool_orders)
        {
            if (map.mx_map_id == null)
            {
                continue;
            }
            
            var downloadedMap = await _mxMapService.FindAndDownloadMapAsync(map.mx_map_id ?? 0, "", null);
            maps.Add(downloadedMap);
        }

        return maps;
    }
    
    public async Task SetupServerAsync(string matchToken)
    {
        var match = await _geardownApi.Matches.GetMatchDataByTokenAsync(matchToken);

        // 1. get the maps
        // 2. create the match settings
        // 3. whitelist match participant
        // 4. whitelist admins and spectators
        // 5. Load new match settings

        var maps = await GetMapsAsync(match);

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
                foreach (var setting in format.match_settings)
                {
                    s[setting.key] = setting.value;
                }
            });

            builder.WithMaps(maps);
            // builder.AddMap("Campaigns/CurrentQuarterly/Spring 2023 - 01.Map.Gbx");
        });

        await _matchSettings.LoadMatchSettingsAsync(name);
    }
}
