using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;

namespace PluginSample;

public class ExampleChatCommands : ChatCommandGroup
{
    [Command("ping", "Ping the server!")]
    public Task Ping() =>
        Context.Message.ReplyAsync("Pong!");

    [Command("echo", "Have the server echo something")]
    [Permission("admin")]
    public Task Echo(string message) =>
        Context.Client.ChatSendServerMessageAsync("[Server] " + message);
}
