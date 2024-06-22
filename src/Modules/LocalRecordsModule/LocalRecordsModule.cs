using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

namespace EvoSC.Modules.Official.LocalRecordsModule;

[Module(IsInternal = true)]
public class LocalRecordsModule(IManialinkManager manialinkManager, ILocalRecordsService localRecordsService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => localRecordsService.ShowWidgetToAllAsync();

    public Task DisableAsync() => manialinkManager.HideManialinkAsync("LocalRecordsModule.LocalRecordsWidget");
}
