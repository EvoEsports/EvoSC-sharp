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
    public PlayerDetailedInfo? DetailedInfo { get; }
    public PlayerInfo Info { get; }
    public PlayerFlags Flags => new(Info.Flags);
    protected GbxRemoteClient Client { get; }
}
