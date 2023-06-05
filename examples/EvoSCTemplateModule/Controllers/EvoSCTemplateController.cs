using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Controllers.Context;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.EvoSCTemplateModule.Controllers;

[Controller]
public class EvoSCTemplateController : EvoScController<EventControllerContext>
{
    private readonly ILogger<EvoSCTemplateController> _logger;
    
    // You want to dependency inject the needed services here at the constructor
    public EvoSCTemplateController(ILogger<EvoSCTemplateController> logger)
    {
        _logger = logger;
    } 
    
    
    
    
    
    
}
