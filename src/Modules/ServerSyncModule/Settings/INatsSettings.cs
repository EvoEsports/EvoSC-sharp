using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;
using LinqToDB.Common;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Settings;

[Settings]
public interface INatsSettings
{
    [Option(DefaultValue = "127.0.0.1"), Description("Host/IP of the NATS server connection.")]
    public string Host { get; set; }

    [Option(DefaultValue = 4222), Description("Port for the NATS server connection.")]
    public int Port { get; set; }

    [Option(DefaultValue = "EvoSC"), Description("The name of the stream to use for state messages.")]
    public string StreamName { get; set; }
    
    [Option(DefaultValue = "EvoSC-consumer")]
    [Description("The name of the consumer to use for message consumption.")]
    public string ConsumerName { get; set; }

    [Option(DefaultValue = "ServerSync")]
    [Description("The message group this server is part of. Used to get messages from all other servers in the same group.")]
    public string MessageGroup { get; set; }

    [Option(DefaultValue = (ulong)0)]
    [Description("The last player state update message ID.")]
    public ulong PlayerStatesStartSequence { get; set; }
    
    [Option(DefaultValue = false)]
    [Description("Whether to use a Key Value store for the NATS server.")]
    public bool UseKeyValueStore { get; set; }
    
    [Option(DefaultValue = "EvoSC-bucket")]
    [Description("The name of the key vault bucket.")]
    public string KeyVaultBucketName { get; set; }
}
