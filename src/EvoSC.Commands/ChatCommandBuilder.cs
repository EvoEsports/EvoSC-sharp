using EvoSC.Commands.Interfaces;

namespace EvoSC.Commands;

public class ChatCommandBuilder
{
    private string _name;
    private string _description;
    private string? _permission;
    private List<string> _aliases;

    /// <summary>
    /// Create a new chat command.
    /// </summary>
    public ChatCommandBuilder()
    {
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
        
        return new ChatCommand
        {
            Name = _name,
            Description = _description,
            Permission = _permission
        };
    }
}
