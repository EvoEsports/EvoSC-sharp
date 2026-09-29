using System.Globalization;
using System.Reflection;
using System.Resources;
using EvoSC.Common.Interfaces.Localization;
using Microsoft.Extensions.Logging;

namespace EvoSC.Common.Localization;

public class LocalizationManager : ILocalizationManager
{
    private readonly ResourceManager _resourceManager;
    private readonly ILogger<LocalizationManager> _logger;

    public LocalizationManager(Assembly assembly, string resource, ILogger<LocalizationManager> logger)
    {
        _logger = logger;
        _resourceManager = new ResourceManager(resource, assembly);
        
        // verify resource
        _resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
    }

    public ResourceManager Manager => _resourceManager;

    public string GetString(CultureInfo culture, string name, params object[] args)
    {
        var localeString = _resourceManager.GetString(name, culture);

        if (localeString == null)
        {
            _logger.LogWarning("Failed to find locale name '{Name}', using the key as the value.", name);

            return name;
        }

        return string.Format(localeString, args);
    }
}