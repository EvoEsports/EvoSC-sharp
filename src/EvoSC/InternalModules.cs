using EvoSC.Common.Interfaces;
using EvoSC.Modules.EvoEsports.ServerSyncModule;
using EvoSC.Modules.EvoEsports.ToornamentModule;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Nsgr.ContactAdminModule;
using EvoSC.Modules.Official.ASayModule;
using EvoSC.Modules.Official.CurrentMapModule;
using EvoSC.Modules.Official.ExampleModule;
using EvoSC.Modules.Official.FastestCpModule;
using EvoSC.Modules.Official.ForceTeamModule;
using EvoSC.Modules.Official.GameModeUiModule;
using EvoSC.Modules.Official.LiveRankingModule;
using EvoSC.Modules.Official.LocalRecordsModule;
using EvoSC.Modules.Official.MapListModule;
using EvoSC.Modules.Official.MapQueueModule;
using EvoSC.Modules.Official.MapsModule;
using EvoSC.Modules.Official.MatchManagerModule;
using EvoSC.Modules.Official.MatchRankingModule;
using EvoSC.Modules.Official.MatchReadyModule;
using EvoSC.Modules.Official.MatchTrackerModule;
using EvoSC.Modules.Official.MotdModule;
using EvoSC.Modules.Official.NextMapModule;
using EvoSC.Modules.Official.OpenPlanetModule;
using EvoSC.Modules.Official.Player;
using EvoSC.Modules.Official.PlayerRecords;
using EvoSC.Modules.Official.RoundRankingModule;
using EvoSC.Modules.Official.Scoreboard;
using EvoSC.Modules.Official.ScoreboardModule;
using EvoSC.Modules.Official.ServerManagementModule;
using EvoSC.Modules.Official.SetName;
using EvoSC.Modules.Official.SpectatorCamModeModule;
using EvoSC.Modules.Official.SpectatorTargetInfoModule;
using EvoSC.Modules.Official.TeamChatModule;
using EvoSC.Modules.Official.TeamInfoModule;
using EvoSC.Modules.Official.TeamSettingsModule;
using EvoSC.Modules.Official.WorldRecordModule;
using FluentMigrator.Runner.Exceptions;

namespace EvoSC;

public static class InternalModules
{
    public static readonly Type[] Modules =
    [
        //typeof(ExampleModule),
        typeof(GameModeUiModule),
        typeof(PlayerModule),
        typeof(MapsModule),
        typeof(WorldRecordModule),
        typeof(PlayerRecordsModule),
        typeof(MatchManagerModule),
        typeof(SetNameModule),
        typeof(ScoreboardModule),
        typeof(FastestCpModule),
        typeof(CurrentMapModule),
        typeof(MotdModule),
        typeof(OpenPlanetModule),
        typeof(MatchTrackerModule),
        typeof(MatchReadyModule),
        typeof(NextMapModule),
        typeof(LiveRankingModule),
        typeof(MatchRankingModule),
        typeof(ASayModule),
        typeof(SpectatorTargetInfoModule),
        typeof(SpectatorCamModeModule),
        typeof(MapQueueModule),
        typeof(MapListModule),
        typeof(LocalRecordsModule),
        typeof(ForceTeamModule),
        typeof(TeamSettingsModule),
        typeof(ServerManagementModule),
        typeof(TeamInfoModule),
        typeof(TeamChatModule),
        typeof(ServerSyncModule),
        typeof(ToornamentModule),
        typeof(ContactAdminModule),
        typeof(RoundRankingModule)
    ];

    /// <summary>
    /// Run any migrations from all the modules.
    /// </summary>
    /// <param name="migrations"></param>
    /// <exception cref="Exception"></exception>
    public static void RunInternalModuleMigrations(this IMigrationManager migrations)
    {
        foreach (var module in Modules)
        {
            try
            {
                migrations.MigrateFromAssembly(module.Assembly);
            }
            catch (MissingMigrationsException ex)
            {
                // ignore this as modules don't always have migrations, but we still try to find them
            }
        }
    }
    
    /// <summary>
    /// Load all internal modules.
    /// </summary>
    /// <param name="modules"></param>
    public static async Task LoadInternalModulesAsync(this IModuleManager modules)
    {
        foreach (var module in Modules)
        {
            await modules.LoadAsync(module.Assembly);
        }
    }
}
