namespace EvoSC.Manialinks.Validation;

public class FormValidationResult
{
    private readonly Dictionary<string, List<EntryValidationResult>> _results = new();

    public void AddResult(EntryValidationResult result)
    {
        if (!_results.ContainsKey(result.Name))
        {
            _results.Add(result.Name, new List<EntryValidationResult>());
        }

        _results[result.Name].Add(result);
    }

    public IEnumerable<EntryValidationResult>? GetResult(string name) =>
        _results.TryGetValue(name, out var result) ? result : null;

    public bool IsValid => _results.Values.All(results => results.All(v => !v.IsInvalid));
}
