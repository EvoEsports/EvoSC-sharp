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
    public static string PlayerDisconnect = "GbxRemote.PlayerDisconnect";
    /// <summary>
    /// When a player's state has changed.
    /// </summary>
    public static string PlayerInfoChanged = "GbxRemote.PlayerInfoChanged";
    /// <summary>
    /// When a map has ended.
    /// </summary>
    public static string EndMap = "GbxRemote.EndMap";
    /// <summary>
    /// When the match has ended.
    /// </summary>
    public static string EndMatch = "GbxRemote.EndMatch";
    /// <summary>
    /// When the map starts.
    /// </summary>
    public static string BeginMap = "GbxRemote.BeginMap";
    /// <summary>
    /// When a match is about to start.
    /// </summary>
    public static string BeginMatch = "GbxRemote.BeginMatch";
    /// <summary>
    /// When a echo message has been sent.
    /// </summary>
    public static string Echo = "GbxRemote.Echo";
    /// <summary>
    /// When an answer from a manialink has been triggered.
    /// </summary>
    public static string ManialinkPageAnswer = "GbxRemote.ManialinkPageAnswer";
    /// <summary>
    /// When the map list got modified.
    /// </summary>
    public static string MapListModified = "GbxRemote.MapListModified";
}
