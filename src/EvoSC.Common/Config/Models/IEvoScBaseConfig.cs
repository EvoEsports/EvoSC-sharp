using EvoSC.Common.Themes;

namespace EvoSC.Common.Config.Models;

public interface IEvoScBaseConfig
{
    public IDatabaseConfig Database { get; set; }
    public ILoggingConfig Logging { get; set; }
    public IServerConfig Server { get; set; }
    public IPathConfig Path { get; set; }
    // public IThemeConfig Theme { get; set; }
    public IModuleConfig Modules { get; set; }
    public ILocaleConfig Locale { get; set; }
    
    public DynamicThemeOptions Theme { get; set; }
}
