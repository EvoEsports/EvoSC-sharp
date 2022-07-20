using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Chat;
using EvoSC.Core.Commands.Generic.Attributes;

namespace EvoSC.Core.Modules.Info;

public class Commands : ChatCommandGroup
{
    [Command("version", "Show controller version.")]
    public Task Version()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        return Context.Message.ReplyAsync($"$5b3EvoSC Version: {version}");
    }
}
