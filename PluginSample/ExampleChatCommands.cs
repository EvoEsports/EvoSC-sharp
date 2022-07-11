using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Core.Helpers;
using GbxRemoteNet;

namespace PluginSample;

[CommandGroup("test", "test group", "admin")]
public class ExampleChatCommands : ChatCommandGroup
{
    private readonly Manialink _manialink;

    public ExampleChatCommands(GbxRemoteClient client)
    {
        _manialink = new Manialink(client);
    }

    [Command("ping", "Ping the server!")]
    public Task Ping() =>
        Context.Message.ReplyAsync("Pong!");

    [Command("echo", "Have the server echo something")]
    public Task Echo(string message) =>
        Context.Client.ChatSendServerMessageAsync("[Server] " + message);

    [Command("test", "Do some stuff")]
    public async Task Test(string someStr, int myNumber, float floatNumber)
    {
        Context.Message.ReplyAsync("Parameter 1 (string): " + someStr);
        Context.Message.ReplyAsync("Parameter 2 (int): " + myNumber);
        Context.Message.ReplyAsync("Parameter 3 (float): " + floatNumber);
    }

    [Command("show", "Show manialink")]
    public Task Show() => _manialink.Send(Context.Player);
}