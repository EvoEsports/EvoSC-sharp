using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using Moq;

namespace EvoSC.Testing.Controllers;

public class ControllerContextMock<TContext> where TContext : class, IControllerContext
{
    private Mock<IContextService> _contextService;
    private Mock<TContext> _context;
    private Mock<IAuditService> _auditService;
    private Mock<IAuditEventBuilder> _auditEventBuilder;
    
    /// <summary>
    /// The context mock.
    /// </summary>
    public Mock<TContext> Context => _context;
    
    /// <summary>
    /// The context service mock.
    /// </summary>
    public Mock<IContextService> ContextService => _contextService;
    
    /// <summary>
    /// The audit service mock.
    /// </summary>
    public Mock<IAuditService> AuditService => _auditService;

    /// <summary>
    /// The audit event builder mock.
    /// </summary>
    public Mock<IAuditEventBuilder> AuditEventBuilder => _auditEventBuilder;

    public ControllerContextMock()
    {
        _auditService = new Mock<IAuditService>();
        _auditEventBuilder = Mocking.NewAuditEventBuilderMock();
        
        _context = new Mock<TContext>();
        _context.Setup(c => c.AuditEvent).Returns(_auditEventBuilder.Object);

        _contextService = Mocking.NewContextServiceMock(Context.Object, null);
    }
}
