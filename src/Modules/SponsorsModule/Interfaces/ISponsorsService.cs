using EvoSC.Common.Interfaces.Models;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.SponsorsModule.Interfaces;

public interface ISponsorsService
{
    public Task ShowWidgetToAllSpectators();
    public Task HideWidgetFromEveryone();
    public Task ShowWidget(string playerLogin);
    public Task ShowOrHide(PlayerInfoChangedGbxEventArgs playerInfoChangedArgs);
}
