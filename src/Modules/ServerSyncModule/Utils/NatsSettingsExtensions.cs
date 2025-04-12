using EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Utils;

public static class NatsSettingsExtensions
{
    public static string GetConnectionUrl(this INatsSettings settings) =>
        $"nats://{settings.Host}:{settings.Port}";
}
