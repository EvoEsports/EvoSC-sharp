using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;

namespace EvoSC.Manialinks.Middleware;

/// <summary>
/// This middleware checks if a user has the permission to execute a Manialink Action.
/// A message in chat is sent to the player in case of insufficient permissions.
/// </summary>
public class ManialinkPermissionMiddleware
{
    private readonly ActionDelegate _next;
    private readonly IPermissionManager _permissions;
    private readonly IServerClient _server;
    
    public ManialinkPermissionMiddleware(ActionDelegate next, IPermissionManager permissions, IServerClient server)
    {
        _next = next;
        _permissions = permissions;
        _server = server;
    }

    public async Task ExecuteAsync(IControllerContext context)
    {
        if (context is not ManialinkInteractionContext mlContext || mlContext.ManialinkAction.Action.Permission == null)
        {
            await _next(context);
            return;
        }

        if (await _permissions.HasPermissionAsync(mlContext.Player, mlContext.ManialinkAction.Action.Permission))
        {
            await _next(context);
            return;
        }

        await _server.ErrorMessageAsync("Sorry, you don't have permission to do that.", mlContext.Player);
    }
}
