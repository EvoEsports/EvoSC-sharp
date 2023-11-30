using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.SetName.Themes;

[Theme(Name = "Set Name", Description = "Default theme for the Set Name module.")]
public class DefaultSetNameTheme : Theme<DefaultSetNameTheme>
{
    public DefaultSetNameTheme() 
    {
        
    }
    
    public override Task ConfigureAsync()
    {
        return Task.CompletedTask;
    }
}
