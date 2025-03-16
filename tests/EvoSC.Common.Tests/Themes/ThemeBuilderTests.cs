using System.Threading.Tasks;
using EvoSC.Common.Themes;
using Xunit;

namespace EvoSC.Common.Tests.Themes;

public class ThemeBuilderTests
{
    public class ThemeSetOption : Theme<ThemeSetOption>
    {
        public override Task ConfigureAsync(dynamic theme)
        {
            Set("MyOption").To("My Value");

            return Task.CompletedTask;
        }
    }
    
    public class ThemeReplaceComponent : Theme<ThemeReplaceComponent>
    {
        public override Task ConfigureAsync(dynamic theme)
        {
            Replace("MyComponent").With("OtherComponent");

            return Task.CompletedTask;
        }
    }
    
    [Fact]
    public async Task Theme_Option_Is_Added()
    {
        var theme = new ThemeSetOption();

        await theme.ConfigureAsync(null);
        
        Assert.True(theme.ThemeOptions.ContainsKey("MyOption"));
        Assert.Equal("My Value", theme.ThemeOptions["MyOption"]);
    }

    [Fact]
    public async Task Component_Replacement_Is_Added()
    {
        var theme = new ThemeReplaceComponent();

        await theme.ConfigureAsync(null);
        
        Assert.True(theme.ComponentReplacements.ContainsKey("MyComponent"));
        Assert.Equal("OtherComponent", theme.ComponentReplacements["MyComponent"]);
    }
}
