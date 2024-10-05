using System.Text.Json;
using System.Text.Json.Serialization;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Modules.EvoEsports.ToornamentModule.Interfaces;
using EvoSC.Modules.EvoEsports.ToornamentModule.Models;
using EvoSC.Modules.EvoEsports.ToornamentModule.Settings;
using Microsoft.Extensions.Logging;
using ToornamentApi.Models.Api.TournamentApi;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchSettingsCreatorService(
    ILogger<MatchSettingsCreatorService> logger,
    IToornamentSettings toornamentSettings,
    IToornamentService toornamentService,
    IMatchSettingsService matchSettingsService,
    IServerClient serverClient,
    IDiscordNotifyService notifyService) : IMatchSettingsCreatorService
{
    private readonly Random _rng = new();

    public async Task<string> CreateMatchSettingsAsync(TournamentBasicData tournament, MatchInfo matchInfo,
        StageInfo stageInfo, GroupInfo groupInfo, RoundInfo roundInfo, IEnumerable<IMap> maps)
    {
        logger.LogDebug("Creating matchsettings");
        var settingsData = new TrackmaniaIntegrationSettingsData();
        if (toornamentSettings.UseToornamentDiscipline)
        {
            //DisciplineInfo contains the trackmania matchsettings (nrOfRounds, point distribution etc)
            if (tournament != null)
            {
                logger.LogDebug("Fetching matchsettings defined in Toornament Discipline {Discipline}.",
                    tournament.Discipline);
                DisciplineInfo? discipline;
                discipline = await toornamentService.GetDisciplineAsync(tournament.Discipline);
                var tmIntegration = discipline?.Features.FirstOrDefault(f => f.Name == "trackmania_integration");

                if (tmIntegration == null)
                {
                    throw new InvalidOperationException("No match discipline configuration found");
                }

                var settingsDefault = tmIntegration.Options.FirstOrDefault(o => o.Key == "settings_default").Value;
                settingsData = TrackmaniaIntegrationSettingsData.CreateFromObject(settingsDefault);
            }
        }
        else
        {
            logger.LogDebug("Using matchsettings defined in environment variable");
            if (string.IsNullOrEmpty(toornamentSettings.Disciplines))
            {
                throw new ArgumentNullException(nameof(toornamentSettings.Disciplines),
                    "No disciplines defined in Environment settings");
            }

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };
            var allDisciplines =
                JsonSerializer.Deserialize<List<TrackmaniaIntegrationSettingsData>>(toornamentSettings.Disciplines,
                    jsonSerializerOptions);
            settingsData = allDisciplines.FirstOrDefault(x =>
                x.StageNumber == stageInfo.Number && x.GroupNumber == groupInfo.Number &&
                x.RoundNumber == roundInfo.Number);
        }

        if (settingsData == null)
        {
            throw new ArgumentNullException("Settings are missing");
        }

        if (settingsData.Scripts.S_PointsRepartition.Equals("n-1"))
        {
            logger.LogDebug(
                "Generating Points Repartition based on the number of Opponents in this match, which is {0} opponents",
                matchInfo.Opponents.Count());
            settingsData.Scripts.S_PointsRepartition = GeneratePointRepartition(matchInfo.Opponents.Count());
        }

        var name = $"tournament_{settingsData.StageNumber}_{settingsData.GroupNumber}_{settingsData.RoundNumber}";

        var mapsToAdd = new List<IMap>();

        foreach (var map in maps)
        {
            try
            {
                logger.LogDebug("Checking if all the maps are available on the server");
                var serverMap = await serverClient.Remote.GetMapInfoAsync(map.FilePath);

                if (serverMap != null)
                {
                    mapsToAdd.Add(map);
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        if (settingsData.TracksShuffle)
        {
            logger.LogDebug("Shuffling the map order");
            mapsToAdd = Shuffle(mapsToAdd);
        }

        await CreateMatchSettings(name, settingsData, mapsToAdd);

        var warmupSettingsData = settingsData;
        warmupSettingsData.Scripts.S_WarmUpDuration = 3600;
        warmupSettingsData.Scripts.S_WarmUpNb = 10;
        await CreateMatchSettings(name + "_warmup", settingsData, mapsToAdd);

        var matchName = "Match#" + stageInfo.Number + "." + groupInfo.Number + "." + roundInfo.Number + "." + matchInfo.Number;

        await notifyService.NotifyMatchInfoAsync(matchName, mapsToAdd);

        logger.LogDebug("End of CreateMatchSettingsAsync()");
        return name;
    }

    public async Task CreateMatchSettings(string name, TrackmaniaIntegrationSettingsData settingsData,
        List<IMap> mapsToAdd)
    {
        try
        {
            await matchSettingsService.CreateMatchSettingsAsync(name, builder =>
            {
                if (toornamentSettings.UseDefaultGameMode)
                {
                    var mode = settingsData.GameMode.ToLowerInvariant() switch
                    {
                        "rounds" => DefaultModeScriptName.Rounds,
                        "cup" => DefaultModeScriptName.Cup,
                        "time_attack" => DefaultModeScriptName.TimeAttack,
                        "knockout" => DefaultModeScriptName.Knockout,
                        "laps" => DefaultModeScriptName.Laps,
                        "team" => DefaultModeScriptName.Teams,
                        _ => DefaultModeScriptName.TimeAttack
                    };
                    logger.LogDebug("Creating match settings with gamemode {0}", mode);

                    builder.WithMode(mode);
                }
                else
                {
                    if (string.IsNullOrEmpty(toornamentSettings.GameModes))
                    {
                        logger.LogWarning("Custom gamemode not defined in environment!!");
                        throw new ArgumentNullException(nameof(toornamentSettings.GameModes));
                    }

                    var availableCustomModes = toornamentSettings.GameModes.Trim().Split(',');

                    var selectedGameMode = availableCustomModes.FirstOrDefault(gm => gm.ToLowerInvariant().Contains(settingsData.GameMode));

                    if (string.IsNullOrEmpty(selectedGameMode))
                    {
                        logger.LogWarning("GameMode could not be determined from custom gamemodes. Falling back to Default scriptmodes");
                        var mode = settingsData.GameMode.ToLowerInvariant() switch
                        {
                            "rounds" => DefaultModeScriptName.Rounds,
                            "cup" => DefaultModeScriptName.Cup,
                            "time_attack" => DefaultModeScriptName.TimeAttack,
                            "knockout" => DefaultModeScriptName.Knockout,
                            "laps" => DefaultModeScriptName.Laps,
                            "team" => DefaultModeScriptName.Teams,
                            _ => DefaultModeScriptName.TimeAttack
                        };
                        logger.LogDebug("Creating match settings with gamemode {0}", mode);

                        builder.WithMode(mode);
                    }
                    else
                    {
                        var prefixedGameMode = "Trackmania/" + selectedGameMode;
                        logger.LogDebug("Creating match settings with custom gamemode {0}", prefixedGameMode);

                        builder.WithMode(prefixedGameMode);
                    }
                }

                builder.WithModeSettings(s =>
                {
                    var type = settingsData.Scripts.GetType();
                    var properties = settingsData.Scripts.GetType().GetProperties();

                    logger.LogDebug("Creating match settings with scriptsettings: ");
                    foreach (var item in properties)
                    {
                        var value = item.GetValue(settingsData.Scripts);
                        logger.LogDebug("[{0}]: {1}", item.Name, value);
                        s[item.Name] = value;
                    }
                });

                builder.WithMaps(mapsToAdd);
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    private string GeneratePointRepartition(int number)
    {
        if (number < 1)
        {
            return string.Empty; // Return an empty string if the number is less than 1
        }

        var countdownArray = new string[number];
        for (int i = 0; i < number; i++)
        {
            countdownArray[i] = (number - i).ToString();
        }

        return string.Join(",", countdownArray);
    }

    private List<IMap> Shuffle(List<IMap> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            IMap value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
}
