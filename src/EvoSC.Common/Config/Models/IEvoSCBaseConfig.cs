namespace EvoSC.Common.Config.Models;

public interface IEvoSCBaseConfig
{
    public IDatabaseConfig Database { get; set; }
    public ILoggingConfig Logging { get; set; }
    public IServerConfig Server { get; set; }
    public IPathConfig Path { get; set; }
}
