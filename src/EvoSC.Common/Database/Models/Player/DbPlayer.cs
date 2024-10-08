﻿using EvoSC.Common.Interfaces.Models;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Player;

[Table(TableName)]
public class DbPlayer : IPlayer
{
    public const string TableName = "Players";
    
    /// <summary>
    /// Database ID of the player.
    /// </summary>
    [PrimaryKey, Identity]
    public long Id { get; set; }
    
    /// <summary>
    /// Time at which the player was last seen on the server.
    /// </summary>
    [Column]
    public DateTime? LastVisit { get; set; }
    
    /// <summary>
    /// Essentially the time at which the player first appeared on the server.
    /// </summary>
    [Column]
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Time at which the player was updated the last time in the database.
    /// </summary>
    [Column]
    public DateTime UpdatedAt { get; set; }

    [Column]
    public string AccountId { get; set; }
    
    [Column]
    public string NickName { get; set; }
    
    [Column]
    public string UbisoftName { get; set; }
    
    [Column]
    public string? Zone { get; set; }

    public IPlayerSettings Settings => DbSettings;
    public IEnumerable<IGroup> Groups { get; set; }
    public IGroup? DisplayGroup { get; set; }

    [Association(ThisKey = nameof(Id), OtherKey = nameof(DbPlayerSettings.PlayerId))]
    public DbPlayerSettings DbSettings { get; set; }

    public DbPlayer() {}

    public DbPlayer(IPlayer? player)
    {
        if (player == null)
        {
            return;
        }

        Id = player.Id;
        LastVisit = default;
        CreatedAt = default;
        UpdatedAt = default;
        AccountId = player.AccountId;
        NickName = player.NickName;
        UbisoftName = player.UbisoftName;
        Zone = player.Zone;
        Groups = player.Groups;
        DisplayGroup = player.DisplayGroup;
    }
    
    public bool Equals(IPlayer? other) => other != null && AccountId.Equals(other.AccountId, StringComparison.Ordinal);
}
