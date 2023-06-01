using System.ComponentModel.DataAnnotations;
using System.Reflection;
using EvoSC.Common.Exceptions.Parsing;
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
using EvoSC.Manialinks.Models;
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
        IControllerManager controllers, IPlayerManagerService players,
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
            .WithHandlerMethod<ManiaLinkPageActionGbxEventArgs>(HandleManialinkPageAnswerAsync)
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

    private async Task HandleManialinkPageAnswerAsync(object? sender, ManiaLinkPageActionGbxEventArgs args)
    {
        try
        {
            var (action, path) = _manialinkActionManager.FindAction(args.Answer);

            var (controller, context) = _controllers.CreateInstance(action.ControllerType);

            if (context.ServiceScope.Container == null)
            {
                throw new InvalidOperationException("Service scope container is null.");
            }
            
            var player = await GetPlayerAsync(args.Login);
            var manialinkManager = context.ServiceScope.Container.GetRequiredService<IManialinkManager>();

            var (actionParams, entryModel, validationResults) =
                await ConvertRequestParametersAsync(action.FirstParameter, path, args.Entries,
                    context.ServiceScope.Container);

            var manialinkInteractionContext = new ManialinkInteractionContext(player, context)
            {
                ManialinkAction = new ManialinkActionContext {Action = action, EntryModel = entryModel},
                ManialinkManager = manialinkManager
            };

            controller.SetContext(manialinkInteractionContext);

            if (controller is ManialinkController manialinkController)
            {
                manialinkController.AddEarlyValidationResults(validationResults);
                await manialinkController.ValidateModelAsync();
            }

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

    private async Task<IOnlinePlayer> GetPlayerAsync(string login)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(login);

        try
        {
            return await _players.GetOnlinePlayerAsync(accountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get player during Manialink action");
            throw;
        }
    }
    
    /// <summary>
    /// Parse action parameters and create a list of values that can be used as arguments to the action method.
    /// </summary>
    /// <param name="currentParam">First parameter in the parameter list.</param>
    /// <param name="currentNode">First node in the route components list.</param>
    /// <param name="entries">Form entry input from the user.</param>
    /// <param name="services">Services to be used for entry model instantiation.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if action parameters are missing.</exception>
    private async Task<(object[] values, object? entryModel, IEnumerable<ValidationResult> validationResults)> ConvertRequestParametersAsync(IMlActionParameter? currentParam, IMlRouteNode? currentNode, TmSEntryVal[] entries, Container services)
    {
        var values = new List<object>();
        object? entryModel = null;
        IEnumerable<ValidationResult> validationResults = new List<ValidationResult>();

        while (currentParam != null)
        {
            // ignores nodes which are not a parameter
            while (currentNode is {IsParameter: false})
            {
                currentNode = currentNode?.Children?.Values.First();
            }
            
            if (currentParam.IsEntryModel)
            {
                // convert entry model, can only be one of them
                if (entryModel != null)
                {
                    throw new InvalidOperationException("Cannot convert more than one Entry model.");
                }

                var (entryModelInstance, modelValidationResults) =
                    await ConvertEntryModelAsync(currentParam.Type, entries, services);

                values.Add(entryModelInstance);
                validationResults = modelValidationResults;
                entryModel = entryModelInstance;
            }
            else if (currentNode == null)
            {
                throw new InvalidOperationException("Missing parameters for manialink action.");
            }
            else
            {
                // convert a route parameter
                values.Add(await _valueReader.ConvertValueAsync(currentParam.Type, currentNode.Name));
                currentNode = currentNode?.Children?.Values.First();
            }

            currentParam = currentParam.NextParameter;
        }

        return (values: values.ToArray(), entryModel: entryModel, validationResults: validationResults);
    }

    /// <summary>
    /// Instantiates the entry model and fills in the input data to it. It also performs basic validation
    /// such as checking for correct data type.
    /// </summary>
    /// <param name="type">The type of the entry model.</param>
    /// <param name="entries">Input data from the user.</param>
    /// <param name="services">Service container to be used for entry model instantiation.</param>
    /// <returns></returns>
    private async Task<(object, IEnumerable<ValidationResult>)> ConvertEntryModelAsync(Type type, TmSEntryVal[] entries, Container services)
    {
        var instance = ActivatorUtilities.CreateInstance(services, type);
        var modelProperties =
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        var validationResults = new List<ValidationResult>();

        // build entry dict for faster lookup
        var entriesDict = new Dictionary<string, TmSEntryVal>();
        foreach (var entry in entries)
        {
            entriesDict[entry.Name] = entry;
        }

        foreach (var modelProperty in modelProperties)
        {
            var name = modelProperty.Name;
            object? value = null;
            var entryExists = entriesDict.TryGetValue(name, out var entry);

            if (entryExists)
            {
                try
                {
                    value = await _valueReader.ConvertValueAsync(modelProperty.PropertyType, entry.Value);
                    modelProperty.SetValue(instance, value);
                }
                catch (Exception ex) when (ex is ValueConversionException or FormatException)
                {
                    validationResults.Add(new ValidationResult($"Wrong format, must be {modelProperty.PropertyType.Name}", new[] {name}));
                    _logger.LogDebug(ex, "Failed to convert entry value for property {Prop} in model {Model}",
                        modelProperty.Name, type.Name);
                }
            }
        }

        return (instance, validationResults);
    }
}
