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

    [Option(DefaultValue = "ServerSync")]
    [Description("The message group this server is part of. Used to get messages from all other servers in the same group.")]
    public string MessageGroup { get; set; }

    [Option(DefaultValue = (ulong)0)]
    [Description("The last player state update message ID.")]
    public ulong PlayerStatesStartSequence { get; set; }
    
    [Option(DefaultValue = "EvoSC-bucket")]
    [Description("The name of the key vault bucket.")]
    public string KeyVaultBucketName { get; set; }
}
