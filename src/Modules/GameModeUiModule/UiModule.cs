using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.UIModule.Interfaces;

namespace EvoSC.Modules.Official.UIModule;

[Module(IsInternal = true)]
public class UiModule(IUiModuleService uiModuleService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => uiModuleService.ApplyConfigurationAsync();

    public Task DisableAsync() => Task.CompletedTask;
}
