using EvoSC.Common.Util.MatchSettings;

namespace EvoSC.Modules.Evo.GeardownModule.Util;

public static class MatchSettingsTypeUtils
{
    private static Dictionary<string, Type> s_settingsTypeMap = new()
    {
        {"S_ChatTime", typeof(int)},
        {"S_UseClublinks", typeof(bool)},
        {"S_UseClublinksSponsors", typeof(bool)},
        {"S_NeutralEmblemUrl", typeof(string)},
        {"S_IsChannelServer", typeof(bool)},
        {"S_AllowRespawn", typeof(bool)},
        {"S_RespawnBehaviour", typeof(int)},
        {"S_HideOpponents", typeof(bool)},
        {"S_PointsLimit", typeof(int)},
        {"S_FinishTimeout", typeof(int)},
        {"S_UseAlternateRules", typeof(bool)},
        {"S_ForceLapsNb", typeof(int)},
        {"S_DisplayTimeDiff", typeof(bool)},
        {"S_PointsRepartition", typeof(string)},
        {"S_RoundsPerMap", typeof(int)},
        {"S_NbOfWinners", typeof(int)},
        {"S_WarmUpNb", typeof(int)},
        {"S_WarmUpDuration", typeof(int)},
        {"S_TimeLimit", typeof(int)},
        {"S_DisableGiveUp", typeof(bool)},
        {"S_MapsPerMatch", typeof(int)},
        {"S_UseTieBreak", typeof(bool)},
        {"S_MaxPointsPerRound", typeof(int)},
        {"S_PointsGap", typeof(int)},
        {"S_UseCustomPointsRepartition", typeof(bool)},
        {"S_CumulatePoints", typeof(bool)}
    };

    public static Task<object?> ConvertToCorrectType(string name, string? value) =>
        MatchSettingsMapper.ToValueTypeAsync(s_settingsTypeMap[name], value);
}
