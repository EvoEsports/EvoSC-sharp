using System.Collections.Concurrent;

namespace EvoSC.Modules.Official.ScoreboardModule.Utils;

public static class ScoreboardNicknameUtils
{
    /// <summary>
    /// Converts the nickname repo to a ManiaScript array.
    /// </summary>
    /// <param name="nicknameMap"></param>
    /// <returns></returns>
    public static string ToManiaScriptArray(ConcurrentDictionary<string, string> nicknameMap)
    {
        var entriesList = nicknameMap.Select(ToManiaScriptArrayEntry).ToList();
        var joinedEntries = string.Join(",\n", entriesList);

        return $"[{joinedEntries}]";
    }
    
    /// <summary>
    /// Converts an entry of the nickname repo to a ManiaScript array entry.
    /// </summary>
    /// <param name="loginNickname"></param>
    /// <returns></returns>
    public static string ToManiaScriptArrayEntry(KeyValuePair<string, string> loginNickname)
    {
        return $"\"{loginNickname.Key}\" => \"{EscapeNickname(loginNickname.Value)}\"";
    }

    /// <summary>
    /// Escapes a nickname to be safely inserted into a XMl comment.
    /// </summary>
    /// <param name="nickname"></param>
    /// <returns></returns>
    public static string EscapeNickname(string nickname)
    {
        return nickname.Replace("-->", "-\u2192", StringComparison.OrdinalIgnoreCase)
            .Replace("\"", "\\\"", StringComparison.OrdinalIgnoreCase);
    }
}
