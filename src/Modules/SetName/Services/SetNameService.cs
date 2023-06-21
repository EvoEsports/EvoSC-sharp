using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.SetName.Events;
using EvoSC.Modules.Official.SetName.Interfaces;
using GBX.NET.Engines.Game;

namespace EvoSC.Modules.Official.SetName.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class SetNameService : ISetNameService
{
    private readonly IServerClient _server;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerCacheService _playerCache;
    private readonly IEventManager _events;
    private readonly dynamic _locale;

    public SetNameService(IServerClient server, IPlayerRepository playerRepository, IPlayerCacheService playerCache,
        IEventManager events, ILocale locale)
    {
        _server = server;
        _playerRepository = playerRepository;
        _playerCache = playerCache;
        _events = events;
        _locale = locale;
    }

    public async Task SetNicknameAsync(IPlayer player, string newName)
    {
        if (player.NickName.Equals(newName, StringComparison.Ordinal))
        {
            await _server.ErrorMessageAsync(_locale.PlayerLanguage.DidNotChangeName, player);
            return;
        }
        
        await _playerRepository.UpdateNicknameAsync(player, newName);
        await _playerCache.UpdatePlayerAsync(player);
        
        await _server.SuccessMessageAsync(_locale.PlayerLanguage.NameSuccessfullySet(newName), player);
        await _server.InfoMessageAsync(_locale.PlayerChangedTheirName(player.NickName, newName));
        
        await _events.RaiseAsync(SetNameEvents.NicknameUpdated, new NicknameUpdatedEventArgs
        {
            Player = player,
            OldName = player.NickName,
            NewName = newName
        });
    }
}
