using System.Text.RegularExpressions;
using EvoSC.Common.Util.EnumIdentifier;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces.Models;

namespace EvoSC.Modules.Official.OpenPlanetControl.Models;

public class OpenPlanetInfo : IOpenPlanetInfo
{
    private static readonly Regex OpenPlanetRegex =
        new("^Openplanet ([\\d.]+) \\((\\w+), ([A-Z]\\w+), (\\d{4}-\\d{2}-\\d{2})\\)(?:\\s(?:\\[([A-Z]+)\\]))*$");
    
    public Version Version { get; set; }
    public string Game { get; set; }
    public string Branch { get; set; }
    public string Build { get; set; }
    public OpenPlanetSignatureMode SignatureMode { get; set; }
    public bool IsOpenPlanet { get; set; }

    internal OpenPlanetInfo(Version version, string game, string branch, string build, OpenPlanetSignatureMode mode,
        bool isOpenPlanet)
    {
        Version = version;
        Game = game;
        Branch = branch;
        Build = build;
        SignatureMode = mode;
        IsOpenPlanet = isOpenPlanet;
    }

    public static IOpenPlanetInfo Parse(string toolInfo)
    {
        var match = OpenPlanetRegex.Match(toolInfo);

        if (!match.Success)
        {
            return new OpenPlanetInfo(Version.Parse("0.0.0"), "", "", "", OpenPlanetSignatureMode.Unknown, false);
        }

        var version = Version.Parse(match.Groups[1].Value);
        var game = match.Groups[2].Value;
        var branch = match.Groups[3].Value;
        var build = match.Groups[4].Value;
        var signatureMode = match.Groups[5].Value.ToEnumValue<OpenPlanetSignatureMode>() ?? OpenPlanetSignatureMode.Regular;

        return new OpenPlanetInfo(version, game, branch, build, signatureMode, true);
    }
}
