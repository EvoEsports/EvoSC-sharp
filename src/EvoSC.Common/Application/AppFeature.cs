using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Application;

public enum AppFeature
{
    All,
    
    [Identifier(NoPrefix = true)]
    Config,
    
    [Identifier(NoPrefix = true)]
    Logging,
    
    [Identifier(NoPrefix = true)]
    DatabaseMigrations,
    
    [Identifier(NoPrefix = true)]
    Database,
    
    [Identifier(NoPrefix = true)]
    Events,
    
    [Identifier(NoPrefix = true)]
    GbxRemoteClient,
    
    [Identifier(NoPrefix = true)]
    Modules,
    
    [Identifier(NoPrefix = true)]
    ControllerManager,
    
    [Identifier(NoPrefix = true)]
    PlayerManager,
    
    [Identifier(NoPrefix = true)]
    MapsManager,
    
    [Identifier(NoPrefix = true)]
    PlayerCache,
    
    [Identifier(NoPrefix = true)]
    MatchSettings,
    
    [Identifier(NoPrefix = true)]
    Auditing,
    
    [Identifier(NoPrefix = true)]
    ServicesManager,
    
    [Identifier(NoPrefix = true)]
    ChatCommands,
    
    [Identifier(NoPrefix = true)]
    ActionPipelines,
    
    [Identifier(NoPrefix = true)]
    Permissions,
    
    [Identifier(NoPrefix = true)]
    Manialinks
}
