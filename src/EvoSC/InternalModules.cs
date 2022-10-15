using EvoSC.Common.Interfaces;
using EvoSC.Modules;
using EvoSC.Modules.Official.Player;

namespace EvoSC;

public static class InternalModules
{
    public static List<Type> Modules = new()
    {
        typeof(PlayerModule)
    };

    public static void RunInternalModuleMigrations(this IMigrationManager migrations)
    {
        foreach (var module in Modules)
        {
            migrations.MigrateFromAssembly(module.Assembly);
        }
    }
    
    public static async Task LoadInternalModules(this IModuleManager modules)
    {
        foreach (var module in Modules)
        {
            await modules.LoadModulesFromAssembly(module.Assembly);
        }
    }
}
