using EvoSC.Modules.Official.TeamSettingsModule.Models;

namespace EvoSC.Modules.Official.TeamSettingsModule.Events.EventArgs;

public class TeamSettingsEventArgs: System.EventArgs
{
    public required TeamSettingsModel Settings { get; init; }
}
