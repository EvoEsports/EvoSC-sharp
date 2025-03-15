using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.MapListModule.Themes;

[Theme(Name = "Map List", Description = "Default theme for the Map List.")]
public class DefaultMapListTheme: Theme<DefaultMapListTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {

        return Task.CompletedTask;
    }
}
