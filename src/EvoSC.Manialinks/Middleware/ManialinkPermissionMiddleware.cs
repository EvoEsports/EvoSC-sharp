﻿using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;

namespace EvoSC.Manialinks.Middleware;

/// <summary>
/// This middleware checks if a user has the permission to execute a Manialink Action.
/// A message in chat is sent to the player in case of insufficient permissions.
/// </summary>
public class ManialinkPermissionMiddleware(ActionDelegate next, IPermissionManager permissions, IChatService chat)
{
    public async Task ExecuteAsync(IControllerContext context)
    {
        if (context is not ManialinkInteractionContext mlContext || mlContext.ManialinkAction.Action.Permission == null)
        {
            await next(context);
            return;
        }

        if (await permissions.HasPermissionAsync(mlContext.Player, mlContext.ManialinkAction.Action.Permission))
        {
            await next(context);
            return;
        }

        await chat.ErrorMessageAsync("Sorry, you don't have permission to do that.", mlContext.Player);
    }
}
