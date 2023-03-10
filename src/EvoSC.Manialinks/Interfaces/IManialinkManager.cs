using System.Reflection;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkManager
{
    public void AddTemplate(string name, string content);
    public void RemoveTemplate(string name);
    public Task SendManialinkAsync(string name, dynamic data);
}
