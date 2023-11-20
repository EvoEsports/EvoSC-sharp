using System.Collections;
using System.Dynamic;

namespace EvoSC.Common.Themes;

public class DynamicThemeOptions : DynamicObject, IDictionary<string, object>
{
    private readonly Dictionary<string, object> _options;

    public DynamicThemeOptions() => _options = new Dictionary<string, object>();
    
    public DynamicThemeOptions(Dictionary<string, object> options) => _options = options;

    public override bool TryGetMember(GetMemberBinder binder, out object? result) =>
        _options.TryGetValue(binder.Name.Replace('_', '.'), out result);

    public object this[string key]
    {
        get => _options[key];
        set => _options[key] = value;
    }

    public ICollection<string> Keys => _options.Keys;

    public ICollection<object> Values => _options.Values;

    public bool ContainsKey(string key) => _options.ContainsKey(key);

    public void Add(string key, object value) => _options.Add(key, value);

    public bool Remove(string key) => _options.Remove(key);

    public bool TryGetValue(string key, out object value) => _options.TryGetValue(key, out value!);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _options.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(KeyValuePair<string, object> item) => throw new NotImplementedException();

    public void Clear() => _options.Clear();

    public bool Contains(KeyValuePair<string, object> item) => throw new NotImplementedException();

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => throw new NotImplementedException();

    public bool Remove(KeyValuePair<string, object> item) => throw new NotImplementedException();

    public int Count => _options.Count;
    public bool IsReadOnly => false;
}
