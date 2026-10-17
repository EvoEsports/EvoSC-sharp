using System.Text;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.HelpModule.Models;

namespace EvoSC.Modules.Official.HelpModule.Controllers;

[Controller]
public class HelpCommandController(IChatCommandManager commands, IManialinkManager manialinks)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("help", "Show a list of all available commands.")]
    [CommandAlias("/commands")]
    public Task ShowHelpAsync()
    {
        var commandList = commands.GetCommands()
            .Select(ToHelpCommandInfo)
            .OrderBy(cmd => cmd.Usage)
            .ToArray();

        return manialinks.SendManialinkAsync(Context.Player, "HelpModule.HelpTemplate", new { commands = commandList });
    }

    private static HelpCommandInfo ToHelpCommandInfo(IChatCommand cmd) => new()
    {
        Usage = BuildUsage(cmd), Description = cmd.Description, Permission = cmd.Permission
    };

    private static string BuildUsage(IChatCommand cmd)
    {
        var usage = new StringBuilder(cmd.UsePrefix ? ChatCommandManager.CommandPrefix : "").Append(cmd.Name);

        foreach (var param in cmd.Parameters)
        {
            usage.Append(param.Optional ? " [" : " <").Append(param.Name).Append(param.Optional ? ']' : '>');
        }

        return usage.ToString();
    }
}
