using System.ComponentModel;
using System.Reflection;
using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class ChatCommandBuilder
{
    private string _name;
    private string _description;
    private string? _permission;
    private List<string> _aliases;
    private Type _controllerType;
    private MethodInfo _handlerMethod;
    private List<ICommandParameter> _parameters;
    private bool _usePrefix;

    /// <summary>
    /// Create a new chat command.
    /// </summary>
    public ChatCommandBuilder()
    {
        _aliases = new List<string>();
        _parameters = new List<ICommandParameter>();
        _usePrefix = true;
    }

    /// <summary>
    /// Modify an existing chat command.
    /// </summary>
    /// <param name="cmd"></param>
    public ChatCommandBuilder(IChatCommand cmd)
    {
        _name = cmd.Name;
        _description = cmd.Description;
        _permission = cmd.Permission;
        _aliases = cmd.Aliases.ToList();
        _parameters = cmd.Parameters.ToList();
    }

    public ChatCommandBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ChatCommandBuilder WithDescription(string desc)
    {
        _description = desc;
        return this;
    }

    public ChatCommandBuilder WithPermission(string perm)
    {
        _permission = perm;
        return this;
    }

    public ChatCommandBuilder AddAlias(string alias)
    {
        _aliases.Add(alias);
        return this;
    }

    public ChatCommandBuilder WithController(Type controllerType)
    {
        _controllerType = controllerType;
        return this;
    }

    public ChatCommandBuilder WithController<TController>() => WithController(typeof(TController));

    public ChatCommandBuilder WithHandlerMethod(MethodInfo method)
    {
        _handlerMethod = method;
        return this;
    }

    public ChatCommandBuilder WithHandlerMethod(Action method) => WithHandlerMethod(method.Method);
    public ChatCommandBuilder WithHandlerMethod<T>(Action<T> method) => WithHandlerMethod(method.Method);
    public ChatCommandBuilder WithHandlerMethod<T1, T2>(Action<T1, T2> method) => WithHandlerMethod(method.Method);
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3>(Action<T1, T2, T3> method) => WithHandlerMethod(method.Method);
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method) => WithHandlerMethod(method.Method);
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method) => WithHandlerMethod(method.Method);
    public ChatCommandBuilder WithHandlerMethod<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method) => WithHandlerMethod(method.Method);

    public ChatCommandBuilder AddParameter(ICommandParameter parameter)
    {
        _parameters.Add(parameter);
        return this;
    }

    public ChatCommandBuilder AddParameter(ParameterInfo parInfo)
    {
        var descAttr = parInfo.GetCustomAttribute<DescriptionAttribute>();

        AddParameter(new CommandParameter(
            parInfo,
            descAttr == null ? null : descAttr.Description
        ));

        return this;
    }

    public ChatCommandBuilder UsePrefix(bool usePrefix)
    {
        _usePrefix = usePrefix;
        return this;
    }

    public ChatCommandBuilder WithNoPrefix() => UsePrefix(false);
    
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
