using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EvoSC.Domain.Maps;
using EvoSC.Domain.Players;
using GbxRemoteNet;
using GbxRemoteNet.Structs;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EvoSC.Interfaces.Players;

public interface IServerPlayer : IPlayer
{
    /// <summary>
    /// Extra information about the player on the server.
    /// </summary>
    public PlayerDetailedInfo? DetailedInfo { get; }
    /// <summary>
    /// Information about the player on the server.
    /// </summary>
    public PlayerInfo Info { get; }
    /// <summary>
    /// Various flags about the player on the server.
    /// </summary>
    public PlayerFlags Flags => new(Info.Flags);
    protected GbxRemoteClient Client { get; }
}
