using System.Text.RegularExpressions;

namespace EvoSC.Modules.Official.OpenPlanetControl.Models;

public class OpenPlanetInfo
{
    /**
    private readonly string MODE_REGULAR = "REGULAR"; //Default: all signed plugins
    private readonly string MODE_DEV = "DEVMODE"; //All plugins, including unsigned ones 
    private readonly string MODE_OFFICIAL = "OFFICIAL"; //Plugins shipped with Openplanet
    private readonly string MODE_COMPETITION = "COMPETITION"; //Plugins approved for use in TMGL
    private readonly string MODE_UNKNOWN = "UNKNOWN";
**/

    public string version { get; set; } = "0.0.0";

    public string game { get; set; } = "unknown";
    public string branch { get; set; } = "";
    public string build { get; set; } = "";
    public string signatureMode { get; set; } = "REGULAR";
    public bool isOpenPlanet { get; set; } = false;

    public OpenPlanetInfo(string data)
    {
        var re = new Regex("^Openplanet ([\\d.]+) \\((\\w+), ([A-Z]\\w+), (\\d{4}-\\d{2}-\\d{2})\\)(?:\\s(?:\\[([A-Z]+)\\]))*$");
        var match = re.Match(data);
        if (!match.Success) return;
        this.isOpenPlanet = true;
        version = match.Groups[1].Value;
        game = match.Groups[2].Value;
        branch = match.Groups[3].Value;
        build = match.Groups[4].Value;
        signatureMode = match.Groups[5].Value;
        if (signatureMode == "") signatureMode = "REGULAR";
    }

    public bool isDevMode()
    {
        return signatureMode == "DEVMODE";
    }
}
