namespace EvoSC.Common.Interfaces;

public interface IConfig
{
    public TConfig Get<TConfig>(string key);
}
