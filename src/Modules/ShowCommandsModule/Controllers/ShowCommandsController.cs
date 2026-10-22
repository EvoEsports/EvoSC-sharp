using System.Text;
using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ShowCommandsModule.Models;

namespace EvoSC.Modules.Official.ShowCommandsModule.Controllers;

[Controller]
public class ShowCommandsController(IChatCommandManager commands, IManialinkManager manialinks)
    : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("commands", "Show a list of all available commands.")]
    public Task ShowCommandsAsync()
    {
        var commandList = commands.GetCommands()
            .Select(ToCommandInfo)
            .OrderBy(cmd => cmd.Permission != null)
            .ThenBy(cmd => cmd.Usage)
            .ToArray();

        return manialinks.SendManialinkAsync(Context.Player, "ShowCommandsModule.ShowCommandsTemplate", new { commands = commandList });
    }

    private static CommandInfo ToCommandInfo(IChatCommand cmd) => new()
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
