using EvoSC.Common.Models.Audit;
using EvoSC.Common.Models.Players;
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
        
        Assert.Equal(AuditEventStatus.Info, builder.Status);
        Assert.True(builder.IsCanceled);
        Assert.False(builder.Activated);
    }
    
    [Fact]
    public void Event_Name_Constructor_Starts_Has_Correct_Defaults()
    {
        var builder = new AuditEventBuilder(null, "MyEvent");
        
        Assert.Equal(AuditEventStatus.Info, builder.Status);
        Assert.Equal("MyEvent", builder.EventName);
        Assert.False(builder.IsCanceled);
        Assert.True(builder.Activated);
    }

    [Fact]
    public void WithEventName_Sets_Correctly()
    {
        var builder = new AuditEventBuilder(null);

        builder.WithEventName("MyEvent");
        
        Assert.Equal("MyEvent", builder.EventName);
    }

    enum MyAuditNames
    {
        MyAuditEvent
    }
    
    [Fact]
    public void WithEventName_Sets_Correctly_With_Enum_Identifier()
    {
        var builder = new AuditEventBuilder(null);

        builder.WithEventName(MyAuditNames.MyAuditEvent);
        
        Assert.Equal("MyAuditEvent", builder.EventName);
    }

    [Fact]
    public void Audit_Event_Properties_Properly_Set()
    {
        var myProps = new {MyProp = 123};
        var builder = new AuditEventBuilder(null);

        builder.HavingProperties(myProps);

        Assert.NotNull(builder.Properties);
        Assert.Equal(myProps.MyProp, builder.Properties.MyProp);
    }

    [Theory]
    [InlineData(AuditEventStatus.Info)]
    [InlineData(AuditEventStatus.Error)]
    [InlineData(AuditEventStatus.Success)]
    public void Audit_Event_Status_Properly_Set(AuditEventStatus status)
    {
        var builder = new AuditEventBuilder(null);

        builder.WithStatus(status);
        
        Assert.Equal(status, builder.Status);
    }

    [Fact]
    public void Success_Event_Set()
    {
        var builder = new AuditEventBuilder(null);

        builder.Success();
        
        Assert.Equal(AuditEventStatus.Success, builder.Status);
    }
    
    [Fact]
    public void Info_Event_Set()
    {
        var builder = new AuditEventBuilder(null);

        builder.Info();
        
        Assert.Equal(AuditEventStatus.Info, builder.Status);
    }
    
    [Fact]
    public void Error_Event_Set()
    {
        var builder = new AuditEventBuilder(null);

        builder.Error();
        
        Assert.Equal(AuditEventStatus.Error, builder.Status);
    }

    [Fact]
    public void Event_Actor_Properly_Set()
    {
        var actor = new Player {NickName = "MyPlayer"};
        var builder = new AuditEventBuilder(null);

        builder.CausedBy(actor);
        
        Assert.NotNull(builder.Actor);
        Assert.Equal("MyPlayer", builder.Actor.NickName);
    }

    [Fact]
    public void Audit_Event_Canceled()
    {
        var builder = new AuditEventBuilder(null, "MyEvent");

        builder.Cancel();
        
        Assert.True(builder.IsCanceled);
    }
    
    [Fact]
    public void Audit_Event_UnCanceled()
    {
        var builder = new AuditEventBuilder(null);

        builder.Cancel();
        builder.UnCancel();
        
        Assert.False(builder.IsCanceled);
    }

    [Fact]
    public void Audit_Event_Comment_Added()
    {
        var builder = new AuditEventBuilder(null);

        builder.Comment("My Comment.");
        
        Assert.NotNull(builder.EventComment);
        Assert.Equal("My Comment.", builder.EventComment);
    }
}
