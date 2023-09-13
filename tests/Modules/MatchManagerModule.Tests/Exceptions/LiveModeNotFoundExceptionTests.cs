using EvoSC.Modules.Official.MatchManagerModule.Exceptions;

namespace MatchManagerModule.Tests.Exceptions;

public class LiveModeNotFoundExceptionTests
{
    [Fact]
    public void Exception_Message_Is_Correctly_Set()
    {
        var ex = new LiveModeNotFoundException("MyName");
        
        Assert.Equal("The mode 'MyName' was not found.", ex.Message);
    }
}
