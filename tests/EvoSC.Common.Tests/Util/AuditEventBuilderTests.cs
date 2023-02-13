using EvoSC.Common.Util.Auditing;
using Xunit;

namespace EvoSC.Common.Tests.Util;

public class AuditEventBuilderTests
{
    public AuditEventBuilderTests()
    {
        
    }

    [Fact]
    public void No_Event_Name_Constructor_Starts_Has_Correct_Defaults()
    {
        var builder = new AuditEventBuilder(null);
    }
}
