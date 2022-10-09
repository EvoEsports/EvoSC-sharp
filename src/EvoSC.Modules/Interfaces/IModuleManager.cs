using System.Reflection;

namespace EvoSC.Modules;

public interface IModuleManager
{
    public Task LoadModulesFromAssembly(Assembly assembly);
}
