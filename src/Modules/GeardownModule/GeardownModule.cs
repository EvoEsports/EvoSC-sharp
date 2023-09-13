using EvoSC.Modules.Attributes;
using EvoSC.Modules.Evo.GeardownModule.Settings;

namespace EvoSC.Modules.Evo.GeardownModule;

[Module(IsInternal = true)]
public class GeardownModule : EvoScModule
{
    public GeardownModule(IGeardownSettings settings)
    {
        Console.WriteLine($"settings: {settings.ApiAccessToken}");
    }
}
