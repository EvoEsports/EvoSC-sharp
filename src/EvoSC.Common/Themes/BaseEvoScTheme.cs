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
        Set(DefaultThemeOptions.UIFont).To("GameFontExtraBold");
        Set(DefaultThemeOptions.UIFontSize).To(1);
        Set(DefaultThemeOptions.UITextPrimary).To("FFFFFF");
        Set(DefaultThemeOptions.UITextSecondary).To("EDEDEF");
        Set(DefaultThemeOptions.UIBgPrimary).To("FF0058");
        Set(DefaultThemeOptions.UIBgSecondary).To("47495A");
        Set(DefaultThemeOptions.UIBorderPrimary).To("FF0058");
        Set(DefaultThemeOptions.UIBorderSecondary).To("FFFFFF");
        Set(DefaultThemeOptions.UILogoDark).To("");
        Set(DefaultThemeOptions.UILogoLight).To("");
    }
    
    protected void SetDefaultUtilityColors()
    {
        Set(DefaultThemeOptions.Red).To("E22000");
        Set(DefaultThemeOptions.Green).To("00D909");
        Set(DefaultThemeOptions.Blue).To("3491FA");
        Set(DefaultThemeOptions.Yellow).To("FCE100");
        Set(DefaultThemeOptions.Teal).To("0FC6C2");
        Set(DefaultThemeOptions.Purple).To("722ED1");
        Set(DefaultThemeOptions.Gold).To("FFD000");
        Set(DefaultThemeOptions.Silver).To("9e9e9e");
        Set(DefaultThemeOptions.Bronze).To("915d29");
        Set(DefaultThemeOptions.Grass).To("9FDB1D");
        Set(DefaultThemeOptions.Orange).To("F77234");
        Set(DefaultThemeOptions.Gray).To("191A21");
        Set(DefaultThemeOptions.Black).To("000000");
        Set(DefaultThemeOptions.White).To("FFFFFF");
        Set(DefaultThemeOptions.Pink).To("FF0058");
        
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
        GenerateShades(DefaultThemeOptions.Yellow);
        GenerateShades(DefaultThemeOptions.Teal);
        GenerateShades(DefaultThemeOptions.Purple);
        GenerateShades(DefaultThemeOptions.Gold);
        GenerateShades(DefaultThemeOptions.Silver);
        GenerateShades(DefaultThemeOptions.Bronze);
        GenerateShades(DefaultThemeOptions.Grass);
        GenerateShades(DefaultThemeOptions.Orange);
        GenerateShades(DefaultThemeOptions.Gray);
        GenerateShades(DefaultThemeOptions.Pink);
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
