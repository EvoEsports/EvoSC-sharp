using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EvoSC.Core.Commands.Generic.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EvoSC.Core.Commands.Generic;

public class CommandCollection : ICollection<ICommand>, IReadOnlyDictionary<string, ICommand>
{
    private Dictionary<string, ICommand> _commands = new();
    private Dictionary<string, ICommandGroupInfo> _groups = new();
    private Dictionary<string, List<string>> _groupsMap = new();

    /// <summary>
    /// Register a new command.
    /// </summary>
    /// <param name="cmd">Command to add.</param>
    public void Add(ICommand cmd) => _commands.Add(cmd.Name, cmd);

    /// <summary>
    /// Add a new command group.
    /// </summary>
    /// <param name="name"></param>
    public void AddGroup(ICommandGroupInfo group) => _groups.Add(group.Name, group);

    /// <summary>
    /// Map a command to a group.
    /// </summary>
    /// <param name="group"></param>
    /// <param name="command"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public void MapCommandToGroup(string group, ICommand command)
    {
        if (!ContainsGroup(group))
        {
            throw new KeyNotFoundException("Group name not found.");
        }

        if (!Contains(command))
        {
            Add(command);
        }

        if (CommandContainsInGroup(group, command.Name))
        {
            throw new InvalidOperationException("Command already mapped to group.");
        }

        _groupsMap[group].Add(command.Name);
    }

    /// <summary>
    /// Unmap a command from a group.
    /// </summary>
    /// <param name="group"></param>
    /// <param name="command"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public void UnmapCommandFromGroup(string group, string cmd)
    {
        if (!CommandContainsInGroup(group, cmd))
        {
            throw new KeyNotFoundException("Command does not exit in the group.");
        }

        _groupsMap[group].Remove(cmd);
    }

    /// <summary>
    /// Clear all commands
    /// </summary>
    public void Clear() => _commands.Clear();

    /// <summary>
    /// Check if the collection contains a command.
    /// </summary>
    /// <param name="cmd">Command to check.</param>
    /// <returns></returns>
    public bool Contains(ICommand cmd) => _commands.ContainsKey(cmd.Name);

    /// <summary>
    /// Check if the collection contains a command.
    /// </summary>
    /// <param name="name">Name of the command.</param>
    /// <returns></returns>
    public bool ContainsKey(string key) => _commands.ContainsKey(key);

    /// <summary>
    /// Check whether the collection contains a command group.
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public bool ContainsGroup(string group) => _groups.ContainsKey(group);

    /// <summary>
    /// Check whether the collection contains a command group.
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public bool ContainsGroup(ICommandGroupInfo group) => _groups.ContainsKey(group.Name);

    /// <summary>
    /// Check if a group contains a command.
    /// </summary>
    /// <param name="group"></param>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public bool CommandContainsInGroup(string group, string cmd) =>
        _groupsMap.ContainsKey(group) && _groupsMap[group].Any(c => c == cmd);

    /// <summary>
    /// Remove a command.
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    public bool Remove(ICommand cmd)
    {
        if (CommandContainsInGroup(cmd.Group.Name, cmd.Name))
        {
            UnmapCommandFromGroup(cmd.Group.Name, cmd.Name);
        }

        return _commands.Remove(cmd.Name);
    }

    /// <summary>
    /// Remove a group and all it's mapped commands.
    /// </summary>
    /// <param name="group">The group to remove.</param>
    /// <returns></returns>
    public bool RemoveGroup(ICommandGroupInfo group)
    {
        if (_groups.Remove(group.Name) && _groupsMap.Remove(group.Name))
        {
            return true;
        }

        return false;
    }

    public ICommandGroupInfo GetGroup(string groupName) => _groups[groupName];

    public void CopyTo(ICommand[] array, int arrayIndex) { }

    public bool TryGetValue(string key, out ICommand value) => _commands.TryGetValue(key, out value);

    public ICommand this[string key] => _commands[key];

    public IEnumerable<string> Keys => _commands.Keys;
    public IEnumerable<ICommand> Values => _commands.Values;

    public int Count => _commands.Count;
    public bool IsReadOnly => false;

    IEnumerator<KeyValuePair<string, ICommand>> IEnumerable<KeyValuePair<string, ICommand>>.GetEnumerator() =>
        _commands.GetEnumerator();

    public IEnumerator<ICommand> GetEnumerator() => _commands.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
