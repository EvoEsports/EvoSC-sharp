using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;
using EvoSC.Common.Util.ServerUtils;

namespace EvoSC.Commands.Middleware;

public class PermissionMiddleware
{
    private readonly ActionDelegate _next;
    private readonly IPermissionManager _permissions;
    private readonly IServerClient _server;

    public PermissionMiddleware(ActionDelegate next, IPermissionManager permissions, IServerClient server)
    {
        _next = next;
        _permissions = permissions;
        _server = server;
    }

    public async Task ExecuteAsync(IControllerContext context)
    {
        var cmdContext = context as CommandInteractionContext;

        if (cmdContext == null)
        {
            // not a command interaction
            await _next(context);
            return;
        }

        if (cmdContext.CommandExecuted.Permission == null ||
            await _permissions.HasPermissionAsync(cmdContext.Player, cmdContext.CommandExecuted.Permission))
        {
            await _next(context);
            return;
        }

        await _server.SendChatMessageAsync("Insufficient permissions to run this command.", cmdContext.Player);
    }
}
