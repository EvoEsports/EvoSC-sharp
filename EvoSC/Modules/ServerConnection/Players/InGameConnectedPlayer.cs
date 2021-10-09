using System;

namespace EvoSC.Modules.ServerConnection
{
    /// <summary>
    ///     Component that represent an entity as a player on the server
    /// </summary>
    public readonly struct InGameConnectedPlayer : IEquatable<InGameConnectedPlayer>
    {
        public readonly int Id;

        public InGameConnectedPlayer(int id)
        {
            Id = id;
        }

        public bool Equals(InGameConnectedPlayer other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is InGameConnectedPlayer other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
