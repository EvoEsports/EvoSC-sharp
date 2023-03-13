using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using ManiaTemplates;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks;

public class ManialinkManager : IManialinkManager
{
    private readonly ILogger<ManialinkManager> _logger;
    private readonly IServerClient _server;
    private readonly ManiaTemplateEngine _engine = new();
    private readonly Dictionary<string, IManialinkTemplateInfo> _templates = new();
    private readonly Dictionary<string, IManiaScriptInfo> _scripts = new();

    public ManialinkManager(ILogger<ManialinkManager> logger, IServerClient server)
    {
        _logger = logger;
        _server = server;
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
        // todo: remove template from engine also
        _templates.Remove(name);
    }

    public void RemoveManiaScript(string name)
    {
        // todo: remove from engine
        _scripts.Remove(name);
    }

    public async Task SendManialinkAsync(string name, dynamic data)
    {
        if (!_templates.ContainsKey(name))
        {
            throw new InvalidOperationException($"Template '{name}' not found.");
        }
        
        var assemblies = new List<Assembly> {typeof(IOnlinePlayer).Assembly};
        assemblies.AddRange(_templates[name].Assemblies);
        
        var manialinkOutput = await _engine.RenderAsync(name, data, assemblies);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }

    public async Task PreprocessAllAsync()
    {
        foreach (var template in _templates.Values)
        {
            _logger.LogDebug("Preprocessing template {Name}", template.Name);
            await _engine.PreProcessAsync(template.Name, template.Assemblies);
        }
    }
}
