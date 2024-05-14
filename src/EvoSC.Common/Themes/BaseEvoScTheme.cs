using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Common.Themes;

[Theme(Name = "Default", Description = "The default theme as defined in the EvoSC# config.")]
public class BaseEvoScTheme : Theme<BaseEvoScTheme>
{
    public async override Task ConfigureAsync()
    {
        SetDefaultUtilityColors();
        GenerateUtilityColorShades();
        
        SetDefaultChatColors();
        SetDefaultThemeOptions();
    }

    protected void SetDefaultChatColors()
    {
        Set(DefaultThemeOptions.ChatPrimary).To("fff");
        Set(DefaultThemeOptions.ChatSecondary).To("eee");
        Set(DefaultThemeOptions.ChatInfo).To("29b");
        Set(DefaultThemeOptions.ChatDanger).To("c44");
        Set(DefaultThemeOptions.ChatWarning).To("e83");
        Set(DefaultThemeOptions.ChatSuccess).To("5b6");
    }

    protected void SetDefaultThemeOptions()
    {
        Set(DefaultThemeOptions.UIFont).To("GameFontSemiBold");
        Set(DefaultThemeOptions.UIFontSize).To(1);
        
        Set(DefaultThemeOptions.UIHeaderBg).To("28212F");
        Set(DefaultThemeOptions.UIBgPrimary).To("28212F");
        Set(DefaultThemeOptions.UIBgHighlight).To("50515A");
        
        Set(DefaultThemeOptions.UIAccentPrimary).To("FF0058");
        Set(DefaultThemeOptions.UIAccentSecondary).To("FFFFFF");
        
        Set(DefaultThemeOptions.UISurfaceBgPrimary).To("454252");
        Set(DefaultThemeOptions.UISurfaceBgSecondary).To("FFFFFF");
        
        Set(DefaultThemeOptions.UITextPrimary).To("FFFFFF");
        Set(DefaultThemeOptions.UITextSecondary).To("2D2D2D");
        Set(DefaultThemeOptions.UITextMuted).To("A2A0AD");
        
        Set(DefaultThemeOptions.UILogoDark).To("");
        Set(DefaultThemeOptions.UILogoLight).To("");
    }
    
    protected void SetDefaultUtilityColors()
    {
        Set(DefaultThemeOptions.Red).To("E22000");
        Set(DefaultThemeOptions.Green).To("E22000");
        Set(DefaultThemeOptions.Blue).To("E22000");
        Set(DefaultThemeOptions.Pink).To("E22000");
        Set(DefaultThemeOptions.Gray).To("E22000");
        Set(DefaultThemeOptions.Orange).To("E22000");
        Set(DefaultThemeOptions.Yellow).To("E22000");
        Set(DefaultThemeOptions.Teal).To("E22000");
        Set(DefaultThemeOptions.Purple).To("E22000");
        Set(DefaultThemeOptions.Gold).To("E22000");
        Set(DefaultThemeOptions.Silver).To("E22000");
        Set(DefaultThemeOptions.Bronze).To("E22000");
        Set(DefaultThemeOptions.Grass).To("E22000");
        Set(DefaultThemeOptions.Dirt).To("E22000");
        Set(DefaultThemeOptions.Tarmac).To("E22000");
        Set(DefaultThemeOptions.Ice).To("E22000");
        
        Set(DefaultThemeOptions.Black).To("000000");
        Set(DefaultThemeOptions.White).To("FFFFFF");
        
        Set(DefaultThemeOptions.Info).To("29b");
        Set(DefaultThemeOptions.Success).To("c44");
        Set(DefaultThemeOptions.Warning).To("e83");
        Set(DefaultThemeOptions.Danger).To("5b6");
    }
    
    protected void GenerateUtilityColorShades()
    {
        GenerateShades(DefaultThemeOptions.Red);
        GenerateShades(DefaultThemeOptions.Green);
        GenerateShades(DefaultThemeOptions.Blue);
        GenerateShades(DefaultThemeOptions.Pink);
        GenerateShades(DefaultThemeOptions.Gray);
        GenerateShades(DefaultThemeOptions.Orange);
        GenerateShades(DefaultThemeOptions.Yellow);
        GenerateShades(DefaultThemeOptions.Teal);
        GenerateShades(DefaultThemeOptions.Purple);
        GenerateShades(DefaultThemeOptions.Gold);
        GenerateShades(DefaultThemeOptions.Silver);
        GenerateShades(DefaultThemeOptions.Bronze);
        GenerateShades(DefaultThemeOptions.Grass);
        GenerateShades(DefaultThemeOptions.Dirt);
        GenerateShades(DefaultThemeOptions.Tarmac);
        GenerateShades(DefaultThemeOptions.Ice);
    }
    
    private void GenerateShades(string key)
    {
        var color = (string)ThemeOptions[key];

        for (var i = 1; i < 10; i++)
        {
            var lightness = i * 10f;
            var shade = ColorUtils.SetLightness(color, lightness);
            Set($"{key}{lightness}").To(shade);
        }
    }
}
