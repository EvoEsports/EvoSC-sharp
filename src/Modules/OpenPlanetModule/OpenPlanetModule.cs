using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Parsing;

namespace EvoSC.Modules.Official.OpenPlanetModule;

[Module(IsInternal = true)]
public class OpenPlanetModule : EvoScModule, IToggleable
{
    private readonly IManialinkManager _manialinks;
    private readonly IOpenPlanetControlSettings _settings;
    private readonly IManialinkInteractionHandler _manialinkInteractions;

    public OpenPlanetModule(IManialinkManager manialinks, IOpenPlanetControlSettings settings,
        IManialinkInteractionHandler manialinkInteractions)
    {
        _manialinks = manialinks;
        _settings = settings;
        _manialinkInteractions = manialinkInteractions;
    }

    public Task EnableAsync()
    {
        _manialinkInteractions.ValueReader.AddReader(new OpenPlanetInfoValueReader());
        
        _manialinks.SendPersistentManialinkAsync("OpenPlanetModule.DetectOP", new
        {
            config = _settings
        });

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        _manialinks.HideManialinkAsync("OpenPlanetModule.DetectOP");
        _manialinkInteractions.ValueReader.RemoveReaders(typeof(IOpenPlanetInfo));

        return Task.CompletedTask;
    }
}
