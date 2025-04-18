﻿using EvoSC.Commands.Exceptions;
using EvoSC.Commands.Interfaces;
using EvoSC.Commands.Parser;
using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Middleware;
using EvoSC.Common.Remote.ChatRouter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EvoSC.Commands.Middleware;

public class CommandsMiddleware(ActionDelegate next, ILogger<CommandsMiddleware> logger,
    IChatCommandManager cmdManager, IControllerManager controllers, IServerClient serverClient,
    IActionPipelineManager actionPipeline)
{
    private readonly ChatCommandParser _parser = new(cmdManager);

    async Task HandleUserErrorsAsync(IParserResult result, IPlayer player)
    {
        if (result.Exception is CommandParserException cmdParserException)
        {
            if (!cmdParserException.IntendedCommand)
            {
                return;
            }

            var message = $"Error: {cmdParserException.Message}";
            await serverClient.Chat.ErrorMessageAsync($"Error: {message}", player);
        }

        if (result.Exception is PlayerNotFoundException playerNotFoundException)
        {
            await serverClient.Chat.ErrorMessageAsync($"Error: {playerNotFoundException.Message}", player);
        }
        else
        {
            logger.LogError(result.Exception, "Failed to parse command");
        }
    }

    private async Task ExecuteCommandAsync(IChatCommand cmd, object[] args, ChatRouterPipelineContext routerContext)
    {
        var (controller, context) = controllers.CreateInstance(cmd.ControllerType);

        if (context.ServiceScope.Container == null)
        {
            throw new InvalidOperationException("Failed to instantiate controller context as the service container is null");
        }
        
        var contextServerClient = context.ServiceScope.Container.GetRequiredService<IServerClient>();

        var playerInteractionContext = new CommandInteractionContext((IOnlinePlayer)routerContext.Author, contextServerClient, context)
        {
            CommandExecuted = cmd
        };
        
        controller.SetContext(playerInteractionContext);
        var contextService = context.ServiceScope.GetInstance<IContextService>();
        contextService.UpdateContext(playerInteractionContext);

        var actionChain = actionPipeline.BuildChain(PipelineType.ControllerAction, _ =>
            (Task?)cmd.HandlerMethod.Invoke(controller, args) ?? Task.CompletedTask
        );

        try
        {
            await actionChain(playerInteractionContext);
        }
        finally
        {
            if (context.AuditEvent is { IsCanceled: false, Activated: true })
            {
                // allow actor to be manually set, so avoid overwrite
                if (context.AuditEvent.Actor == null)
                {
                    context.AuditEvent.CausedBy(playerInteractionContext.Player);
                }

                await context.AuditEvent.LogAsync();
            }
            else if (!context.AuditEvent.IsCanceled && cmd.Permission != null)
            {
                logger.LogWarning("Command '{Name}' has permissions set but does not activate an audit", cmd.Name);
            }
        }
    }

    private static void CheckAliasHiding(ChatRouterPipelineContext context, IParserResult parserResult)
    {
        if (parserResult.IsIntended)
        {
            return;
        }

        if (parserResult.Command.Aliases.TryGetValue(parserResult.AliasUsed, out var alias) && alias.Hide)
        {
            context.ForwardMessage = false;
        }
    }
    
    public async Task ExecuteAsync(ChatRouterPipelineContext context)
    {
        if (context.MessageText.Trim().StartsWith(ChatCommandParser.CommandPrefix, StringComparison.Ordinal))
        {
            context.ForwardMessage = false;
        }
        
        try
        {
            var parserResult = await _parser.ParseAsync(context.MessageText);

            if (parserResult.Success)
            {
                await ExecuteCommandAsync(parserResult.Command, parserResult.Arguments.ToArray(), context);
                CheckAliasHiding(context, parserResult);
            }
            else if (parserResult.Exception != null)
            {
                await HandleUserErrorsAsync(parserResult, context.Author);
            }
            else
            {
                logger.LogError(
                    "An unknown error occured while trying to parse command. No exception thrown, but parsing failed");
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "A fatal error occured while trying to handle chat command");
            throw;
        }
        
        await next(context);
    }
}
