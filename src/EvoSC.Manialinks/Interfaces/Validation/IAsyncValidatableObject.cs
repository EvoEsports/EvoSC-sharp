using System.ComponentModel.DataAnnotations;

namespace EvoSC.Manialinks.Interfaces.Validation;

public interface IAsyncValidatableObject
{
    public Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext);
}
