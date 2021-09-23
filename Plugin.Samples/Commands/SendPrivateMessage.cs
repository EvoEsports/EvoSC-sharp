using DefaultEcs;
using GameHost.V3;
using EvoSC.ChatCommand;
using EvoSC.Core.Plugins;
using EvoSC.Core.Remote;
using EvoSC.ServerConnection;

namespace Plugin.Samples.SimpleCommand
{
    //
    // SendPrivateMessage:
    //
    // Create two commands via the custom injector 'ChatCommandAttribute'
    // - NoMessageArgFound /tell string:Recipient
    // - OnTellCommand /tell string:Recipient string:Message
    //
    //  NoMessageArgFound:
    //      Hidden command, which is used for when the user fail to put the message argument.
    //
    //  OnTellCommand
    //      Send a private message to another user.
    //      The sender will receive the message they sent.
    //
    // ===================================================
    //  How to test?
    // ===================================================
    //
    //  As player, start writing '/tell' in the chat,
    //  Add the recipient enclosed in ""
    //  Add your message enclosed in ""
    //  example:
    //      /tell "Guerro323" "Hello World!"
    //
    //  It should return those results:
    //      If the player isn't connected:
    //          Recipient 'Argument' not found!
    //      If the player is connected:
    //          You and the player receive the same message:
    //              'Sender Name' −> 'Message'
    //
    //  You can also send a message to yourself
    //
    
    public class SendPrivateMessage : PluginSystemBase
    {
        [Dependency] private IGbxRemote _remote;
        [Dependency] private World _world;

        public SendPrivateMessage(Scope scope) : base(scope)
        {
        }

        private EntitySet _playerSet;
        
        protected override void OnInit()
        {
            // We must send a message to a connected player on the server.
            _playerSet = _world.GetEntities()
                .With<PlayerLogin>()
                .With<InGamePlayerInfo>()
                .With<InGameConnectedPlayer>()
                .AsSet();

            Disposables.Add(_playerSet);
        }

        [ChatCommand("/tell", hidden: true)]
        private void NoMessageArgFound(PlayerEntity playerEntity)
        {
            _remote.ChatSendServerMessageToLoginAsync("You forgot to put the message!", playerEntity.Login);
        }

        [ChatCommand("/tell")]
        private void OnTellCommand(PlayerEntity playerEntity, string nickNameOrLogin, string message)
        {
            Entity otherPlayer = default;
            foreach (var player in _playerSet.GetEntities())
            {
                var login = player.Get<PlayerLogin>().Value;
                // Since we're on TM2020, should we strip color codes?
                // (I guess it would be better to make it future proof)
                // (but this is just a simple sample for now)
                if (player.Get<InGamePlayerInfo>().NickName == nickNameOrLogin)
                {
                    otherPlayer = player;
                    nickNameOrLogin = login;
                    break;
                }

                if (login == nickNameOrLogin)
                    otherPlayer = player;
            }

            if (otherPlayer == default)
            {
                _remote.ChatSendServerMessageToLoginAsync(
                    $"$999Recipient '{nickNameOrLogin}' not found!",
                    playerEntity.Login
                );
                return;
            }

            _remote.ChatSendServerMessageToLoginAsync(
                $"$<{playerEntity.Info.Result.NickName}$z$s$> $999-> $aaa{message}",
                // send to both the receiver and sender
                $"{nickNameOrLogin},{playerEntity.Login}"
            );
        }
    }
}