using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

namespace EvoSC.Modules.Official.LocalRecordsModule;

[Module(IsInternal = true)]
public class LocalRecordsModule(IManialinkManager manialinkManager, ILocalRecordsService localRecordsService) : EvoScModule, IToggleable
{
    public Task EnableAsync() =>
        manialinkManager.SendPersistentManialinkAsync("LocalRecordsModule.LocalRecordsWidget",
            async () =>
            {
                var records = localRecordsService.GetLocalsOfCurrentMapAsync();
                return new { records };
            });

    public Task DisableAsync() => manialinkManager.HideManialinkAsync("LocalRecordsModule.LocalRecordsWidget");
}
