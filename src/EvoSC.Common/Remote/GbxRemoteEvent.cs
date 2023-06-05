using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Remote;

public enum GbxRemoteEvent
{
    /// <summary>
    /// When a player sends a message through in-game chat.
    /// </summary>
    [Identifier(Name = "GbxRemote.PlayerChat")]
    PlayerChat,
    /// <summary>
    /// When a player successfully connects to the server.
    /// </summary>
    [Identifier(Name = "GbxRemote.PlayerConnect")]
    PlayerConnect,
    /// <summary>
    /// When a player disconnects from the server.
    /// </summary>
    /// <returns></returns>
    [Identifier(Name = "GbxRemote.PlayerDisconnect")]
    PlayerDisconnect,
    /// <summary>
    /// When a player's state has changed.
    /// </summary>
    [Identifier(Name = "GbxRemote.PlayerInfoChanged")]
    PlayerInfoChanged,
    /// <summary>
    /// When a map has ended.
    /// </summary>
    [Identifier(Name = "GbxRemote.EndMap")]
    EndMap,
    /// <summary>
    /// When the match has ended.
    /// </summary>
    [Identifier(Name = "GbxRemote.EndMatch")]
    EndMatch,
    /// <summary>
    /// When the map starts.
    /// </summary>
    [Identifier(Name = "GbxRemote.BeginMap")]
    BeginMap,
    /// <summary>
    /// When a match is about to start.
    /// </summary>
    [Identifier(Name = "GbxRemote.BeginMatch")]
    BeginMatch,
    /// <summary>
    /// When a echo message has been sent.
    /// </summary>
    [Identifier(Name = "GbxRemote.Echo")]
    Echo,
    /// <summary>
    /// When an answer from a manialink has been triggered.
    /// </summary>
    [Identifier(Name = "GbxRemote.ManialinkPageAnswer")]
    ManialinkPageAnswer,
    /// <summary>
    /// When the map list got modified.
    /// </summary>
    [Identifier(Name = "GbxRemote.MapListModified")]
    MapListModified,
    /// <summary>
    /// When the server status changes
    /// </summary>
    [Identifier(Name = "GbxRemote.StatusChanged")]
    StatusChanged,
    
}
