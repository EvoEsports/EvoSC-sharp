using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.Auditing;
using SimpleInjector;

namespace EvoSC.Common.Services;

public class ContextService : IContextService
{
    private IControllerContext? _context;
    private readonly IAuditService _auditService;

    public ContextService(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public IControllerContext CreateContext(Scope scope, IController controller)
    {
        IControllerContext context = new GenericControllerContext(scope)
        {
            Controller = controller,
            AuditEvent = new AuditEventBuilder(_auditService)
        };
        
        context.SetScope(scope);

        _context = context;
        return context;
    }

    public IControllerContext GetContext()
    {
        if (_context == null)
        {
            throw new InvalidOperationException("Context is not created yet.");
        }

        return _context;
    }

    public AuditEventBuilder Audit() => GetContext().AuditEvent;
}
