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

    /// <summary>
    /// Create a new chat command.
    /// </summary>
    public ChatCommandBuilder()
    {
        _aliases = new List<string>();
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
        
        return new ChatCommand
        {
            Name = _name,
            Description = _description,
            Permission = _permission,
            Aliases = _aliases,
            ControllerType = _controllerType,
            HandlerMethod = _handlerMethod
        };
    }
}
