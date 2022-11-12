using EvoSC.Common.Util;
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
}
