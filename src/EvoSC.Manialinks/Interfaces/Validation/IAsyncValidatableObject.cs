using System.ComponentModel.DataAnnotations;

namespace EvoSC.Manialinks.Interfaces.Validation;

/// <summary>
/// Enables asynchronous model-level validation.
/// </summary>
public interface IAsyncValidatableObject
{
    /// <summary>
    /// Validate the current model in an async context.
    /// </summary>
    /// <param name="validationContext">The validation context for this model.</param>
    /// <returns></returns>
    public Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext);
}
