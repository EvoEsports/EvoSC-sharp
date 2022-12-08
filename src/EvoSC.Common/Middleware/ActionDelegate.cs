using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;

namespace EvoSC.Common.Middleware;

/// <summary>
/// Invocation delegate for middlewares actions.
/// </summary>
/// <typeparam name="TContext"></typeparam>
public delegate Task ActionDelegate(IPipelineContext context);
