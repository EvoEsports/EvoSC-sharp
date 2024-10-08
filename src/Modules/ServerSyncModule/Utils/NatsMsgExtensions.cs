using System.Text.Json;
using NATS.Client;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Utils;

public static class NatsMsgExtensions
{
    public static T? Deserialize<T>(this Msg msg) => 
        JsonSerializer.Deserialize<T>(msg.Data);
}
