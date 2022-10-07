namespace EvoSC.CLI.Attributes;

public class CliOptionAttribute : Attribute
{
    public string[] Aliases { get; }
    public string Description { get; }
    public Type TOption { get; }
    
    public CliOptionAttribute(Type tOption, string name, string description)
    {
        Aliases = new[] {name};
        TOption = tOption;
        Description = description;
    }
    
    public CliOptionAttribute(Type tOption, string description, params string[] aliases)
    {
        Aliases = aliases;
        Description = description;
        TOption = tOption;
    }
}
