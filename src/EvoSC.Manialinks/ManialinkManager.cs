using System.Collections.Concurrent;
using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Remote;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Events;
using EvoSC.Common.Themes.Events.Args;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using EvoSC.Manialinks.Themes;
using EvoSC.Manialinks.Util;
using GbxRemoteNet;
using GbxRemoteNet.Events;
using ManiaTemplates;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks;

public class ManialinkManager : IManialinkManager
{
    private readonly ILogger<ManialinkManager> _logger;
    private readonly IServerClient _server;
    private readonly IThemeManager _themeManager;

    private readonly ManiaTemplateEngine _engine = new();
    private readonly Dictionary<string, IManialinkTemplateInfo> _templates = new();
    private readonly Dictionary<string, IManiaScriptInfo> _scripts = new();
    private readonly ConcurrentDictionary<string, IPersistentManialink> _persistentManialinks = new();

    private static IEnumerable<Assembly> s_defaultAssemblies = new[]
    {
        typeof(IOnlinePlayer).Assembly, typeof(ManialinkManager).Assembly
    };

    public ManialinkManager(ILogger<ManialinkManager> logger, IServerClient server, IEventManager events,
        IThemeManager themeManager)
    {
        _logger = logger;
        _server = server;
        _themeManager = themeManager;

        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.PlayerConnect)
            .WithInstance(this)
            .WithInstanceClass<ManialinkManager>()
            .WithHandlerMethod<PlayerConnectGbxEventArgs>(HandlePlayerConnectAsync)
            .AsAsync()
        );

        events.Subscribe(s => s
            .WithPriority(EventPriority.High)
            .WithEvent(ThemeEvents.CurrentThemeChanged)
            .WithInstance(this)
            .WithInstanceClass<ManialinkManager>()
            .WithHandlerMethod<ThemeUpdatedEventArgs>(HandleThemeActivatedAsync));

        themeManager.AddThemeAsync<BaseEvoScTheme>();
        themeManager.AddThemeAsync<DefaultAlertTheme>();
        themeManager.AddThemeAsync<DefaultButtonTheme>();
        themeManager.AddThemeAsync<DefaultCheckboxTheme>();
        themeManager.AddThemeAsync<DefaultRadioButtonTheme>();
        themeManager.AddThemeAsync<DefaultTextInputTheme>();
        themeManager.AddThemeAsync<DefaultToggleSwitchTheme>();
        themeManager.AddThemeAsync<DefaultWindowTheme>();
        themeManager.AddThemeAsync<DefaultChipTheme>();
        themeManager.AddThemeAsync<DefaultSeparatorTheme>();
        themeManager.AddThemeAsync<DefaultSelectTheme>();
        themeManager.AddThemeAsync<DefaultDialogTheme>();
        
        _engine.GlobalVariables["Util"] = new GlobalManialinkUtils(themeManager);
        _engine.GlobalVariables["Icons"] = new GameIcons();
        _engine.GlobalVariables["Font"] = new FontManialinkHelper(themeManager);
        _engine.GlobalVariables["Color"] = new ColorUtils();
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

    public async Task RemoveAndHideTemplateAsync(string name)
    {
        await HideManialinkAsync(name);
        RemoveTemplate(name);
    }

    public void RemoveManiaScript(string name)
    {
        _engine.RemoveManiaScript(name);
        _scripts.Remove(name);
    }

    public async Task SendManialinkAsync(string name, IDictionary<string, object?> data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(string name, dynamic data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }

    public Task SendManialinkAsync(string name) => SendManialinkAsync(name, new { });

    public async Task SendPersistentManialinkAsync(string name, IDictionary<string, object?> data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
        _persistentManialinks[name] = new PersistentManialink
        {
            Name = name,
            Type = PersistentManialinkType.Static,
            CompiledOutput = manialinkOutput 
        };
    }

    public async Task SendPersistentManialinkAsync(string name, dynamic data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
        _persistentManialinks[name] = new PersistentManialink
        {
            Name = name,
            Type = PersistentManialinkType.Static,
            CompiledOutput = manialinkOutput 
        };
    }

    public Task SendPersistentManialinkAsync(string name) => SendPersistentManialinkAsync(name, new { });

    public Task SendPersistentManialinkAsync(string name, Func<Task<dynamic>> setupData) =>
        SendPersistentManialinkAsync(name, async Task<IDictionary<string, object?>> () =>
        {
            var rawData = await setupData();
            IDictionary<string, object?> data = new ExpandoObject();

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(rawData.GetType()))
            {
                data.Add(prop.Name, prop.GetValue(rawData));
            }
            
            return data;
        });

    public async Task SendPersistentManialinkAsync(string name, Func<Task<IDictionary<string, object?>>> setupData)
    {
        name = GetEffectiveName(name);
        
        _persistentManialinks[name] = new PersistentManialink
        {
            Name = name,
            Type = PersistentManialinkType.Static,
            DynamicDataCallbackAsync = setupData
        };

        var data = await setupData();
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }

    public Task RemovePersistentManialinkAsync(string name)
    {
        name = GetEffectiveName(name);
        _persistentManialinks.TryRemove(name, out _);
        return Task.CompletedTask;
    }

    public async Task SendManialinkAsync(IPlayer player, string name, IDictionary<string, object?> data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageToLoginAsync(player.GetLogin(), manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(IPlayer player, string name, dynamic data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageToLoginAsync(player.GetLogin(), manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(string playerLogin, string name, dynamic data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        await _server.Remote.SendDisplayManialinkPageToLoginAsync(playerLogin, manialinkOutput, 0, false);
    }

    public async Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, IDictionary<string, object?> data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        var multiCall = CreateMultiCall(players, manialinkOutput);
        await _server.Remote.MultiCallAsync(multiCall);
    }

    public async Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, dynamic data)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = await PrepareAndRenderAsync(name, data);
        var multiCall = CreateMultiCall(players, manialinkOutput);
        await _server.Remote.MultiCallAsync(multiCall);
    }

    public Task HideManialinkAsync(string name)
    {
        name = GetEffectiveName(name);
        _persistentManialinks.TryRemove(name, out _);
        var manialinkOutput = ManialinkUtils.CreateHideManialink(name);
        return _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 3, true);
    }

    public Task HideManialinkAsync(IPlayer player, string name)
    {
        name = GetEffectiveName(name);
        _persistentManialinks.TryRemove(name, out _);
        var manialinkOutput = ManialinkUtils.CreateHideManialink(name);
        return _server.Remote.SendDisplayManialinkPageToLoginAsync(player.GetLogin(), manialinkOutput, 3, true);
    }

    public Task HideManialinkAsync(string playerLogin, string name)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = ManialinkUtils.CreateHideManialink(name);
        return _server.Remote.SendDisplayManialinkPageToLoginAsync(playerLogin, manialinkOutput, 3, true);
    }

    public Task HideManialinkAsync(IEnumerable<IPlayer> players, string name)
    {
        name = GetEffectiveName(name);
        var manialinkOutput = ManialinkUtils.CreateHideManialink(name);
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

    public void AddGlobalVariable<T>(string name, T value) =>
        _engine.GlobalVariables.AddOrUpdate(name, value, (_, _) => value);

    public void RemoveGlobalVariable(string name)
    {
        if (_engine.GlobalVariables.ContainsKey(name))
        {
            _engine.GlobalVariables.Remove(name, out _);
        }

        throw new KeyNotFoundException($"Did not find global variable named '{name}'.");
    }

    public void ClearGlobalVariables() => _engine.GlobalVariables.Clear();

    /// <summary>
    /// Used to send persistent manialinks to newly connected players.
    /// </summary>
    private async Task HandlePlayerConnectAsync(object sender, PlayerConnectGbxEventArgs e)
    {
        try
        {
            foreach (var (_, manialink) in _persistentManialinks)
            {
                string? output = null;
                
                switch (manialink.Type)
                {
                    case PersistentManialinkType.Static:
                        output = manialink.CompiledOutput;
                        break;
                    case PersistentManialinkType.Dynamic:
                        {
                            var data = await manialink.DynamicDataCallbackAsync?.Invoke();
                            output = await PrepareAndRenderAsync(manialink.Name, data);
                        }
                        break;
                }

                if (output == null)
                {
                    _logger.LogWarning("Failed to get output of persistent ({Type}) manialink: {Name}",
                        manialink.Type switch
                        {
                            PersistentManialinkType.Dynamic => "Dynamic",
                            PersistentManialinkType.Static => "Static",
                            _ => "Unknown",
                        },
                        manialink.Name);
                    continue;
                }
                
                await _server.Remote.SendDisplayManialinkPageToLoginAsync(e.Login, manialink.CompiledOutput, 0, false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send persistent manialink login '{Login}'. Did they leave already?",
                e.Login);
        }
    }
    
    private Task HandleThemeActivatedAsync(object sender, ThemeUpdatedEventArgs e)
    {
        _engine.GlobalVariables["Theme"] = _themeManager.Theme;

        return Task.CompletedTask;
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

    public async Task<string> PrepareAndRenderAsync(string name, IDictionary<string, object?> data)
    {
        var assemblies = PrepareRender(name);
        return await _engine.RenderAsync(name, data, assemblies);
    }
    
    public async Task<string> PrepareAndRenderAsync(string name, dynamic data)
    {
        var assemblies = PrepareRender(name);
        return await _engine.RenderAsync(name, data, assemblies);
    }

    public string GetEffectiveName(string name) =>
        _themeManager.ComponentReplacements.TryGetValue(name, out var effectiveName)
            ? effectiveName
            : name;

    public ManialinkTransaction CreateTransaction() => new(this, _server);
}
