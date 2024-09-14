using System.Drawing;
using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Util.ServerUtils;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Modules.Official.MapQueueModule.Interfaces;

namespace EvoSC.Modules.Official.MapQueueModule.Controllers;

[Controller]
public class QueueCommandsController(IMapQueueService mapQueue, IMapService maps) : EvoScController<ICommandInteractionContext>
{
    [ChatCommand("queue", "Queue a map to be played.")]
    public async Task QueueAsync(int id)
    {
        var map = await maps.GetMapByIdAsync(id);
        await mapQueue.EnqueueAsync(map);
        await Context.Chat.SuccessMessageAsync($"$<{Context.Player.NickName}$> queued map $<{map.Name}$>.");
    }

    [ChatCommand("queuelist", "List currently queued maps.")]
    public async Task QueueListAsync()
    {
        var message = new TextFormatter();
        var mapList = mapQueue.QueuedMaps
            .Select(m => new FormattedText(m.Name)
                .AsNotIsolated()
                .WithStyle(style => style.WithColor(Color.Gray))
            );

        message.AddText(text => text.WithText("Queued Maps: "));
        
        foreach (var map in mapList)
        {
            message.AddText(map);
            message.AddText(", ");
        }

        await Context.Chat.SendChatMessageAsync(message, Context.Player);
    }
}
