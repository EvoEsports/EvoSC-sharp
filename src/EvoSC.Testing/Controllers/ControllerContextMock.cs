using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using NSubstitute;

namespace EvoSC.Testing.Controllers;

public class ControllerContextMock<TContext> where TContext : class, IControllerContext
{
    private IContextService _contextService;
    private TContext _context;
    private IAuditService _auditService;
    private IAuditEventBuilder _auditEventBuilder;
    
    /// <summary>
    /// The context mock.
    /// </summary>
    public TContext Context => _context;
    
    /// <summary>
    /// The context service mock.
    /// </summary>
    public IContextService ContextService => _contextService;
    
    /// <summary>
    /// The audit service mock.
    /// </summary>
    public IAuditService AuditService => _auditService;

    /// <summary>
    /// The audit event builder mock.
    /// </summary>
    public IAuditEventBuilder AuditEventBuilder => _auditEventBuilder;

    public ControllerContextMock()
    {
        _auditService = Substitute.For<IAuditService>();
        _auditEventBuilder = Mocking.NewAuditEventBuilderMock();
        
        _context = Substitute.For<TContext>();
        _context.AuditEvent.Returns(_auditEventBuilder);

        _contextService = Mocking.NewContextServiceMock(Context, null);
    }
}
