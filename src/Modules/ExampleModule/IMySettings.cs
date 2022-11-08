using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.ExampleModule;

[Settings]
public interface IMySettings
{
    [Option(DefaultValue = "default value")]
    public string MyOption { get; set; }
}
