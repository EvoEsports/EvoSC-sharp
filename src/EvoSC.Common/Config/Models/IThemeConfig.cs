using System.ComponentModel;
using Config.Net;

namespace EvoSC.Common.Config.Models;

public interface IThemeConfig
{

    [Description("The primary text color to use.")]
    [Option(Alias = "Chat.primaryTextColor", DefaultValue = "fff")]
    public string PrimaryTextColorChat { get; }

    [Description("The secondary text color to use.")]
    [Option(Alias = "secondaryTextColor", DefaultValue = "eee")]
    public string SecondaryTextColorChat { get; }

    [Description("The text color to use for info.")]
    [Option(Alias = "infoTextColor", DefaultValue = "29b")]
    public string InfoTextColorChat { get; }

    [Description("The text color to use for errors.")]
    [Option(Alias = "errorTextColor", DefaultValue = "c44")]
    public string ErrorTextColorChat { get; }

    [Description("The text color to use for warnings.")]
    [Option(Alias = "warningTextColor", DefaultValue = "e83")]
    public string WarningTextColorChat { get; }

    [Description("The text color to use for success.")]
    [Option(Alias = "successTextColor", DefaultValue = "5b6")]
    public string SuccessTextColorChat { get; }
}
