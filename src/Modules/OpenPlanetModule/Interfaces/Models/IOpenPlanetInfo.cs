using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;

public interface IOpenPlanetInfo
{
    public Version Version { get; set; }
    public string Game { get; set; }
    public string Branch { get; set; }
    public string Build { get; set; }
    public OpenPlanetSignatureMode SignatureMode { get; set; }
    public bool IsOpenPlanet { get; set; }

    public static abstract IOpenPlanetInfo Parse(string toolInfo);
}
