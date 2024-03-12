using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Builders;
using GbxRemoteNet.Exceptions;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Services;

public class MatchSettingsService(ILogger<MatchSettingsService> logger, IServerClient server, IEvoScBaseConfig config, IMapService mapService)
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

    public Task<IMatchSettings> CreateMatchSettingsAsync(string name, Action<MatchSettingsBuilder> matchSettings)
    {
        var builder = new MatchSettingsBuilder();
        matchSettings(builder);

        return SaveMatchSettingsAsync(name, builder);
    }

    public async Task<IMatchSettings> GetMatchSettingsAsync(string name)
    {
        var filePath = await GetFilePathAsync(name);
        
        var contents = await File.ReadAllTextAsync(filePath);
        return await MatchSettingsXmlParser.ParseAsync(contents);
    }

    public async Task<IEnumerable<IMap>> GetCurrentMapListAsync()
    {
        var serverMapList = await server.Remote.GetMapListAsync(-1, 0);

        var maps = new List<IMap>();

        foreach (var serverMap in serverMapList)
        {
            var map = await mapService.GetMapByUidAsync(serverMap.UId);

            if (map == null)
            {
                continue;
            }
            
            maps.Add(map);
        }

        return maps;
    }

    public async Task EditMatchSettingsAsync(string name, Action<MatchSettingsBuilder> builderAction)
    {
        var currentMatchSettings = await GetMatchSettingsAsync(name);
        var builder = new MatchSettingsBuilder(currentMatchSettings);
        builderAction(builder);
        await SaveMatchSettingsAsync(name, builder);
    }

    public async Task DeleteMatchSettingsAsync(string name)
    {
        var filePath = await GetFilePathAsync(name);
        File.Delete(filePath);
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
