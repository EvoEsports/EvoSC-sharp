namespace EvoSC.Manialinks.Validation;

public class EntryValidationResult
{
    /// <summary>
    /// The name of the entry field that was validated.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Whether this field is invalid or not.
    /// </summary>
    public required bool IsInvalid { get; init; }
    
    /// <summary>
    /// The error message if this field is invalid.
    /// </summary>
    public string? Message { get; init; }
}
