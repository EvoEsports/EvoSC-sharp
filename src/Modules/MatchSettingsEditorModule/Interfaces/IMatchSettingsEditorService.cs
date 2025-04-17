using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Interfaces;

public interface IMatchSettingsEditorService
{
    public Task ShowOverviewAsync(IOnlinePlayer player);
    public Task ShowEditorAsync(IOnlinePlayer player, string name);
}
