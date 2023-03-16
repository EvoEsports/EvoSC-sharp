namespace EvoSC.Manialinks.Validation;

public class EntryValidationResult
{
    public required string Name { get; init; }
    public required bool IsInvalid { get; init; }
    public string? Message { get; init; }
}
