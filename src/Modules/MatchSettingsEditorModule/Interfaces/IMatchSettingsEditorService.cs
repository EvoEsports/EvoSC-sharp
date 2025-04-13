using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;

public interface IMatchSettingsEditorService
{
    public Task ShowEditorAsync(IOnlinePlayer player);
}
