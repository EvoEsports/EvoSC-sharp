using System;
using System.Runtime.InteropServices;
using System.Text;
using EvoSC.Utility.Remotes;
using GameHost.V3;
using GameHost.V3.Ecs;
using GameHost.V3.Injection.Dependencies;
using NLog;

namespace EvoSC.Modules.ServerConnection
{
    public class GbxRemote : AppSystem
    {
        private ILogger _logger;
        private ILowLevelGbxRemote _lowLevel;

        public GbxRemote(Scope scope) : base(scope)
        {
            scope.Context.Register(this);
        
            Dependencies.AddRef(() => ref _logger);
            Dependencies.AddRef(() => ref _lowLevel);
        }

        protected override void OnInit()
        {
            Console.WriteLine("remote loaded " + _lowLevel.GetType());
        }

        private readonly StringBuilder _stringBuilder = new();

        /// <summary>
        /// Send a global message on the server
        /// </summary>
        /// <param name="text">The message</param>
        public void SendChatMessage(string text)
        {
            _lowLevel.ChatSendServerMessageAsync(text);
        }

        /// <summary>
        /// Send a message to player on the server
        /// </summary>
        /// <param name="text">The message</param>
        /// <param name="player">The target player</param>
        public void SendChatMessageToPlayer(string text, PlayerEntity player)
        {
            SendChatMessageToPlayers(text, MemoryMarshal.CreateSpan(ref player, 1));
        }

        /// <summary>
        /// Send a message to multiple players on the server
        /// </summary>
        /// <param name="text">The message</param>
        /// <param name="players">The targeted players</param>
        public void SendChatMessageToPlayers(string text, ReadOnlySpan<PlayerEntity> players)
        {
            if (players.Length == 0)
            {
                _logger.Warn("SendChatMessageToPlayers had an empty players array.");
                return;
            }
            else if (players.Length == 1)
            {
                _lowLevel.ChatSendServerMessageToLoginAsync(text, players[0].Login);
                return;
            }

            _stringBuilder.Clear();
            for (var i = 0; i < players.Length; i++)
            {
                _stringBuilder.Append(players[i].Login);
                if (i + 1 < players.Length)
                    _stringBuilder.Append(',');
            }

            _lowLevel.ChatSendServerMessageToLoginAsync(text, _stringBuilder.ToString());
        }
    }
}
