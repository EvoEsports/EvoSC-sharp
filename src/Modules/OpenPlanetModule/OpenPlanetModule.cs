using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.OpenPlanetModule.Config;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Parsing;

namespace EvoSC.Modules.Official.OpenPlanetModule;

[Module(IsInternal = true)]
public class OpenPlanetModule(IManialinkManager manialinks, IOpenPlanetControlSettings settings,
        IManialinkInteractionHandler manialinkInteractions)
    : EvoScModule, IToggleable
{
    public Task EnableAsync()
    {
        manialinkInteractions.ValueReader.AddReader(new OpenPlanetInfoValueReader());
        
        manialinks.SendPersistentManialinkAsync("OpenPlanetModule.DetectOP", new
        {
            config = settings
        });

        return Task.CompletedTask;
    }

    public Task DisableAsync()
    {
        manialinks.HideManialinkAsync("OpenPlanetModule.DetectOP");
        manialinkInteractions.ValueReader.RemoveReaders(typeof(IOpenPlanetInfo));

        return Task.CompletedTask;
    }
}
