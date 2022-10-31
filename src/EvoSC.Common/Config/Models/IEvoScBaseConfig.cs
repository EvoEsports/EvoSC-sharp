namespace EvoSC.Common.Config.Models;

public interface IEvoScBaseConfig
{
    public IDatabaseConfig Database { get; set; }
    public ILoggingConfig Logging { get; set; }
    public IServerConfig Server { get; set; }
}
