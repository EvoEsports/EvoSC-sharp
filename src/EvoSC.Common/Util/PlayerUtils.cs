using System.Text;
using System.Text.RegularExpressions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Models.Enums;
using EvoSC.Common.Models.Players;
using GbxRemoteNet.Structs;

namespace EvoSC.Common.Util;

public static class PlayerUtils
{
    private const string AccountIdRegex =
        "^[0-9a-fA-F]{8}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{4}\b-[0-9a-fA-F]{12}$";

    public static readonly IPlayer NadeoPlayer = new Player
    {
        Id = 1,
        AccountId = "Nadeo",
        NickName = "Nadeo",
        UbisoftName = "Nadeo",
        Zone = null
    };

    /// <summary>
    /// Check whether this player is a Nadeo placeholder player.
    /// This means that the the login, account id and name is all
    /// set to "Nadeo".
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool IsNadeoPlaceholder(this IPlayer player) =>
        player.AccountId.Equals(NadeoPlayer.AccountId, StringComparison.Ordinal);

    /// <summary>
    /// Convert a Trackmania player's account ID to the old "login" format.
    /// </summary>
    /// <param name="accountId">The account ID format of the user.</param>
    /// <returns></returns>
    public static string ConvertAccountIdToLogin(string accountId)
    {
        if (accountId.Equals(NadeoPlayer.AccountId, StringComparison.Ordinal))
        {
            return NadeoPlayer.AccountId;
        }

        if (accountId.StartsWith("*fakeplayer", StringComparison.Ordinal))
        {
            return accountId;
        }
        
        var bytes = Convert.FromHexString(accountId.Replace("-", ""));

        return Convert.ToBase64String(bytes)
            .Replace("=", "", StringComparison.Ordinal)
            .Replace("+", "-", StringComparison.Ordinal)
            .Replace("/", "_", StringComparison.Ordinal);
    }

    /// <summary>
    /// Convert a Trackmania player's old "login" format to the account ID format.
    /// </summary>
    /// <param name="login">Login format of the user.</param>
    /// <returns></returns>
    public static string ConvertLoginToAccountId(string login)
    {
        if (login.Equals(NadeoPlayer.AccountId, StringComparison.Ordinal))
        {
            return NadeoPlayer.AccountId;
        }

        if (login.StartsWith("*fakeplayer", StringComparison.Ordinal))
        {
            return login;
        }
        
        var base64 = login
            .Replace("-", "+", StringComparison.Ordinal)
            .Replace("_", "/", StringComparison.Ordinal);

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
    /// Checks the given login against a regex to determine if it's an accountId.
    /// </summary>
    /// <param name="login">The player's login.</param>
    /// <returns></returns>
    public static bool IsAccountId(string login)
    {
        return Regex.IsMatch(login, AccountIdRegex, RegexOptions.None, TimeSpan.FromMilliseconds(100));
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
    public static bool IsForcedSpectator(this TmPlayerInfo player) => (uint)player.Flags % 10 == 1 || player.IsForcedSpectatorSelectable();
    
    /// <summary>
    /// Check if the player is forced into spectator but they can chose to select out of it.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool IsForcedSpectatorSelectable(this TmPlayerInfo player) => (uint)player.Flags % 10 == 2;
    
    /// <summary>
    /// Get the player's stereo display mode.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static int StereoDisplayMode(this TmPlayerInfo player) => (player.Flags / 1000) % 10;
    
    /// <summary>
    /// Check if a player is managed by another server.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool IsManagedByAnOtherServer(this TmPlayerInfo player) => ((uint)player.Flags/10000) % 10 == 1;
    
    /// <summary>
    /// Check if this player is the server itself.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool IsServer(this TmPlayerInfo player) => ((uint)player.Flags/100000) % 10 == 1;
    
    /// <summary>
    /// Check if this player has a player slot and can play.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool HasPlayerSlot(this TmPlayerInfo player) => ((uint)player.Flags/1000000) % 10 == 1;
    
    /// <summary>
    /// Check if this player is broadcasting.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool IsBroadcasting(this TmPlayerInfo player) => ((uint)player.Flags/10000000) % 10 == 1;
    
    /// <summary>
    /// Check if this player has joined the game or not.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool HasJoinedGame(this TmPlayerInfo player) => ((uint)player.Flags/100000000) % 10 == 1;

    /// <summary>
    /// Parse the player's flags and get an object of them.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static IPlayerFlags GetFlags(this TmPlayerInfo player)
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
