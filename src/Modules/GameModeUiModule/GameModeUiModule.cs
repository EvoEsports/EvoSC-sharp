using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.GameModeUiModule.Interfaces;

namespace EvoSC.Modules.Official.GameModeUiModule;

[Module(IsInternal = true)]
public class GameModeUiModule(IGameModeUiModuleService gameModeUiModuleService) : EvoScModule, IToggleable
{
    public Task EnableAsync() => gameModeUiModuleService.ApplyComponentSettingsAsync(gameModeUiModuleService.GetDefaultSettings());

    public Task DisableAsync() => Task.CompletedTask;
}
