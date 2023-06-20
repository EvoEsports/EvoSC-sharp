using System.Globalization;

namespace EvoSC.Common.Interfaces.Localization;

public interface ILocalizationManager
{
    public string GetString(CultureInfo culture, string name, params object[] args);
}
