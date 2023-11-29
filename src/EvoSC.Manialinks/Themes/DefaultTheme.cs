using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;
using EvoSC.Common.Util.TextFormatting;

namespace EvoSC.Manialinks.Themes;

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
        Set("Red").To("E22000");
        Set("Green").To("00D909");
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
        // button
        Set("UI.Button.Default.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.Button.Default.BgFocus").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UIBgPrimary]));
        Set("UI.Button.Default.Text").To(ThemeOptions[DefaultThemeOptions.UITextPrimary]);
        Set("UI.Button.Default.DisabledBg").To(ColorUtils.GrayScale((string)ThemeOptions[DefaultThemeOptions.UIBgPrimary]));
        Set("UI.Button.Default.DisabledText").To(ColorUtils.GrayScale((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        
        Set("UI.Button.Secondary.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.Button.Secondary.BgFocus").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UIBgSecondary]));
        Set("UI.Button.Secondary.Text").To(ThemeOptions[DefaultThemeOptions.UITextSecondary]);
        Set("UI.Button.Secondary.DisabledBg").To(ColorUtils.GrayScale((string)ThemeOptions[DefaultThemeOptions.UIBgSecondary]));
        Set("UI.Button.Secondary.DisabledText").To(ColorUtils.GrayScale((string)ThemeOptions[DefaultThemeOptions.UITextSecondary]));
        
        // text field
        Set("UI.TextField.Default.Text").To(ThemeOptions[DefaultThemeOptions.UITextPrimary]);
        Set("UI.TextField.Default.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.TextField.Default.Border").To(ThemeOptions[DefaultThemeOptions.UIBorderSecondary]);

        // toggle switch
        Set("UI.ToggleSwitch.Default.OnText").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.ToggleSwitch.Default.OnBg").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.ToggleSwitch.Default.OffText").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.ToggleSwitch.Default.OffBg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.ToggleSwitch.Default.BgSecondary").To(ThemeOptions["White"]);

        // checkbox
        Set("UI.Checkbox.Default.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.Checkbox.Default.Text").To(ThemeOptions[DefaultThemeOptions.UITextPrimary]);
        Set("UI.Checkbox.Default.BgFocus").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.Checkbox.Default.Border").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.Checkbox.Default.CheckColor").To(ThemeOptions["White"]);

        // radio button
        Set("UI.RadioButton.Default.Text").To(ThemeOptions[DefaultThemeOptions.UITextPrimary]);
        Set("UI.RadioButton.Default.CheckSize").To(2);

        // window
        Set("UI.Window.Default.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.Window.Default.Header.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgPrimary]);
        Set("UI.Window.Default.Header.BgFocus").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UIBgPrimary]));
        Set("UI.Window.Default.Title.Text").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        Set("UI.Window.Default.CloseBtn.Text").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        Set("UI.Window.Default.MinimizeBtn.Text").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        
        Set("UI.Window.Secondary.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.Window.Secondary.Header.Bg").To(ThemeOptions[DefaultThemeOptions.UIBgSecondary]);
        Set("UI.Window.Secondary.Header.BgFocus").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UIBgSecondary]));
        Set("UI.Window.Secondary.Title.Text").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        Set("UI.Window.Secondary.CloseBtn.Text").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        Set("UI.Window.Secondary.MinimizeBtn.Text").To(ColorUtils.Lighten((string)ThemeOptions[DefaultThemeOptions.UITextPrimary]));
        
        // alert
        Set("UI.Alert.Default.Text").To(ThemeOptions[DefaultThemeOptions.UITextPrimary]);
        Set("UI.Alert.Default.BgSecondary").To(ThemeOptions["White"]);
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
