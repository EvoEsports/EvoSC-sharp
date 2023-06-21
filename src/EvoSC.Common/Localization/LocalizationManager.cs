using System.Globalization;
using System.Reflection;
using System.Resources;
using EvoSC.Common.Interfaces.Localization;

namespace EvoSC.Common.Localization;

public class LocalizationManager : ILocalizationManager
{
    private readonly ResourceManager _resourceManager;

    public LocalizationManager(Assembly assembly, string resource)
    {
        _resourceManager = new ResourceManager(resource, assembly);
        _resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
    }

    public ResourceManager Manager => _resourceManager;

    public string GetString(CultureInfo culture, string name, params object[] args)
    {
        var localeString = _resourceManager.GetString(name, culture);

        if (localeString == null)
        {
            throw new KeyNotFoundException($"Failed to find locale name {name}.");
        }

        return string.Format(localeString, args);
    }
}
