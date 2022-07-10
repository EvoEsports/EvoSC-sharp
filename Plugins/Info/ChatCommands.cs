using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;
using EvoSC.Core.Helpers;
using EvoSC.Core.Services.UI;
using Newtonsoft.Json.Serialization;

namespace Info;

public class ChatCommands : ChatCommandGroup
{
    private readonly UiService _uiService;
    
    public ChatCommands(UiService uiService)
    {
        _uiService = uiService;
    }
    
    [Command("version", "Controller version.")]
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

    [Command("help", "Show information and help on controller usage.")]
    public Task Help()
    {
        throw new NotImplementedException();
    }
}