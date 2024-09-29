using System.Runtime.Serialization;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Models
{
    public enum GameMode
    {
        [EnumMember(Value = "rounds")]
        Rounds,
        [EnumMember(Value = "cup")]
        Cup,
        [EnumMember(Value = "time_attack")]
        TimeAttack,
        [EnumMember(Value = "knockout")]
        Knockout,
        [EnumMember(Value = "laps")]
        Laps,
        [EnumMember(Value = "team")]
        Team
    }
}
