using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Services;
using EvoSC.Modules.Evo.GeardownModule.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Settings;

namespace EvoSC.Modules.Evo.GeardownModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class GeardownApiService : IGeardownApiService
{
    public IGeardownMatchApi Matches { get; }

    public GeardownApiService(IGeardownSettings settings)
    {
        Matches = new GeardownMatchApi(settings);
    }
}
