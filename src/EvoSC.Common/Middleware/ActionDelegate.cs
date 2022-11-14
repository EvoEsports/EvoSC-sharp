using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Common.Middleware;

public delegate Task ActionDelegate(IControllerContext context);
