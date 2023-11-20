using System.Collections.Generic;
using EvoSC.Common.Themes;
using Xunit;

namespace EvoSC.Common.Tests.Themes;

public class DynamicThemeOptionsTests
{
    [Fact]
    public void Can_Access_Dict_Items_By_Member_Access()
    {
        var dict = new Dictionary<string, object> { { "MyOption", "MyValue" } };
        var options = new DynamicThemeOptions(dict);
        
        dynamic dynamicOptions = options;
        
        Assert.NotNull(dynamicOptions.MyOption);
        Assert.Equal("MyValue", dynamicOptions.MyOption);
    }
    
    [Fact]
    public void Can_Access_Multilevel_Options()
    {
        var dict = new Dictionary<string, object> { { "MyOptions.MyOption1", "MyValue" } };
        var options = new DynamicThemeOptions(dict);
        
        dynamic dynamicOptions = options;
        
        Assert.Equal("MyValue", dynamicOptions.MyOptions_MyOption1);
    }
}
