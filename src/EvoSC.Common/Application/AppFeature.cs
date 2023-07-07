using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Application;

public enum AppFeature
{
    /// <summary>
    /// All features available in EvoSC.
    /// </summary>
    All,
    
    /// <summary>
    /// The EvoSC base configuration.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Config,
    
    /// <summary>
    /// The standard EvoSC logger.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Logging,
    
    /// <summary>
    /// Standard EvoSC database migrations.
    /// </summary>
    [Identifier(NoPrefix = true)]
    DatabaseMigrations,
    
    /// <summary>
    /// Database connection and repositories.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Database,
    
    /// <summary>
    /// The event system.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Events,
    
    /// <summary>
    /// Connection to the Trackmania server through XMLRPC.
    /// </summary>
    [Identifier(NoPrefix = true)]
    GbxRemoteClient,
    
    /// <summary>
    /// The module system.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Modules,
    
    /// <summary>
    /// Controller framework and manager.
    /// </summary>
    [Identifier(NoPrefix = true)]
    ControllerManager,
    
    /// <summary>
    /// Player manager to manage players offline and online.
    /// </summary>
    [Identifier(NoPrefix = true)]
    PlayerManager,
    
    /// <summary>
    /// Maps manager to manage, add or remove maps.
    /// </summary>
    [Identifier(NoPrefix = true)]
    MapsManager,
    
    /// <summary>
    /// Cache for fast retrieval of player information.
    /// </summary>
    [Identifier(NoPrefix = true)]
    PlayerCache,
    
    /// <summary>
    /// Features to manage match settings and game mode.
    /// </summary>
    [Identifier(NoPrefix = true)]
    MatchSettings,
    
    /// <summary>
    /// Audit logging.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Auditing,
    
    /// <summary>
    /// Manager for multi-service-container features. Mostly used for modules.
    /// </summary>
    [Identifier(NoPrefix = true)]
    ServicesManager,
    
    /// <summary>
    /// Chat command framework.
    /// </summary>
    [Identifier(NoPrefix = true)]
    ChatCommands,
    
    /// <summary>
    /// Middlewares and action pipelines.
    /// </summary>
    [Identifier(NoPrefix = true)]
    ActionPipelines,
    
    /// <summary>
    /// Player groups and permissions.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Permissions,
    
    /// <summary>
    /// Manage, add, remove, display manialinks and respond to actions. The manialink framework.
    /// </summary>
    [Identifier(NoPrefix = true)]
    Manialinks
}
