using EvoSC.Common.Config.Models.ThemeOptions;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Themes;
using EvoSC.Manialinks.Themes;

namespace EvoSC.Modules.Official.DefaultThemeModule;

[Theme(Name = "Default Theme", Description = "The default theme of EvoSC#.")]
public class DefaultTheme : Theme<DefaultTheme>
{
    public override void ConfigureAsync() => 
         Set("Test").To("yep")
        .Set("yeo").To("hmm")
        .Replace("my component").With("new component")
        .Set("dsgf").To("dfsg");
}
