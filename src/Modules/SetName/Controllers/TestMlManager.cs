using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Modules.Official.SetName.Controllers;

public class TestMlManager : IManialinkManager
{
    public Task AddDefaultTemplatesAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAndPreProcessTemplateAsync(IManialinkTemplateInfo template)
    {
        throw new NotImplementedException();
    }

    public Task AddManiaScriptAsync(IManiaScriptInfo maniaScript)
    {
        throw new NotImplementedException();
    }

    public void RemoveTemplate(string name)
    {
        throw new NotImplementedException();
    }

    public void RemoveManiaScript(string name)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(string name, IDictionary<string, object?> data)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(string name, dynamic data)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task SendPersistentManialinkAsync(string name, IDictionary<string, object?> data)
    {
        throw new NotImplementedException();
    }

    public Task SendPersistentManialinkAsync(string name, dynamic data)
    {
        throw new NotImplementedException();
    }

    public Task SendPersistentManialinkAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(IPlayer player, string name, IDictionary<string, object?> data)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(IPlayer player, string name, dynamic data)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, IDictionary<string, object?> data)
    {
        throw new NotImplementedException();
    }

    public Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, dynamic data)
    {
        throw new NotImplementedException();
    }

    public Task HideManialinkAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task HideManialinkAsync(IPlayer player, string name)
    {
        throw new NotImplementedException();
    }

    public Task HideManialinkAsync(IEnumerable<IPlayer> players, string name)
    {
        throw new NotImplementedException();
    }

    public Task PreprocessAllAsync()
    {
        throw new NotImplementedException();
    }

    public void AddTemplate(IManialinkTemplateInfo template)
    {
        throw new NotImplementedException();
    }

    public void AddManiaScript(IManiaScriptInfo maniaScript)
    {
        throw new NotImplementedException();
    }
}
