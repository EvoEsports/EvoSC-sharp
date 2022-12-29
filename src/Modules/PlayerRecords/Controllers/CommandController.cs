using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;

namespace EvoSC.Modules.Official.PlayerRecords.Controllers;

[Controller]
public class CommandController : EvoScController<CommandInteractionContext>
{
    private readonly IPlayerRecordsService _playerRecords;
    private readonly IServerClient _server;
    
    public CommandController(IPlayerRecordsService playerRecords, IServerClient server)
    {
        _playerRecords = playerRecords;
        _server = server;
    }
    
    [ChatCommand("pb", "Show your best time on the current map.")]
    public async Task ShowPb()
    {
        var map = await _playerRecords.GetOrAddCurrentMapAsync();
        var pb = await _playerRecords.GetPlayerRecordAsync(Context.Player, map);

        if (pb == null)
        {
            await _server.InfoMessage("You don't have a time on this map yet.");
            return;
        }

        var ms = pb.Score % 1000;
        var s = pb.Score / 1000 % 60;
        var m = pb.Score / 1000 / 60;
        var formattedTime = $"{(m > 0 ? m + ":" : "")}{s:00}.{ms:000}";

        await _server.InfoMessage($"Your current pb is $<$fff{formattedTime}$>");
    }
}
