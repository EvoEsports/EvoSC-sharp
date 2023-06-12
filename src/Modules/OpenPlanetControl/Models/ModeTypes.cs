namespace EvoSC.Modules.Official.OpenPlanetControl.Models;

public enum ModeType
{
    UNKNOWN = 0,
    REGULAR = 1, //Default: all signed plugins
    DEV = 2 , //All plugins, including unsigned ones 
    OFFICIAL = 4, //Plugins shipped with Openplanet
    COMPETITION = 8 //Plugins approved for use in TMGL
}
