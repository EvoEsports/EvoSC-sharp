using EvoSC.Manialinks.Validation;

namespace EvoSC.Manialinks.Tests;

public class ValidationResultTests
{
    [Fact]
    public void Test_Validation_Result_Added()
    {
        var formValResult = new FormValidationResult();
        
        formValResult.AddResult(new EntryValidationResult
        {
            Name = "MyEntry",
            IsInvalid = true,
            Message = "My validation message."
        });

        var result = formValResult.GetResult("MyEntry")?.FirstOrDefault();
        
        Assert.Equal("MyEntry", result.Name);
        Assert.Equal("My validation message.", result.Message);
        Assert.True(result.IsInvalid);
        Assert.False(formValResult.IsValid);
    }
    
    [Fact]
    public void Test_Validation_Result_With_Multiple_Entries_Is_Invalid()
    {
        var formValResult = new FormValidationResult();

        formValResult.AddResult(new EntryValidationResult {Name = "MyEntry1", IsInvalid = true});
        formValResult.AddResult(new EntryValidationResult {Name = "MyEntry2", IsInvalid = true});
        formValResult.AddResult(new EntryValidationResult {Name = "MyEntry3", IsInvalid = false});
        
        Assert.False(formValResult.IsValid);
    }
    
    [Fact]
    public void Test_Validation_Result_With_Multiple_Entries_Is_Valid()
    {
        var formValResult = new FormValidationResult();

        formValResult.AddResult(new EntryValidationResult {Name = "MyEntry1", IsInvalid = false});
        formValResult.AddResult(new EntryValidationResult {Name = "MyEntry2", IsInvalid = false});
        formValResult.AddResult(new EntryValidationResult {Name = "MyEntry3", IsInvalid = false});
        
        Assert.True(formValResult.IsValid);
    }
}
