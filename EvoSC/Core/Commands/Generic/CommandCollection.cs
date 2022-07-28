using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EvoSC.Core.Commands.Generic.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore.Update;

namespace EvoSC.Core.Commands.Generic;

public class CommandCollection : ICommandCollection<ICommand>
{
    private List<ICommand> _commands = new();
    
    public IReadOnlyList<ICommand> Commands { get; }
    
    public void AddCommand(ICommand cmd)
    {
        if (CommandExists(cmd.Name, cmd.Group?.Name))
        {
            throw new InvalidOperationException($"Command '{cmd.Name}' already exists.");
        }
        
        _commands.Add(cmd);
    }

    public bool RemoveCommand(string name, string? group = null) =>
        _commands.Remove(GetCommand(name, group));

    public bool RemoveGroup(string group) =>
        _commands.RemoveAll(c => c.Group != null && c.Group.Name == group) > 0;

    public ICommand GetCommand(string name, string? group = null)
    {
        if (!CommandExists(name, group))
        {
            throw new InvalidOperationException($"Command '{name}' in group '{group}' does not exist.");
        }

        return _commands.First(c => c.Name == name && c.Group?.Name == group);
    }

    public bool CommandExists(string name, string? group = null) =>
        _commands.Any(c => c.Name == name && c.Group?.Name == group);
}
