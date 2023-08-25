using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Evo.GeardownModule.Repositories;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GeardownApiService : IGeardownApiService
{
    public IGeardownEventApi Events { get; }
    public IGeardownGroupApi Groups { get; }
    public IGeardownMatchApi Matches { get; }

    public GeardownApiService(IGeardownSettings settings)
    {
        Events = new GeardownEventApi(settings);
        Groups = new GeardownGroupApi(settings);
        Matches = new GeardownMatchApi(settings);
    }
}