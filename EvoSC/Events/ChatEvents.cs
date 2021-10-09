using EvoSC.Modules.ServerConnection;

namespace EvoSC.Events
{
    public struct EventOnPlayerChat
    {
        public PlayerEntity Player;
        public string Text;
    }
}