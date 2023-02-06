using System.Reflection;
using EvoSC.Common.Util.MatchSettings.Attributes;
using Xunit;

namespace EvoSC.Common.Tests.Util.MatchSettings;

public class AttributeTests
{
    [Fact]
    public void Default_Value_For_Custom_Mode()
    {
        var attr = new DefaultScriptSettingValueAttribute("MyMode", 123);
        
        Assert.Equal("MyMode", attr.OnMode);
        Assert.Equal(123, attr.Value);
    }

    [Fact]
    public void Custom_Script_Name_For_ScriptSettingsForAttribute()
    {
        var attr = new ScriptSettingsForAttribute("My_Custom_Mode");
        
        Assert.Equal("My_Custom_Mode", attr.Name);
    }
}
