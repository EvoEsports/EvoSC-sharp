using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Arguments;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Events.CoreEvents;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Official.Player.Interfaces;

namespace EvoSC.Modules.Official.Player.Controllers;

[Controller]
public class PlayerEventController(IPlayerService playerService, Locale locale, IChatService chat) : EvoScController<IEventControllerContext>
{
    private readonly dynamic _locale = locale;
    
    [Subscribe(PlayerEvents.PlayerJoined)]
    public async Task OnPlayerJoinedAsync(object sender, PlayerJoinedEventArgs args)
    {
        if (args.IsNewPlayer)
        {
            await chat.InfoMessageAsync(_locale.PlayerFirstJoined(args.Player.NickName));
            return;
        }
        
        await playerService.GreetPlayerAsync(args.Player);
    }

    [Subscribe(PlayerEvents.NewPlayerAdded)]
    public Task OnNewPlayerAddedAsync(object sender, NewPlayerAddedEventArgs args) =>
        playerService.SetupPlayerAsync(args.Player);
}
