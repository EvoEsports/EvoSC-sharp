using System;

namespace EvoSC.ServerConnection
{
    public readonly struct PlayerLogin : IEquatable<PlayerLogin>
    {
        public readonly string Value;

        public PlayerLogin(string login)
        {
            Value = login;
        }

        public bool Equals(PlayerLogin other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerLogin other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }
    }
}
