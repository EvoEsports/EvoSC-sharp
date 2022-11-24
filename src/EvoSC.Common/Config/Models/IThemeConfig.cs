using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public class IThemeConfig
{
    [Description("The primary text color to use.")]
    [Option(Alias = "primaryTextColor", DefaultValue = "fff")]
    public string PrimaryTextColor { get; }
    [Description("The primary text color to use.")]
    [Option(Alias = "primaryTextColor", DefaultValue = "396")]
    public string SecondaryTextColor { get; }
}
