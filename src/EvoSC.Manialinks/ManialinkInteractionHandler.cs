using System.Reflection;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Middleware;
using EvoSC.Common.Remote;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using GbxRemoteNet.Events;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using StringReader = EvoSC.Common.TextParsing.ValueReaders.StringReader;

namespace EvoSC.Manialinks;

public class ManialinkInteractionHandler : IManialinkInteractionHandler
{
    private readonly IManialinkActionManager _manialinkActionManager;
    private readonly ILogger<ManialinkInteractionHandler> _logger;
    private readonly IPlayerManagerService _players;
    private readonly IControllerManager _controllers;
    private readonly IActionPipelineManager _actionPipeline;
    private readonly ValueReaderManager _valueReader = new();

    public ManialinkInteractionHandler(IEventManager events, IManialinkActionManager manialinkActionManager,
        ILogger<ManialinkInteractionHandler> logger, IPlayerManagerService playerManager, 
        IServiceContainerManager serviceManager, IControllerManager controllers, IPlayerManagerService players,
        IActionPipelineManager actionPipeline)
    {
        _manialinkActionManager = manialinkActionManager;
        _logger = logger;
        _controllers = controllers;
        _players = players;
        _actionPipeline = actionPipeline;

        SetupValueReader(playerManager);
        
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.ManialinkPageAnswer)
            .WithInstance(this)
            .WithInstanceClass<ManialinkInteractionContext>()
            .WithHandlerMethod<ManiaLinkPageActionEventArgs>(HandleManialinkPageAnswerAsync)
            .AsAsync()
        );
    }

    private void SetupValueReader(IPlayerManagerService playerManager)
    {
        _valueReader.AddReader(new BooleanReader());
        _valueReader.AddReader(new FloatReader());
        _valueReader.AddReader(new IntegerReader());
        _valueReader.AddReader(new StringReader());
        _valueReader.AddReader(new OnlinePlayerReader(playerManager));
    }

    private async Task HandleManialinkPageAnswerAsync(object? sender, ManiaLinkPageActionEventArgs args)
    {
        try
        {
            var (action, path) = _manialinkActionManager.FindAction(args.Answer);

            var (controller, context) = _controllers.CreateInstance(action.ControllerType);
            var player = await GetPlayerAsync(args.Login);
            var manialinkInteractionContext = new ManialinkInteractionContext(player, context)
            {
                ManialinkActionExecuted = action
            };

            controller.SetContext(manialinkInteractionContext);

            var actionParams =
                await ConvertRequestParametersAsync(action.FirstParameter, path, args.Entries,
                    context.ServiceScope.Container);

            var actionChain = _actionPipeline.BuildChain(PipelineType.ControllerAction, _ =>
                (Task?)action.HandlerMethod.Invoke(controller, actionParams) ?? Task.CompletedTask
            );

            try
            {
                await actionChain(manialinkInteractionContext);
            }
            finally
            {
                if (context.AuditEvent.Activated)
                {
                    // allow actor to be manually set, so avoid overwrite
                    if (context.AuditEvent.Actor == null)
                    {
                        context.AuditEvent.CausedBy(manialinkInteractionContext.Player);
                    }

                    await context.AuditEvent.LogAsync();
                }
                else if (action.Permission != null)
                {
                    _logger.LogWarning("Command '{Name}' has permissions set but does not activate an audit",
                        action.HandlerMethod.Name);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "No action found for route '{Route}'", args.Answer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call manialink action for route '{Route}'", args.Answer);
        }
    }

    public async Task<IOnlinePlayer> GetPlayerAsync(string login)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(login);

        try
        {
            return await _players.GetOnlinePlayerAsync(accountId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get player during Manialink action");
            throw;
        }
    }
    
    private async Task<object[]> ConvertRequestParametersAsync(IMlActionParameter? currentParam, IMlRouteNode? currentNode, TmSEntryVal[] entries, Container services)
    {
        var values = new List<object>();

        while (currentParam != null)
        {
            while (currentNode is {IsParameter: false})
            {
                currentNode = currentNode?.Children?.Values.First();
            }
            
            if (currentParam.IsEntryModel)
            {
                values.Add(await ConvertEntryModel(currentParam.Type, entries, services));
            }
            else if (currentNode == null)
            {
                throw new InvalidOperationException("Missing parameters for manialink action.");
            }
            else
            {
                values.Add(await _valueReader.ConvertValueAsync(currentParam.Type, currentNode.Name));
                currentNode = currentNode?.Children?.Values.First(); 
            }

            currentParam = currentParam.NextParameter;
        }

        return values.ToArray();
    }

    private async Task<object> ConvertEntryModel(Type type, TmSEntryVal[] entries, Container services)
    {
        var instance = ActivatorUtilities.CreateInstance(services, type);
        var modelProperties =
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

        // build entry dict for faster lookup
        var entriesDict = new Dictionary<string, TmSEntryVal>();
        foreach (var entry in entries)
        {
            entriesDict[entry.Name] = entry;
        }
        
        foreach (var modelProperty in modelProperties)
        {
            var name = modelProperty.Name;

            if (entriesDict.TryGetValue(name, out var entry))
            {
                var value = await _valueReader.ConvertValueAsync(modelProperty.PropertyType, entry.Value);
                modelProperty.SetValue(instance, value);
            }
        }

        return instance;
    }
}
