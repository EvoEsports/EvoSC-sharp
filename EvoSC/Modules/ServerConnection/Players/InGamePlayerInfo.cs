namespace EvoSC.Modules.ServerConnection
{
    // TODO: Extract most of the variables into other components
// This component is really big, and most variables have more sense to be in other new components
    public readonly struct InGamePlayerInfo
    {
        // TODO: nickname should be its own component
        public readonly string NickName;
        public readonly int TeamId;
        public readonly int LadderRanking;
        // TODO: SpectatorStatus should be its own component
        public readonly int SpectatorStatus;
        // TODO: Flags should be its own component
        public readonly int Flags;

        public InGamePlayerInfo(string nickName, int teamId, int ladderRanking, int spectatorStatus, int flags)
        {
            NickName = nickName;
            TeamId = teamId;
            LadderRanking = ladderRanking;
            SpectatorStatus = spectatorStatus;
            Flags = flags;
        }
    }
}