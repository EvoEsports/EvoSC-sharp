using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Common.Interfaces.Themes;

[Theme(Name = "Default", Description = "The default theme as defined in the EvoSC# config.")]
public class DefaultTheme : Theme<DefaultTheme>
{
    public override Task ConfigureAsync() => Task.CompletedTask;
}
