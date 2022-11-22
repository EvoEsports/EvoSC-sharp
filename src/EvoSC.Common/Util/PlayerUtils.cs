using System.Text;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models;
using GbxRemoteNet.Structs;
using Microsoft.Extensions.Primitives;

namespace EvoSC.Common.Util;

public static class PlayerUtils
{
    /// <summary>
    /// Convert a Trackmania player's account ID to the old "login" format.
    /// </summary>
    /// <param name="accountId">The account ID format of the user.</param>
    /// <returns></returns>
    public static string ConvertAccountIdToLogin(string accountId)
    {
        var bytes = Convert.FromHexString(accountId.Replace("-", ""));

        return Convert.ToBase64String(bytes)
            .Replace("=", "")
            .Replace("+", "-")
            .Replace("/", "_");
    }

    /// <summary>
    /// Convert a Trackmania player's old "login" format to the account ID format.
    /// </summary>
    /// <param name="login">Login format of the user.</param>
    /// <returns></returns>
    public static string ConvertLoginToAccountId(string login)
    {
        var base64 = login
            .Replace("-", "+")
            .Replace("_", "/");

        var sb = new StringBuilder(base64);
        for (int i = 0; i < login.Length % 4; i++)
        {
            sb.Append("=");
        }
        
        var bytes = Convert.FromBase64String(sb.ToString());

        var accountId = Convert.ToHexString(bytes[0..4])
                        + "-"
                        + Convert.ToHexString(bytes[4..6])
                        + "-"
                        + Convert.ToHexString(bytes[6..8])
                        + "-"
                        + Convert.ToHexString(bytes[8..10])
                        + "-"
                        + Convert.ToHexString(bytes[10..16]);

        return accountId.ToLower();
    }

    /// <summary>
    /// Get the current state of this player as an enum.
    /// </summary>
    /// <param name="playerInfo"></param>
    /// <returns></returns>
    public static PlayerState GetState(this TmPlayerDetailedInfo playerInfo)
    {
        return playerInfo.IsSpectator ? PlayerState.Spectating : PlayerState.Playing;
    }

    /// <summary>
    /// Check if the player is forced into spectator and can not unselect spectator.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool IsForcedSpectator(this TmPlayerInfo player) => player.Flags % 10 == 1 || player.IsForcedSpectatorSelectable();
    
    /// <summary>
    /// Check if the player is forced into spectator but they can chose to select out of it.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool IsForcedSpectatorSelectable(this TmPlayerInfo player) => player.Flags % 10 == 2;
    
    /// <summary>
    /// Get the player's stereo display mode.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static int StereoDisplayMode(this TmPlayerInfo player) => (player.Flags / 1000) % 10;
    
    /// <summary>
    /// Check if a player is managed by another server.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool IsManagedByAnOtherServer(this TmPlayerInfo player) => (player.Flags/10000) % 10 == 1;
    
    /// <summary>
    /// Check if this player is the server itself.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool IsServer(this TmPlayerInfo player) => (player.Flags/100000) % 10 == 1;
    
    /// <summary>
    /// Check if this player has a player slot and can play.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool HasPlayerSlot(this TmPlayerInfo player) => (player.Flags/1000000) % 10 == 1;
    
    /// <summary>
    /// Check if this player is broadcasting.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool IsBroadcasting(this TmPlayerInfo player) => (player.Flags/10000000) % 10 == 1;
    
    /// <summary>
    /// Check if this player has joined the game or not.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static bool HasJoinedGame(this TmPlayerInfo player) => (player.Flags/100000000) % 10 == 1;

    /// <summary>
    /// Parse the player's flags and get an object of them.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    internal static IPlayerFlags GetFlags(this TmPlayerInfo player)
    {
        return new PlayerFlags
        {
            ForceSpectator = player.IsForcedSpectator(),
            ForceSpectatorSelectable = player.IsForcedSpectatorSelectable(),
            StereoDisplayMode = player.StereoDisplayMode(),
            IsManagedByAnOtherServer = player.IsManagedByAnOtherServer(),
            IsServer = player.IsServer(),
            HasPlayerSlot = player.HasPlayerSlot(),
            IsBroadcasting = player.IsBroadcasting(),
            HasJoinedGame = player.HasJoinedGame()
        };
    }

    /// <summary>
    /// Convert the player's account ID to the login format.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static string GetLogin(this IPlayer player) =>
        ConvertAccountIdToLogin(player.AccountId);
}
