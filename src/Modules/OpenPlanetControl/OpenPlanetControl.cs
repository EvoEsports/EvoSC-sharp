using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.OpenPlanetControl.Config;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetControl.Parsing;

namespace EvoSC.Modules.Official.OpenPlanetControl;

[Module(IsInternal = true)]
public class OpenPlanetControl : EvoScModule, IToggleable
{
    private readonly IManialinkManager _manialinks;
    private readonly IOpenPlanetControlSettings _settings;
    private readonly IManialinkInteractionHandler _manialinkInteractions;

    public OpenPlanetControl(IManialinkManager manialinks, IOpenPlanetControlSettings settings,
        IManialinkInteractionHandler manialinkInteractions)
    {
        _manialinks = manialinks;
        _settings = settings;
        _manialinkInteractions = manialinkInteractions;
    }

    public Task EnableAsync()
    {
        _manialinkInteractions.ValueReader.AddReader(new OpenPlanetInfoValueReader());
        
        _manialinks.SendPersistentManialinkAsync("OpenPlanetControl.DetectOP", new
        {
            config = _settings
        });

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        _manialinks.HideManialinkAsync("OpenPlanetControl.DetectOP");
        _manialinkInteractions.ValueReader.RemoveReaders(typeof(IOpenPlanetInfo));

        return Task.CompletedTask;
    }
}
