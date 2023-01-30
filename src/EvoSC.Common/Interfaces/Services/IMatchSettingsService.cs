using GbxRemoteNet.XmlRpc.ExtraTypes;

namespace EvoSC.Common.Interfaces.Services;

public interface IMatchSettingsService
{
    public Task SetModeScriptSettingsAsync(Action<DynamicObject> settingsAction);
}
