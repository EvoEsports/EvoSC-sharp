using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Modules.Official.Maps.Interfaces;

namespace EvoSC.Modules.Official.Maps.Controllers;

[Controller]
public class NextMapChatController : EvoScController<ICommandInteractionContext>
{
    private readonly INextMapService _nextMapService;
    private readonly IServerClient _server;
    private readonly dynamic _locale;

    public NextMapChatController(INextMapService nextMapService, IServerClient server, Locale locale)
    {
        _nextMapService = nextMapService;
        _server = server;
        _locale = locale;
    }
    
    [ChatCommand("nextmap", "[Command.NextMap]")]
    public async Task GetNextMapAsync()
    {
        var nextMap = await _nextMapService.GetNextMapAsync();
        await _server.InfoMessageAsync(_locale.PlayerLanguage.NextMap(nextMap.Name, nextMap.Author?.NickName));
    }
}
