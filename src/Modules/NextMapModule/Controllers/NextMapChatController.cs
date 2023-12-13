using EvoSC.Commands.Attributes;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Modules.Official.NextMapModule.Interfaces;

namespace EvoSC.Modules.Official.NextMapModule.Controllers;

[Controller]
public class NextMapChatController(INextMapService nextMapService, IServerClient server, Locale locale)
    : EvoScController<ICommandInteractionContext>
{
    private readonly dynamic _locale = locale;

    [ChatCommand("nextmap", "[Command.NextMap]")]
    public async Task GetNextMapAsync()
    {
        var nextMap = await nextMapService.GetNextMapAsync();
        await server.InfoMessageAsync(_locale.PlayerLanguage.NextMap(nextMap.Name, nextMap.Author?.NickName));
    }
}
