namespace EvoSC.Manialinks.Validation;

public class FormValidationResult
{
    private readonly Dictionary<string, ValidationResult> _results = new();

    public void AddResult(ValidationResult result) =>
        _results.Add(result.Name, result);

    public ValidationResult? GetResult(string name) =>
        _results.TryGetValue(name, out var result) ? result : null;

    public bool IsValid => _results.Values.All(v => !v.IsInvalid);
}
