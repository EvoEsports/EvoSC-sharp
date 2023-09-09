using EvoSC.Common.Config.Models.ThemeOptions;

namespace EvoSC.Common.Config.Models;

public interface IThemeConfig
{
    public IChatThemeConfig Chat { get; }
    public IUIThemeConfig UI { get; }
}
