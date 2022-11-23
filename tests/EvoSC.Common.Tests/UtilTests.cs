using EvoSC.Common.Util;
using GbxRemoteNet.Structs;
using Xunit;

namespace EvoSC.Common.Tests;

public class UtilTests
{
    [Fact]
    public void Player_Account_Id_Converted_To_Login()
    {
        var login = PlayerUtils.ConvertAccountIdToLogin("a467a996-eba5-44bf-9e2b-8543b50f39ae");
        
        Assert.Equal("pGepluulRL-eK4VDtQ85rg", login);
    }

    [Fact]
    public void Player_Login_Converted_To_Account_Id()
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId("pGepluulRL-eK4VDtQ85rg");
        
        Assert.Equal("a467a996-eba5-44bf-9e2b-8543b50f39ae", accountId);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    [InlineData(2, true)]
    public void Player_ForcedSpectator_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).IsForcedSpectator();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(2, true)]
    [InlineData(1, false)]
    [InlineData(0, false)]
    public void Player_ForcedSpectatorSelectable_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).IsForcedSpectatorSelectable();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(1000, 1)]
    [InlineData(0, 0)]
    [InlineData(2000, 2)]
    public void Player_StereoDisplayMode_Parsed(int flags, int actual)
    {
        int result = (new TmPlayerInfo {Flags = flags}).StereoDisplayMode();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(10000, true)]
    [InlineData(0, false)]
    public void Player_IsManagedByAnOtherServer_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).IsManagedByAnOtherServer();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(100000, true)]
    [InlineData(0, false)]
    public void Player_IsServer_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).IsServer();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(1000000, true)]
    [InlineData(0, false)]
    public void Player_HasPlayerSlot_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).HasPlayerSlot();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(10000000, true)]
    [InlineData(0, false)]
    public void Player_IsBroadcasting_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).IsBroadcasting();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData(100000000, true)]
    [InlineData(0, false)]
    public void Player_HasJoinedGame_Parsed(int flags, bool actual)
    {
        bool result = (new TmPlayerInfo {Flags = flags}).HasJoinedGame();
        
        Assert.Equal(actual, result);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("a normal string", "a normal string")]
    [InlineData("$f0Fcolor", "color")]
    [InlineData("$obold", "bold")]
    [InlineData("$o$fffcomb$gined", "combined")]
    [InlineData("$l[http://google.com]a link", "a link")]
    public void Trackmania_Text_Formatting_Cleaned(string text, string expected)
    {
        var result = FormattingUtils.CleanTmFormatting(text);
        
        Assert.Equal(expected, result);
    }
}
