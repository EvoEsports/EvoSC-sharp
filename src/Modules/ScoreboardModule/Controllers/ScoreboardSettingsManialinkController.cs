using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Manialinks;

namespace EvoSC.Modules.Official.ScoreboardModule.Controllers;

[Controller]
public class ScoreboardSettingsManialinkController(Locale locale) : ManialinkController
{
    private readonly dynamic _locale = locale;

    public async Task ShowSettingsAsync()
    {
        await ShowAsync(Context.Player, "ScoreboardModule.SettingsWindow",
            new { Locale = _locale });
    }
}
