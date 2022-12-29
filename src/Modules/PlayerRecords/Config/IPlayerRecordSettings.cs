using System.ComponentModel;
using Config.Net;
using EvoSC.Common.Config.Models;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Official.PlayerRecords.Config;

[Settings]
public interface IPlayerRecordSettings
{
    [Option(DefaultValue = EchoOptions.None), Description("How to send a message about a new PB.")]
    public EchoOptions EchoPb { get; }
}
