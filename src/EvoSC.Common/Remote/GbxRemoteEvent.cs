using EvoSC.Common.Events.Attributes;

namespace EvoSC.Common.Remote;

public enum GbxRemoteEvent
{
    /// <summary>
    /// When a player sends a message through in-game chat.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.PlayerChat")]
    PlayerChat,
    /// <summary>
    /// When a player successfully connects to the server.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.PlayerConnect")]
    PlayerConnect,
    /// <summary>
    /// When a player disconnects from the server.
    /// </summary>
    /// <returns></returns>
    [EventIdentifier(Name = "GbxRemote.PlayerDisconnect")]
    PlayerDisconnect,
    /// <summary>
    /// When a player's state has changed.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.PlayerInfoChanged")]
    PlayerInfoChanged,
    /// <summary>
    /// When a map has ended.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.EndMap")]
    EndMap,
    /// <summary>
    /// When the match has ended.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.EndMatch")]
    EndMatch,
    /// <summary>
    /// When the map starts.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.BeginMap")]
    BeginMap,
    /// <summary>
    /// When a match is about to start.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.BeginMatch")]
    BeginMatch,
    /// <summary>
    /// When a echo message has been sent.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.Echo")]
    Echo,
    /// <summary>
    /// When an answer from a manialink has been triggered.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.ManialinkPageAnswer")]
    ManialinkPageAnswer,
    /// <summary>
    /// When the map list got modified.
    /// </summary>
    [EventIdentifier(Name = "GbxRemote.MapListModified")]
    MapListModified
}
