using EvoSC.Common.Util.MatchSettings;
using EvoSC.Common.Util.MatchSettings.Builders;
using Xunit;

namespace EvoSC.Common.Tests.Util.MatchSettings;

public class MatchSettingsBuilderTests
{
    [Theory]
    [InlineData(DefaultModeScriptName.TimeAttack, "Trackmania/TM_TimeAttack_Online.Script.txt")]
    public void Set_Default_Mode_Script_Name(DefaultModeScriptName name, string expected)
    {
        var builder = new MatchSettingsBuilder();
        builder.WithMode(name);

        var ms = builder.Build();
        
        Assert.Equal(expected, ms.GameInfos.ScriptName);
    }
}
