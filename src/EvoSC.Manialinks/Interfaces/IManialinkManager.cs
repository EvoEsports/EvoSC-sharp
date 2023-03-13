using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkManager
{
    public Task AddDefaultTemplatesAsync();
    internal void AddTemplate(IManialinkTemplateInfo template);
    public Task AddTemplateAsync(IManialinkTemplateInfo template);
    internal void AddManiaScript(IManiaScriptInfo maniaScript);
    public Task AddManiaScriptAsync(IManiaScriptInfo maniaScript);
    public void RemoveTemplate(string name);
    public void RemoveManiaScript(string name);
    public Task SendManialinkAsync(string name, dynamic data);
    public Task PreprocessAllAsync();
}
