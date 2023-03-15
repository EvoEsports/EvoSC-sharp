namespace EvoSC.Manialinks.Validation;

public class ValidationResult
{
    public required string Name { get; init; }
    public required bool IsInvalid { get; init; }
    public string? Message { get; init; }
}
