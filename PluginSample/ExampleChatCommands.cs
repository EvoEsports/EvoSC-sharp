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

    [Command("test", "Do some stuff")]
    public async Task Test(string someStr, int myNumber, float floatNumber)
    {
        Context.Message.ReplyAsync("Parameter 1 (string): " + someStr);
        Context.Message.ReplyAsync("Parameter 1 (int): " + myNumber);
        Context.Message.ReplyAsync("Parameter 1 (float): " + floatNumber);
    }
}
