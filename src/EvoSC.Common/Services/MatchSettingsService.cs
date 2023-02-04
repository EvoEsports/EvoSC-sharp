﻿using System.Text;
using System.Xml.Linq;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Builders;
using EvoSC.Common.Util.MatchSettings.Models;
using GbxRemoteNet.Exceptions;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1;

namespace EvoSC.Common.Services;

public class MatchSettingsService : IMatchSettingsService
{
    private readonly ILogger<MatchSettingsService> _logger;
    private readonly IServerClient _server;
    private readonly IEvoScBaseConfig _config;
    
    public MatchSettingsService(ILogger<MatchSettingsService> logger, IServerClient server, IEvoScBaseConfig config)
    {
        _logger = logger;
        _server = server;
        _config = config;
    }
    
    public async Task SetScriptSettingsAsync(Action<Dictionary<string, object>> settingsAction)
    {
        var settings = await _server.Remote.GetModeScriptSettingsAsync();

        if (settings == null)
        {
            throw new InvalidOperationException("Failed to get current ModeScript settings.");
        }
        
        settingsAction(settings);
        await _server.Remote.SetModeScriptSettingsAsync(settings);
    }

    public Task SetScriptSettingsAsync(IMatchSettings matchSettings) => SetScriptSettingsAsync(settings =>
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
        await _server.Remote.GetModeScriptSettingsAsync();

    public async Task LoadMatchSettingsAsync(string name)
    {
        try
        {
            var file = Path.GetFileName($"{name}.txt");
            await _server.Remote.LoadMatchSettingsAsync($"MatchSettings/{file}");
            await _server.Remote.RestartMapAsync();
        }
        catch (XmlRpcFaultException ex)
        {
            _logger.LogError(ex, "Failed to load match settings: {Msg}", ex.Fault.FaultString);

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

        var builtMatchSettings = builder.Build();
        var contents = new Base64(builtMatchSettings
            .ToXmlDocument()
            .GetFullXmlString()
        );
        
        var fileName = Path.Combine("MatchSettings", name + ".txt");

        try
        {
            if (!await _server.Remote.WriteFileAsync(fileName, contents))
            {
                throw new InvalidOperationException("Failed to create match settings due to an unknown error.");
            }
            
            return builtMatchSettings;
        }
        catch (Exception ex)
        {
            if (ex is XmlRpcFaultException faultEx)
            {
                _logger.LogError(faultEx, "Failed to create match settings due to XMLRPC fault");
            }
            else
            {
                _logger.LogError(ex, "An error occured while creating match settings");
            }

            throw;
        }
    }

    public async Task<IMatchSettings> GetMatchSettingsAsync(string name)
    {
        var mapsDir = _config.Path.Maps;

        if (mapsDir == string.Empty)
        {
            mapsDir = await _server.Remote.GetMapsDirectoryAsync();
        }

        // if it's still empty and doesn't exist, we should throw an error
        if (mapsDir == string.Empty && !Directory.Exists(mapsDir))
        {
            // we do this check to increase error tracking, even though file
            // existence is checked later anyways
            throw new DirectoryNotFoundException("Failed to find an existing maps directory.");
        }

        var filePath = Path.Combine(mapsDir, "MatchSettings", name + ".txt");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Failed to find the match settings with the provided name.", filePath);
        }

        var contents = await File.ReadAllTextAsync(filePath);
        return await MatchSettingsXmlParser.ParseAsync(contents);
    }
}
