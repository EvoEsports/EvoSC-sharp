namespace EvoSC.CLI.Attributes;

public class CliCommandAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public CliCommandAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
