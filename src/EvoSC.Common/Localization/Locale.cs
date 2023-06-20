using System.Globalization;
using System.Reflection;
using System.Resources;
using EvoSC.Common.Interfaces.Localization;

namespace EvoSC.Common.Localization;

public class Locale : ILocale
{
    private readonly ResourceManager _resourceManager;
    private readonly List<CultureInfo> _availableCultures = new();

    public Locale(Assembly assembly, string resource)
    {
        _resourceManager = new ResourceManager(resource, assembly);
        _resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
    }

    public string Get(string name, params object[] args)
    {
        var localeString = _resourceManager.GetString(name);

        if (localeString == null)
        {
            throw new KeyNotFoundException($"Failed to find locale name {name}.");
        }

        return string.Format(localeString, args);
    }
}
