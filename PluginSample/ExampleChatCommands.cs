using EvoSC.Attributes;
using EvoSC.Core.Commands.Chat;

namespace PluginSample;

public class ExampleChatCommands : ChatCommandGroup
{
    [Command("ping", "Ping the server!")]
    public Task Ping() =>
        Context.Message.ReplyAsync("Pong!");
}