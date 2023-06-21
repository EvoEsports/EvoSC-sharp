using System.Globalization;
using System.Resources;

namespace EvoSC.Common.Interfaces.Localization;

public interface ILocalizationManager
{
    public ResourceManager Manager { get; }
    public string GetString(CultureInfo culture, string name, params object[] args);
}
