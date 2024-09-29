using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Themes
{
    [Theme(Name = "Tournament Setup View", Description = "Default theme for the Tournament Setup View.")]
    public class DefaultTournamentSetupViewTheme(IThemeManager theme) : Theme<DefaultTournamentSetupViewTheme>
    {
        private readonly dynamic _theme = theme.Theme;

        public override Task ConfigureAsync()
        {

            return Task.CompletedTask;
        }
    }
}
