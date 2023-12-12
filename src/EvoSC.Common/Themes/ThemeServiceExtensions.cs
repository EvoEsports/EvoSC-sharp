using EvoSC.Common.Interfaces.Themes;
using SimpleInjector;

namespace EvoSC.Common.Themes;

public static class ThemesServiceExtensions
{
    public static Container AddEvoScThemes(this Container services)
    {
        services.RegisterSingleton<IThemeManager, ThemeManager>();
        
        return services;
    }
}
