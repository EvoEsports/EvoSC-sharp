using System.Reflection;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkManager
{
    public void AddTemplate(IManialinkTemplateInfo template);
    public void AddManiaScript(IManiaScriptInfo maniaScript);
    public void RemoveTemplate(string name);
    public void RemoveManiaScript(string name);
    public Task SendManialinkAsync(string name, dynamic data);
    public Task PreprocessAllAsync();
}
