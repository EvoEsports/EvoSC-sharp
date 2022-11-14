using EvoSC.Common.Interfaces.Controllers;

namespace EvoSC.Common.Middleware;

/// <summary>
/// Invocation delegate for middlewares actions.
/// </summary>
public delegate Task ActionDelegate(IControllerContext context);
