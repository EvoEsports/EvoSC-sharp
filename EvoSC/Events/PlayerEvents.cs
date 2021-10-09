using EvoSC.Modules.ServerConnection;

namespace EvoSC.Events
{
    public struct EventOnPlayerConnect
    {
        public PlayerEntity Player;
        public bool IsSpectator;
    }

    public struct EventOnPlayerDisconnect
    {
        public PlayerEntity Player;
        public string Reason;
    }

    public struct EventOnPlayerInfoChanged
    {
        public string Login;
        public string NickName;
        public int PlayerId;
        public int TeamId;
        public int SpectatorStatus;
        public int LadderRanking;
        public int Flags;
    }
}