using System.ComponentModel;
using System.Reflection;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class ChatCommandBuilder
{
    private string _name;
    private string _description;
    private string? _permission;
    private readonly Dictionary<string, ICommandAlias> _aliases;
    private Type _controllerType;
    private MethodInfo _handlerMethod;
    private readonly List<ICommandParameter> _parameters;
    private bool _usePrefix;

    /// <summary>
    /// Create a new chat command.
    /// </summary>
    public ChatCommandBuilder()
    {
        _aliases = new Dictionary<string, ICommandAlias>();
        _parameters = new List<ICommandParameter>();
        _usePrefix = true;
    }

    /// <summary>
    /// Modify an existing chat command.
    /// </summary>
    /// <param name="cmd">The existing command to modify.</param>
    public ChatCommandBuilder(IChatCommand cmd)
    {
        _name = cmd.Name;
        _description = cmd.Description;
        _permission = cmd.Permission;
        _aliases = cmd.Aliases;
        _parameters = cmd.Parameters.ToList();
    }

    /// <summary>
    /// Set the name of the command.
    /// </summary>
    /// <param name="name">Name to set.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    /// <summary>
    /// Set the description of this command.
    /// </summary>
    /// <param name="desc">Description to set.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithDescription(string desc)
    {
        _description = desc;
        return this;
    }

    /// <summary>
    /// Set the permission required to execute this command.
    /// </summary>
    /// <param name="perm">Name of the permission.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithPermission(string perm)
    {
        _permission = perm;
        return this;
    }

    /// <summary>
    /// Add an alias that can trigger this command.
    /// </summary>
    /// <param name="alias">Info about the alias.</param>
    /// <returns></returns>
    public ChatCommandBuilder AddAlias(ICommandAlias alias)
    {
        _aliases.Add(alias.Name, alias);
        return this;
    }

    /// <summary>
    /// Add an alias that can trigger this command.
    /// </summary>
    /// <param name="name">Name of the alias.</param>
    /// <param name="hide">Whether to hide the chat message for this alias.</param>
    /// <param name="args">Default arguments to pass to the command.</param>
    /// <returns></returns>
    public ChatCommandBuilder AddAlias(string name, bool hide, params object[] args) => AddAlias(new CommandAlias(name, hide, args));

    /// <summary>
    /// Add an alias that can trigger this command.
    /// </summary>
    /// <param name="name">Name of the alias.</param>
    /// <param name="args">Default arguments to pass to the command</param>
    /// <returns></returns>
    public ChatCommandBuilder AddAlias(string name, params object[] args) => AddAlias(new CommandAlias(name, false, args));

    /// <summary>
    /// Set the controller that contains the action handler for this command.
    /// </summary>
    /// <param name="controllerType">The type of the controller.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithController(Type controllerType)
    {
        _controllerType = controllerType;
        return this;
    }

    /// <summary>
    /// Set the controller that contains the action handler for this command.
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    /// <returns></returns>
    public ChatCommandBuilder WithController<TController>() => WithController(typeof(TController));

    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Method info about the callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod(MethodInfo method)
    {
        _handlerMethod = method;
        return this;
    }

    /// <summary>
    /// Set the callback handler for the command.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod(Action method) => WithHandlerMethod(method.Method);
    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod<T>(Action<T> method) => WithHandlerMethod(method.Method);
    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod<T1, T2>(Action<T1, T2> method) => WithHandlerMethod(method.Method);
    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3>(Action<T1, T2, T3> method) =>
        WithHandlerMethod(method.Method);
    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method) =>
        WithHandlerMethod(method.Method);
    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method) =>
        WithHandlerMethod(method.Method);
    /// <summary>
    /// Set the command's callback handler method.
    /// </summary>
    /// <param name="method">Action to use as callback.</param>
    /// <returns></returns>
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method) =>
        WithHandlerMethod(method.Method);

    /// <summary>
    /// Add a parameter to this command.
    /// </summary>
    /// <param name="parameter">Info about the parameter to add.</param>
    /// <returns></returns>
    public ChatCommandBuilder AddParameter(ICommandParameter parameter)
    {
        _parameters.Add(parameter);
        return this;
    }

    /// <summary>
    /// Add a parameter to this command.
    /// </summary>
    /// <param name="parInfo">Information about the parameter.</param>
    /// <returns></returns>
    public ChatCommandBuilder AddParameter(ParameterInfo parInfo)
    {
        var descAttr = parInfo.GetCustomAttribute<DescriptionAttribute>();

        AddParameter(new CommandParameter
        {
            Description = descAttr?.Description ?? "", ParameterInfo = parInfo
        });

        return this;
    }

    /// <summary>
    /// Whether this command uses the command prefix or not.
    /// </summary>
    /// <param name="usePrefix">If true, the command will require to be prefixed by the user.</param>
    /// <returns></returns>
    public ChatCommandBuilder UsePrefix(bool usePrefix)
    {
        _usePrefix = usePrefix;
        return this;
    }

    /// <summary>
    /// Don't use prefix for this command.
    /// </summary>
    /// <returns></returns>
    public ChatCommandBuilder WithNoPrefix() => UsePrefix(false);
    
    /// <summary>
    /// Build the command info object and return it.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown when required options have not been set.</exception>
    public IChatCommand Build()
    {
        if (_name == null)
        {
            throw new InvalidOperationException("Chat command name must be set.");
        }

        if (_description == null)
        {
            throw new InvalidOperationException("Chat command description must be set.");
        }
        
        if (_controllerType == null)
        {
            throw new InvalidOperationException("Chat command controller type must be set.");
        }
        
        if (_handlerMethod == null)
        {
            throw new InvalidOperationException("Chat command handler method type must be set.");
        }

        foreach (var param in _handlerMethod.GetParameters())
        {
            AddParameter(param);
        }
        
        return new ChatCommand
        {
            Name = _name,
            Description = _description,
            Permission = _permission,
            Aliases = _aliases,
            ControllerType = _controllerType,
            HandlerMethod = _handlerMethod,
            Parameters = _parameters.ToArray(),
            UsePrefix = _usePrefix
        };
    }
}
