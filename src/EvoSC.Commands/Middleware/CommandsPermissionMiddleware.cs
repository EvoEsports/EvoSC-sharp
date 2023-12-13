using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;
using EvoSC.Common.Util.ServerUtils;

namespace EvoSC.Commands.Middleware;

public class CommandsPermissionMiddleware(ActionDelegate next, IPermissionManager permissions, IServerClient server)
{
    public async Task ExecuteAsync(IControllerContext context)
    {
        var cmdContext = context as CommandInteractionContext;

        if (cmdContext == null)
        {
            // not a command interaction
            await next(context);
            return;
        }

        if (cmdContext.CommandExecuted.Permission == null ||
            await permissions.HasPermissionAsync(cmdContext.Player, cmdContext.CommandExecuted.Permission))
        {
            await next(context);
            return;
        }

        await server.SendChatMessageAsync("Insufficient permissions to run this command.", cmdContext.Player);
    }
}
