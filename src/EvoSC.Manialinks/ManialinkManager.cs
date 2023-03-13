using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using EvoSC.Modules.Interfaces;
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

    public void AddTemplate(IManialinkTemplateInfo template)
    {
        if (_templates.ContainsKey(template.Name))
        {
            throw new InvalidOperationException($"Template '{template.Name}' already exists.");
        }
        
        _engine.AddTemplateFromString(template.Name, template.Content);
        _templates[template.Name] = template;
    }

    public Task AddTemplateAsync(IManialinkTemplateInfo template)
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

    public Task AddManiaScriptAsync(IManiaScriptInfo maniaScript)
    {
        AddManiaScript(maniaScript);
        return Task.CompletedTask;
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
