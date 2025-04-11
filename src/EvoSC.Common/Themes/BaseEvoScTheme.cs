using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Common.Themes;

[Theme(Name = "Default", Description = "The default theme as defined in the EvoSC# config.")]
public class BaseEvoScTheme : Theme<BaseEvoScTheme>
{
    public async override Task ConfigureAsync(dynamic theme)
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
        Set(DefaultThemeOptions.UIFontSize).To(0.9);
        
        Set(DefaultThemeOptions.UIHeaderBg).To("28212F");
        Set(DefaultThemeOptions.UIBgPrimary).To("2C2D34");
        Set(DefaultThemeOptions.UIBgHighlight).To("50515A");
        
        Set(DefaultThemeOptions.UIAccentPrimary).To("FF0058");
        Set(DefaultThemeOptions.UIAccentSecondary).To("FFFFFF");
        Set(DefaultThemeOptions.UIAccentWarmUp).To("FF9900");
        
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
        Set(DefaultThemeOptions.Red).To("E3391D");
        Set(DefaultThemeOptions.Green).To("1CE21C");
        Set(DefaultThemeOptions.Blue).To("2929EC");
        Set(DefaultThemeOptions.Pink).To("FF0058");
        Set(DefaultThemeOptions.Gray).To("888888");
        Set(DefaultThemeOptions.Orange).To("F77234");
        Set(DefaultThemeOptions.Yellow).To("FCE100");
        Set(DefaultThemeOptions.Teal).To("0FC6C2");
        Set(DefaultThemeOptions.Purple).To("722ED1");
        Set(DefaultThemeOptions.Gold).To("F6DF0F");
        Set(DefaultThemeOptions.Silver).To("C8C8C8");
        Set(DefaultThemeOptions.Bronze).To("8B670C");
        Set(DefaultThemeOptions.Grass).To("80AC20");
        Set(DefaultThemeOptions.Dirt).To("B15C2C");
        Set(DefaultThemeOptions.Tarmac).To("949494");
        Set(DefaultThemeOptions.Ice).To("AACFD7");
        
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
