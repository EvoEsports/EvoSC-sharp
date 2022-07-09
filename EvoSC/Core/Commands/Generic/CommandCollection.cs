using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EvoSC.Core.Commands.Generic;

public class CommandCollection : ICollection<Command>, IReadOnlyDictionary<string, Command>
{
    private Dictionary<string, Command> _commands = new();
    private Dictionary<string, List<Command>> _groupsMap = new();

    public int Count => _commands.Count;
    public bool IsReadOnly => false;

    IEnumerator<KeyValuePair<string, Command>> IEnumerable<KeyValuePair<string, Command>>.GetEnumerator() =>
        _commands.GetEnumerator();

    public IEnumerator<Command> GetEnumerator() => _commands.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public void Add(Command item) => _commands.Add(item.Name, item);

    public void Clear() => _commands.Clear();

    public bool Contains(Command item) => _commands.ContainsKey(item.Name);

    public void CopyTo(Command[] array, int arrayIndex) {}

    public bool Remove(Command item) =>_commands.Remove(item.Name);

    public bool ContainsKey(string key) => _commands.ContainsKey(key);

    public bool TryGetValue(string key, out Command value) => _commands.TryGetValue(key, out value);

    public Command this[string key] => _commands[key];

    public IEnumerable<string> Keys => _commands.Keys;
    public IEnumerable<Command> Values => _commands.Values;
}
