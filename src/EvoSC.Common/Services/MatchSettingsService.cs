﻿using System.Text;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;
using EvoSC.Common.Util.MatchSettings.Builders;
using GbxRemoteNet.Exceptions;
using GbxRemoteNet.XmlRpc.ExtraTypes;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1;

namespace EvoSC.Common.Services;

public class MatchSettingsService : IMatchSettingsService
{
    private readonly ILogger<MatchSettingsService> _logger;
    private readonly IServerClient _server;
    
    public MatchSettingsService(ILogger<MatchSettingsService> logger, IServerClient server)
    {
        _logger = logger;
        _server = server;
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
            _logger.LogError(ex, "Failed to load match settings");

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
        var xmlDocument = builtMatchSettings.ToXmlDocument();
        var contents = new Base64(xmlDocument.GetFullXmlString());
        var fileName = Path.Combine("MatchSettings", name + ".txt");

        if (!await _server.Remote.WriteFileAsync(fileName, contents))
        {
            throw new InvalidOperationException($"Failed to write match settings to file {fileName}.");
        }

        return builtMatchSettings;
    }
}
