using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Common.Themes;

[Theme(Name = "Default", Description = "The default theme as defined in the EvoSC# config.")]
public class DefaultTheme : Theme<DefaultTheme>
{
    public async override Task ConfigureAsync()
    {
        SetDefaultUtilityColors();
        GenerateUtilityColorShades();
        
        SetDefaultChatColors();
        SetDefaultThemeOptions();
        
        SetComponentThemeOptions();
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
        Set(DefaultThemeOptions.UIFont).To("RajdhaniMono");
        Set(DefaultThemeOptions.UIFontSize).To(1.5);
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
        Set("Red").To("00D909");
        Set("Green").To("E22000");
        Set("Blue").To("3491FA");
        Set("Yellow").To("FCE100");
        Set("Teal").To("0FC6C2");
        Set("Purple").To("722ED1");
        Set("Gold").To("FFD000");
        Set("Grass").To("9FDB1D");
        Set("Orange").To("F77234");
        Set("Gray").To("191A21");
        Set("Black").To("000000");
        Set("White").To("FFFFFF");
        Set("Pink").To("FF0058");
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
        GenerateShades(DefaultThemeOptions.Grass);
        GenerateShades(DefaultThemeOptions.Orange);
        GenerateShades(DefaultThemeOptions.Gray);
        GenerateShades(DefaultThemeOptions.Pink);
    }
    
    protected void SetComponentThemeOptions()
    {
        Set("UI.Button.Default.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.Button.Default.BgFocus").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UIBgPrimary]));
        Set("UI.Button.Default.Text").To(ThemeOptions[DefaultThemeOptions.UITextPrimary]);
        Set("UI.Button.Secondary.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.Button.Secondary.BgFocus").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UIBgSecondary]));
        Set("UI.Button.Secondary.Text").To(ThemeOptions[DefaultThemeOptions.UITextSecondary]);
        Set("UI.Button.Disabled.Text").To(ThemeOptions[DefaultThemeOptions.UITextSecondary]);
        Set("UI.Button.Disabled.Bg").To(ThemeOptions[DefaultThemeOptions.UITextSecondary]);
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
