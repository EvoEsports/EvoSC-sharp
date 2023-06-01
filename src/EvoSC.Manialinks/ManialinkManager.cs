using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using ManiaTemplates;
using ManiaTemplates.Lib;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks;

public class ManialinkManager : IManialinkManager
{
    private readonly ILogger<ManialinkManager> _logger;
    private readonly IServerClient _server;

    private readonly ManiaTemplateEngine _engine = new();
    private readonly Dictionary<string, IManialinkTemplateInfo> _templates = new();
    private readonly Dictionary<string, IManiaScriptInfo> _scripts = new();
    private readonly ConcurrentDictionary<string, string> _persistentManialinks = new();

    private static IEnumerable<Assembly> s_defaultAssemblies = new[]
    {
        typeof(IOnlinePlayer).Assembly, typeof(ManialinkManager).Assembly
    };

    public ManialinkManager(ILogger<ManialinkManager> logger, IServerClient server, IEventManager events)
    {
        _logger = logger;
        _server = server;
        
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerConnect)
            .WithInstance(this)
            .WithInstanceClass<ManialinkManager>()
            .WithHandlerMethod<PlayerConnectGbxEventArgs>(HandlePlayerConnectAsync)
            .AsAsync()
        );
    }

    /// <summary>
    /// Used to send persistent manialinks to newly connected players.
    /// </summary>
    private async Task HandlePlayerConnectAsync(object sender, PlayerConnectGbxEventArgs e)
    {
        try
        {
            foreach (var (_, output) in _persistentManialinks)
            {
                await _server.Remote.SendDisplayManialinkPageToLoginAsync(e.Login, output, 0, false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send persistent manialink login '{Login}'. Did they leave already?",
                e.Login);
        }
    }

    public async Task AddDefaultTemplatesAsync()
    {
        var namespaceParts = "EvoSC.Manialinks".Split(".");
        var assembly = this.GetType().Assembly;
        
        foreach (var resourceName in assembly.GetManifestResourceNames())
        {
            var nameComponents = resourceName.Split('.');

            if (nameComponents.Length <= 1)
            {
                continue;
            }
                
            var extension = nameComponents[^1];
            var templateType = extension.ToEnumValue<ManialinkTemplateType>();

            if (templateType == null)
            {
                continue;
            }

            var resourceStream = assembly.GetManifestResourceStream(resourceName);

            if (resourceStream == null)
            {
                continue;
            }
                
            using var streamReader = new StreamReader(resourceStream);
            var contents = await streamReader.ReadToEndAsync();
            var templateName = GetManialinkTemplateName(namespaceParts, nameComponents);

            switch (templateType)
            {
                // intentionally wont use async versions as we are not preprocessing these templates yet
                case ManialinkTemplateType.Script:
                    // ReSharper disable once MethodHasAsyncOverload
                    AddManiaScript(new ManiaScriptInfo
                    {
                        Name = templateName,
                        Content = contents
                    });
                    break;
                case ManialinkTemplateType.Template:
                    // ReSharper disable once MethodHasAsyncOverload
                    AddTemplate(new ManialinkTemplateInfo
                    {
                        Assemblies = Array.Empty<Assembly>(),
                        Name = templateName,
                        Content = contents
                    });
                    break;
                default:
                    continue;
            }
        }
    }

    private static string GetManialinkTemplateName(string[] namespaceParts, string[] nameComponents)
    {
        var index = 0;
        while (index < namespaceParts.Length &&
               nameComponents[index].Equals(namespaceParts[index], StringComparison.Ordinal))
        {
            index++;
        }

        if (nameComponents[index].Equals("Templates", StringComparison.Ordinal))
        {
            index++;
        }

        var templateName = $"EvoSC.{string.Join(".", nameComponents[index..^1])}";
        return templateName;
    }
    
    private MultiCall CreateMultiCall(IEnumerable<IPlayer> players, string manialinkOutput)
    {
        var multiCall = new MultiCall();

        foreach (var player in players)
        {
            multiCall.Add("SendDisplayManialinkPageToLogin", player.GetLogin(), manialinkOutput, 0, false);
        }

        return multiCall;
    }

    public void AddTemplate(IManialinkTemplateInfo template)
    {
        if (_templates.ContainsKey(template.Name))
        {
            throw new InvalidOperationException($"Template '{template.Name}' already exists.");
        }
        
        _engine.AddTemplateFromString(template.Name, template.Content);
        _templates[template.Name] = template;
    }

    public Task AddAndPreProcessTemplateAsync(IManialinkTemplateInfo template)
    {
        AddTemplate(template);
        return _engine.PreProcessAsync(template.Name, template.Assemblies);
    }

    public void AddManiaScript(IManiaScriptInfo maniaScript)
    {
        if (_scripts.ContainsKey(maniaScript.Name))
        {
            throw new InvalidOperationException($"ManiaScript '{maniaScript.Name}' already exists."); 
        }
        
        _engine.AddManiaScriptFromString(maniaScript.Name, maniaScript.Content);
        _scripts[maniaScript.Name] = maniaScript;
    }

    public void RemoveTemplate(string name)
    {
        _engine.RemoveTemplate(name);
        _templates.Remove(name);
    }

    public void RemoveManiaScript(string name)
    {
        _engine.RemoveManiaScript(name);
        _scripts.Remove(name);
    }

    private IEnumerable<Assembly> PrepareRender(string name)
    {
        if (!_templates.ContainsKey(name))
        {
            throw new InvalidOperationException($"Template '{name}' not found.");
        }

        var assemblies = new List<Assembly>();
        assemblies.AddRange(s_defaultAssemblies);
        assemblies.AddRange(_templates[name].Assemblies);

        return assemblies;
    }

    private async Task<string> PrepareAndRenderAsync(string name, IDictionary<string, object?> data)
    {
        var assemblies = PrepareRender(name);
        return await _engine.RenderAsync(name, data, assemblies);
    }
    
    private async Task<string> PrepareAndRenderAsync(string name, dynamic data)
    {
        var assemblies = PrepareRender(name);
        return await _engine.RenderAsync(name, data, assemblies);
    }
    
    private string CreateHideManialink(string name)
    {
        var sb = new StringBuilder()
            .Append("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\" ?>\n")
            .Append("<manialink version=\"3\" id=\"")
            .Append(ManialinkNameUtils.KeyToId(name))
            .Append("\">\n")
            .Append("</manialink>\n");

        return sb.ToString();
    }

    public async Task SendManialinkAsync(string name, IDictionary<string, object?> data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(string name, dynamic data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }

    public Task SendManialinkAsync(string name) => SendManialinkAsync(name, new { });

    public async Task SendPersistentManialinkAsync(string name, IDictionary<string, object?> data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
        _persistentManialinks[name] = manialinkOutput;
    }

    public async Task SendPersistentManialinkAsync(string name, dynamic data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
        _persistentManialinks[name] = manialinkOutput;
    }

    public Task SendPersistentManialinkAsync(string name) => SendPersistentManialinkAsync(name, new { });

    public async Task SendManialinkAsync(IPlayer player, string name, IDictionary<string, object?> data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageToLoginAsync(player.GetLogin(), manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(IPlayer player, string name, dynamic data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageToLoginAsync(player.GetLogin(), manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, IDictionary<string, object?> data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        var multiCall = CreateMultiCall(players, manialinkOutput);
        await _server.Remote.MultiCallAsync(multiCall);
    }

    public async Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, dynamic data)
    {
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        var multiCall = CreateMultiCall(players, manialinkOutput);
        await _server.Remote.MultiCallAsync(multiCall);
    }

    public Task HideManialinkAsync(string name)
    {
        _persistentManialinks.TryRemove(name, out _);
        var manialinkOutput = CreateHideManialink(name);
        return _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 3, true);
    }

    public Task HideManialinkAsync(IPlayer player, string name)
    {
        _persistentManialinks.TryRemove(name, out _);
        var manialinkOutput = CreateHideManialink(name);
        return _server.Remote.SendDisplayManialinkPageToLoginAsync(player.GetLogin(), manialinkOutput, 3, true);
    }

    public Task HideManialinkAsync(IEnumerable<IPlayer> players, string name)
    {
        var manialinkOutput = CreateHideManialink(name);
        var multiCall = new MultiCall();

        foreach (var player in players)
        {
            multiCall.Add("SendDisplayManialinkPageToLogin", player.GetLogin(), manialinkOutput, 3, true);
        }

        return _server.Remote.MultiCallAsync(multiCall);
    }

    public async Task PreprocessAllAsync()
    {
        foreach (var template in _templates.Values)
        {
            _logger.LogDebug("Preprocessing template {Name}", template.Name);
            
            var assembles = new List<Assembly>();
            assembles.AddRange(s_defaultAssemblies);
            assembles.AddRange(template.Assemblies);

            await _engine.PreProcessAsync(template.Name, assembles);
        }
    }
}
