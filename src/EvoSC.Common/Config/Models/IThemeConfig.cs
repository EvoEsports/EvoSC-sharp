using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Config.Models.ThemeOptions;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Config.Models;

public interface IThemeConfig
{
    public IChatThemeConfig Chat { get; }
}
