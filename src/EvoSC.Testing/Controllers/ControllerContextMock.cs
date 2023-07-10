using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.Auditing;
using Moq;

namespace EvoSC.Testing.Controllers;

public class ControllerContextMock<TContext> where TContext : class, IControllerContext
{
    private Mock<IContextService> _contextService;
    private Mock<TContext> _context;
    private Mock<IAuditService> _auditService;
    private AuditEventBuilder _auditEventBuilder;
    
    public Mock<TContext> Context => _context;
    public Mock<IAuditService> AuditService => _auditService;
    public Mock<IContextService> ContextService => _contextService;

    public ControllerContextMock()
    {
        _auditService = new Mock<IAuditService>();
        
        _auditEventBuilder = new AuditEventBuilder(_auditService.Object);
        
        _context = new Mock<TContext>();
        _context.Setup(c => c.AuditEvent).Returns(_auditEventBuilder);

        _contextService = new Mock<IContextService>();
        _contextService.Setup(m => m.GetContext()).Returns(_context.Object);
        _contextService.Setup(m => m.Audit()).Returns(_auditEventBuilder);
    }
}
