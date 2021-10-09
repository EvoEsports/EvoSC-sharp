using System;
using DefaultEcs;
using EvoSC.Modules.ServerConnection;

namespace EvoSC.Modules.ChatCommand.Components
{
    public delegate void ChatCommandInvoked(PlayerEntity playerEntity, Span<Entity> arguments);
}