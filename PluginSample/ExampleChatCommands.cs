using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Core.Helpers;
using EvoSC.Interfaces.Commands;
using GbxRemoteNet;
using GbxRemoteNet.XmlRpc.ExtraTypes;

namespace PluginSample;

public class ExampleChatCommands : ChatCommandGroup
{
    private readonly Manialink _manialink;
    private readonly IChatCommandsService _commands;

    public ExampleChatCommands(GbxRemoteClient client, IChatCommandsService commands)
    {
        _manialink = new Manialink(client);
        _commands = commands;
    }

    [Command("ping", "Ping the server!")]
    public Task Ping() =>
        Context.Message.ReplyAsync("Pong!");

    [CommandGroup("test")]
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

    [Command("unregister", "Unregister all the commands.")]
    public Task Unregister() =>
        _commands.UnregisterCommands<ExampleChatCommands>();

    [Command("show", "Show manialink")]
    public Task Show() => _manialink.Send(Context.Player);

    [Command("hunt", "Enable hunt mode")]
    public async Task Hunt()
    {
        var settings = await Context.Client.GetModeScriptSettingsAsync();
        settings["S_TimeLimit"] = 0;
        await Context.Client.SetModeScriptSettingsAsync(settings);

        var msg = new ChatMessage();
        msg.SetSuccess();
        msg.SetMessage("Hunt mode enabled.");
        msg.SetIcon(Icon.Checkmark);
        await Context.Message.ReplyAsync(msg);
    }
}