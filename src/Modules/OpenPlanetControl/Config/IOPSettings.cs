using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.OpenPlanetControl.Config;

[Settings]
public interface IopSettings
{
    [Option(DefaultValue = true), Description("Is the module enabled")]
    public bool Enabled { get; }
    
    [Option, Description("Allowed types")]
    public string[] AllowedTypes { get; }
    
}
