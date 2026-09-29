using EvoSC.Common.Interfaces;
using EvoSC.Modules.Interfaces;
using FluentMigrator.Runner.Exceptions;

namespace EvoSC;

/// <summary>
/// The modules that ship with EvoSC. They are registered here by id and loaded from
/// <see cref="DefaultDirectory"/>. Nothing else about them is special: their metadata, their
/// dependencies, their load context and their migrations are all handled exactly like those of an
/// external module. Registering them in code rather than discovering them is what decides what is
/// part of EvoSC, and makes a module missing from the deployment an error instead of a module that
/// silently does not show up.
/// </summary>
public static class InternalModules
{
    /// <summary>
    /// The directory the internal modules are deployed to. Fixed rather than configurable, because
    /// it is where the build stages them next to the host.
    /// </summary>
    public static string DefaultDirectory => Path.Combine(AppContext.BaseDirectory, "modules");

    /// <summary>
    /// The ids of the internal modules. These are the ids from each module's info.toml, which is
    /// what the loader matches against, and are the only place the list is written down.
    /// </summary>
    public static readonly string[] ModuleIds =
    [
        "ASayModule",
        "CurrentMapModule",
        "FastestCpModule",
        "ForceTeamModule",
        "GameModeUiModule",
        "LiveRankingModule",
        "LocalRecordsModule",
        "MapListModule",
        "MapQueueModule",
        "MapsModule",
        "MatchManagerModule",
        "MatchRankingModule",
        "MatchReadyModule",
        "MatchTrackerModule",
        "ModuleManagerModule",
        "MotdModule",
        "NextMapModule",
        "OpenPlanetModule",
        "Player",
        "RecordsModule",
        "RoundRankingModule",
        "ScoreboardModule",
        "ServerManagementModule",
        "SetNameModule",
        "SpectatorCamModeModule",
        "SpectatorTargetInfoModule",
        "TeamChatModule",
        "TeamInfoModule",
        "TeamSettingsModule",
        "UiControlModule",
        "WorldRecordModule"
    ];

    /// <summary>
    /// Runs the migrations of all loaded modules, in dependency order. A module is migrated after
    /// the modules it depends on so that it can point a foreign key at their tables. Each module
    /// is migrated from the assemblies of its own load context, so migrations stay with the module
    /// that owns them.
    /// </summary>
    /// <param name="migrations"></param>
    /// <param name="modules"></param>
    public static void RunModuleMigrations(this IMigrationManager migrations, IModuleManager modules)
    {
        foreach (var module in modules.GetLoadedModulesByDependency())
        {
            foreach (var assembly in module.Assemblies)
            {
                try
                {
                    migrations.MigrateFromAssembly(assembly);
                }
                catch (MissingMigrationsException)
                {
                    // Not every module has migrations, so a module without any is not an error.
                }
            }
        }
    }
}
