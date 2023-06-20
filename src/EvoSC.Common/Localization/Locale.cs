using System.Globalization;
using System.Reflection;
using System.Resources;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;

namespace EvoSC.Common.Localization;

public class Locale : ILocale
{
    private readonly ILocalizationManager _localeManager;
    private readonly IContextService _context;
    private readonly IEvoScBaseConfig _config;
    
    private bool _useDefaultCulture = true;

    public string this[string name, params object[] args] => GetString(name, args);

    ILocale ILocale.PlayerLanguage => UsePlayerLanguage();

    public Locale(ILocalizationManager localeManager, IContextService context, IEvoScBaseConfig config)
    {
        _localeManager = localeManager;
        _context = context;
        _config = config;
    }

    private CultureInfo GetCulture()
    {
        var context = _context.GetContext() as PlayerInteractionContext;

        if (_useDefaultCulture || context?.Player?.Settings == null)
        {
            return CultureInfo.GetCultureInfo(_config.Locale.DefaultLanguage);
        }

        return CultureInfo.GetCultureInfo(context.Player.Settings.DisplayLanguage);
    }

    private ILocale UsePlayerLanguage()
    {
        _useDefaultCulture = false;
        return this;
    }
    
    private string GetString(string name, params object[] args)
    {
        var localString = _localeManager.GetString(GetCulture(), name, args);
        _useDefaultCulture = true;
        return localString;
    }
}
