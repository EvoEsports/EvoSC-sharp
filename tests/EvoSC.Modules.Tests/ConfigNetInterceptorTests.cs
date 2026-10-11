using EvoSC.Modules.Util;

namespace EvoSC.Modules.Tests;

public class ConfigNetInterceptorTests
{
    [Fact]
    public void VerifyCompatibility_Succeeds_Against_Referenced_ConfigNet()
    {
        // ConfigNetInterceptor reaches into Config.Net's internals by reflection. This locks in that
        // the referenced Config.Net version still has the types, constructors and fields it expects,
        // so an incompatible upgrade is caught here instead of at a module's first settings access.
        var exception = Record.Exception(ConfigNetInterceptor.VerifyCompatibility);

        Assert.Null(exception);
    }
}
