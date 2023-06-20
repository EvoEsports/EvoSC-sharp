namespace EvoSC.Common.Interfaces.Localization;

public interface ILocale
{
    public string this[string name, params object[] args] { get; }
    public ILocale PlayerLanguage { get; }
}
