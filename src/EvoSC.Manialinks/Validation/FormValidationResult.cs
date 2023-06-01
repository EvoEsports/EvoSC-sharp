namespace EvoSC.Manialinks.Validation;

/// <summary>
/// Holds all validation results for all entries within a Manialink Form.
/// </summary>
public class FormValidationResult
{
    private readonly Dictionary<string, List<EntryValidationResult>> _results = new();

    /// <summary>
    /// Add a new validation result to the form.
    /// </summary>
    /// <param name="result">Result of an Entry field in the form.</param>
    public void AddResult(EntryValidationResult result)
    {
        if (!_results.ContainsKey(result.Name))
        {
            _results.Add(result.Name, new List<EntryValidationResult>());
        }

        _results[result.Name].Add(result);
    }

    /// <summary>
    /// Get the result of an Entry field within a form.
    /// </summary>
    /// <param name="name">The name of the entry field.</param>
    /// <returns></returns>
    public IEnumerable<EntryValidationResult>? GetResult(string name) =>
        _results.TryGetValue(name, out var result) ? result : null;

    /// <summary>
    /// Whether all entry fields are valid or not.
    /// </summary>
    public bool IsValid => _results.Values.All(results => results.All(v => !v.IsInvalid));
}
