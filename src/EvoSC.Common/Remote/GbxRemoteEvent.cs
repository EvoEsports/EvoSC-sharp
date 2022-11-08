namespace EvoSC.Common.Remote;

public class GbxRemoteEvent
{
    /// <summary>
    /// When a player sends a message through in-game chat.
    /// </summary>
    public const string PlayerChat = "GbxRemote.PlayerChat";
    /// <summary>
    /// when a player successfully connects to the server.
    /// </summary>
    public const string PlayerConnect = "GbxRemote.PlayerConnect";
    /// <summary>
    /// When a player disconnects from the server.
    /// </summary>
    /// <returns></returns>
    public const string PlayerDisconnect = "GbxRemote.PlayerDisconnect";
    /// <summary>
    /// When a player's state has changed.
    /// </summary>
    public const string PlayerInfoChanged = "GbxRemote.PlayerInfoChanged";
    /// <summary>
    /// When a map has ended.
    /// </summary>
    public const string EndMap = "GbxRemote.EndMap";
    /// <summary>
    /// When the match has ended.
    /// </summary>
    public const string EndMatch = "GbxRemote.EndMatch";
    /// <summary>
    /// When the map starts.
    /// </summary>
    public const string BeginMap = "GbxRemote.BeginMap";
    /// <summary>
    /// When a match is about to start.
    /// </summary>
    public const string BeginMatch = "GbxRemote.BeginMatch";
    /// <summary>
    /// When a echo message has been sent.
    /// </summary>
    public const string Echo = "GbxRemote.Echo";
    /// <summary>
    /// When an answer from a manialink has been triggered.
    /// </summary>
    public const string ManialinkPageAnswer = "GbxRemote.ManialinkPageAnswer";
    /// <summary>
    /// When the map list got modified.
    /// </summary>
    public const string MapListModified = "GbxRemote.MapListModified";
}
