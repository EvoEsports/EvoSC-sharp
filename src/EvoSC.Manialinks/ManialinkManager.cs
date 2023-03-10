using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using ManiaTemplates;
using Microsoft.Extensions.Logging;

namespace EvoSC.Manialinks;

public class ManialinkManager : IManialinkManager
{
    private readonly ILogger<ManialinkManager> _logger;
    private readonly IServerClient _server;
    private readonly ManiaTemplateEngine _engine = new();

    public ManialinkManager(ILogger<ManialinkManager> logger, IServerClient server)
    {
        _logger = logger;
        _server = server;
    }

    public void AddTemplate(string name, string content)
    {
        _engine.AddTemplateFromString(name, content);
    }

    public void RemoveTemplate(string name)
    {
    }

    public async Task SendManialinkAsync(string name, dynamic data)
    {
        var manialinkOutput = _engine.Render(name, data);
        await _server.Remote.SendDisplayManialinkPageAsync(manialinkOutput, 0, false);
    }
}
