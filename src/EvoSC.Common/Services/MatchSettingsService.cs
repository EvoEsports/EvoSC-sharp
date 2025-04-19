using EvoSC.Common.Events.Arguments.MatchSettings;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Builders;
using GbxRemoteNet.Exceptions;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MatchSettingsService(ILogger<MatchSettingsService> logger, IServerClient server, IMapService mapService, IEventManager events, IMatchSettingsTrackerService matchSettingsTrackerService)
    : IMatchSettingsService
{
    public async Task SetCurrentScriptSettingsAsync(Action<Dictionary<string, object>> settingsAction)
    {
        var settings = await server.Remote.GetModeScriptSettingsAsync();

        if (settings == null)
        {
            throw new InvalidOperationException("Failed to get current ModeScript settings.");
        }
        
        settingsAction(settings);
        await server.Remote.SetModeScriptSettingsAsync(settings);
        await events.RaiseAsync(MatchSettingsEvent.ScriptSettingsChanged, new ScriptSettingsChangedEventArgs
        {
            NewScriptSettings = settings 
        });
    }

    public Task SetCurrentScriptSettingsAsync(IMatchSettings matchSettings) => SetCurrentScriptSettingsAsync(settings =>
    {
        foreach (var (key, value) in matchSettings.ModeScriptSettings)
        {
            if (value.Value != null)
            {
                settings[key] = value.Value;
            }
        }
    });

    public async Task<Dictionary<string, object>?> GetCurrentScriptSettingsAsync() =>
        await server.Remote.GetModeScriptSettingsAsync();

    public Task LoadMatchSettingsAsync(string name) => LoadMatchSettingsAsync(name, true);
    
    public async Task LoadMatchSettingsAsync(string name, bool skipMap)
    {
        try
        {
            var file = Path.GetFileName($"{name}.txt");
            await server.Remote.LoadMatchSettingsAsync($"MatchSettings/{file}");

            try
            {
                var ms = await GetMatchSettingsAsync(name);
                await events.RaiseAsync(MatchSettingsEvent.MatchSettingsLoaded, new MatchSettingsEventArgs { MatchSettings = ms });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get matchsettings of name '{Name}'", name);
            }

            if (skipMap)
            {
                await server.Remote.NextMapAsync();
            }
        }
        catch (XmlRpcFaultException ex)
        {
            logger.LogError(ex, "Failed to load match settings: {Msg}", ex.Fault.FaultString);

            if (ex.Fault.FaultCode == -1000)
            {
                throw new FileNotFoundException(name);
            }

            throw;
        }
    }

    public async Task<IMatchSettings> CreateMatchSettingsAsync(string name, Action<MatchSettingsBuilder> matchSettings)
    {
        var builder = new MatchSettingsBuilder();
        matchSettings(builder);

        var ms = await SaveMatchSettingsAsync(name, builder);
        await events.RaiseAsync(MatchSettingsEvent.MatchSettingsCreated, new MatchSettingsEventArgs { MatchSettings = ms });
        return ms;
    }

    public async Task<IMatchSettings> GetMatchSettingsAsync(string name)
    {
        var filePath = await GetFilePathAsync(name);
        var mapsDir = await server.GetMapsDirectoryAsync();
        
        var contents = await File.ReadAllTextAsync(filePath);
        var matchSettings = await MatchSettingsXmlParser.ParseAsync(name, contents);

        var enrichedMaps = new List<IMap>();
        foreach (var map in matchSettings.Maps)
        {
            var mapFilePath = Path.Combine(mapsDir, map.FilePath);
            var enrichedMap = await mapService.GetMapByUidAsync(map.Uid) ??
                              await mapService.AddLocalMapAsync(mapFilePath);
            
            enrichedMaps.Add(enrichedMap);
        }

        matchSettings.Maps = enrichedMaps;
        return matchSettings;
    }

    public async Task<IEnumerable<IMap>> GetCurrentMapListAsync()
    {
        var serverMapList = await server.Remote.GetMapListAsync(-1, 0);
        var maps = await mapService.GetMapsByUidAsync(serverMapList.Select(m => m.UId));
        
        return maps;
    }

    public async Task EditMatchSettingsAsync(string name, Action<MatchSettingsBuilder> builderAction)
    {
        var currentMatchSettings = await GetMatchSettingsAsync(name);
        var builder = new MatchSettingsBuilder(currentMatchSettings);
        builderAction(builder);
        var newMatchsettings = await SaveMatchSettingsAsync(name, builder);
        
        await events.RaiseAsync(MatchSettingsEvent.MatchSettingsUpdated, new MatchSettingsEventArgs { MatchSettings = newMatchsettings });
    }

    public async Task DeleteMatchSettingsAsync(string name)
    {
        var ms = await GetMatchSettingsAsync(name);
        var filePath = await GetFilePathAsync(name);
        File.Delete(filePath);
        await events.RaiseAsync(MatchSettingsEvent.MatchSettingsDeleted, new MatchSettingsEventArgs { MatchSettings = ms });
    }

    public async Task<string> GetCurrentScriptNameAsync()
    {
        var scriptInfo = await server.Remote.GetModeScriptInfoAsync();
        return scriptInfo.Name;
    }

    public async Task<DefaultModeScriptName> GetCurrentModeAsync()
    {
        var scriptName = await GetCurrentScriptNameAsync();
        return scriptName.ToEnumValue<DefaultModeScriptName>() ?? DefaultModeScriptName.Unknown;
    }

    public IMatchSettings GetCurrentMatchSettings() => matchSettingsTrackerService.CurrentMatchSettings;

    public Task ReloadCurrentMatchSettingsAsync() => LoadMatchSettingsAsync(GetCurrentMatchSettings().Name);

    public Task EditCurrentMatchSettingsAsync(Action<MatchSettingsBuilder> builderAction) =>
        EditMatchSettingsAsync(GetCurrentMatchSettings().Name, builderAction);

    public async Task<IEnumerable<IMatchSettings>> GetAllMatchSettingsAsync()
    {
        var mapsDir = await server.GetMapsDirectoryAsync();
        var matchSettingsDir = Path.Combine(mapsDir, "MatchSettings");

        if (!Directory.Exists(matchSettingsDir))
        {
            throw new DirectoryNotFoundException("The match settings directory does not exist.");
        }

        var matchSettingsFiles = new List<IMatchSettings>();
        foreach (var msFileName in Directory.GetFiles(matchSettingsDir, "*.txt", SearchOption.TopDirectoryOnly))
        {
            try
            {
                var name = Path.GetFileNameWithoutExtension(msFileName);
                var matchSettings = await GetMatchSettingsAsync(name);
                matchSettingsFiles.Add(matchSettings);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to load match settings file: {File}", msFileName);
            }
        }

        return matchSettingsFiles.ToArray();
    }


    private async Task<string> GetFilePathAsync(string name)
    {
        var mapsDir = await server.GetMapsDirectoryAsync();
        var filePath = Path.Combine(mapsDir, "MatchSettings", name + ".txt");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Failed to find the match settings with the provided name.", filePath);
        }

        return filePath;
    }
    
    private async Task<IMatchSettings> SaveMatchSettingsAsync(string name, MatchSettingsBuilder builder)
    {
        var builtMatchSettings = builder.Build();
        var contents = new GbxBase64(builtMatchSettings
            .ToXmlDocument()
            .GetFullXmlString()
        );
        
        var fileName = Path.Combine("MatchSettings", name + ".txt");

        try
        {
            if (!await server.Remote.WriteFileAsync(fileName, contents))
            {
                throw new InvalidOperationException("Failed to create match settings due to an unknown error.");
            }
            
            return builtMatchSettings;
        }
        catch (Exception ex)
        {
            if (ex is XmlRpcFaultException faultEx)
            {
                logger.LogError(faultEx, "Failed to create match settings due to XMLRPC fault");
            }
            else
            {
                logger.LogError(ex, "An error occured while creating match settings");
            }

            throw;
        }
    }
}
