using System;
using DefaultEcs;
using EvoSC.ServerConnection;

namespace EvoSC.ChatCommand
{
    public delegate void ChatCommandInvoked(PlayerEntity playerEntity, Span<Entity> arguments);
}
