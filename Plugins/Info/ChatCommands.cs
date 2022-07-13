using System.Reflection;
using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Helpers;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services.UI;
using EvoSC.Interfaces.Plugins;
using Newtonsoft.Json.Serialization;
using NLog;

namespace Info;

[Permission("test")]
public class ChatCommands : ChatCommandGroup
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    
    [CommandGroup("test")]
    [Command("cmd", "Controller version.")]
    public Task Version()
    {
        var evoscAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .First(a => a.GetName().Name == "EvoSC");

        var name = evoscAssembly.GetName();

        var message = new ChatMessage();
        message.SetIcon(Icon.Information);
        message.SetMessage($"{name.Name} v{name.Version}");
        message.SetInfo();

        return Context.Message.ReplyAsync(message.Render());
    }

    [CommandGroup("test2")]
    [Command("cmd", "sdgfadg")]
    public Task EvoSC() =>
        Context.Message.ReplyAsync("test");

    [Command("help", "Show information and help on controller usage.")]
    public async Task Help()
    {
        var pluginDir = @"E:\projects\evo\trackmania\evosc-sharp\EvoSC\bin\Debug\net6.0\plugins\Info\Templates";

        _logger.Info(pluginDir);

        var template = new TemplateEngine(pluginDir, "help.xml");

        var xml = template.Render(new
        {
            name = "help"
        });

        await Context.Client.SendDisplayManialinkPageToLoginAsync(Context.Player.Login, xml, 0, false);
    }
}