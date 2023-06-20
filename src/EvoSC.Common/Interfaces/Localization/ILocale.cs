namespace EvoSC.Common.Interfaces.Localization;

public interface ILocale
{
    public string this[string name, params object[] args] { get => Get(name, args); }

    public string Get(string name, params object[] args);
}
