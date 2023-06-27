using System.Dynamic;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;

namespace EvoSC.Common.Localization;

public class LocaleResource : Locale
{
    private readonly ILocalizationManager _localeManager;
    private readonly IContextService _context;
    private readonly IEvoScBaseConfig _config;
    
    private bool _useDefaultCulture = true;

    private static readonly Regex TranslationTag =
        new(@"\[([\w\d_]+)\]", RegexOptions.Compiled, TimeSpan.FromMilliseconds(50));

    public override string this[string name, params object[] args] => GetString(name, args);

    public override Locale PlayerLanguage => UsePlayerLanguage();

    public LocaleResource(ILocalizationManager localeManager, IContextService context, IEvoScBaseConfig config)
    {
        _localeManager = localeManager;
        _context = context;
        _config = config;
    }

    public override ResourceSet? GetResourceSet() =>
        _localeManager.Manager.GetResourceSet(GetCulture(), true, true);

    public override string Translate(string pattern, params object[] args)
    {
        var matches = TranslationTag.Matches(pattern);
        var sb = new StringBuilder();

        var currIndex = 0;
        foreach (Match match in matches)
        {
            sb.Append(pattern.Substring(currIndex, match.Index - currIndex));
            var translation = GetString(match.Groups[1].Value, args);
            currIndex = match.Index + match.Value.Length;

            sb.Append(translation);
        }

        if (currIndex + 1 < pattern.Length)
        {
            sb.Append(pattern.Substring(currIndex));
        }

        return sb.ToString();
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

    private Locale UsePlayerLanguage()
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

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var name = binder.Name.Replace("_", ".", StringComparison.Ordinal);
        result = this[name];
        return true;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var name = binder.Name.Replace("_", ".", StringComparison.Ordinal);
        result = this[name, args!];
        return true;
    }
}
