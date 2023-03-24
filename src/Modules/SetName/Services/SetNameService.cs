using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.SetName.Events;
using EvoSC.Modules.Official.SetName.Interfaces;

namespace EvoSC.Modules.Official.SetName.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class SetNameService : ISetNameService
{
    private readonly IServerClient _server;
    private readonly IPlayerRepository _playerRepository;
    private readonly IPlayerCacheService _playerCache;
    private readonly IEventManager _events;

    public SetNameService(IServerClient server, IPlayerRepository playerRepository, IPlayerCacheService playerCache, IEventManager events)
    {
        _server = server;
        _playerRepository = playerRepository;
        _playerCache = playerCache;
        _events = events;
    }

    public async Task SetNicknameAsync(IPlayer player, string newName)
    {
        if (player.NickName.Equals(newName, StringComparison.Ordinal))
        {
            await _server.ErrorMessageAsync("Did not change the name as it equals the old.");
            return;
        }
        
        await _playerRepository.UpdateNicknameAsync(player, newName);
        await _playerCache.UpdatePlayerAsync(player);
        await _server.SuccessMessageAsync($"Name successfully set!", player);
        await _server.InfoMessageAsync($"{player.NickName} changed their name to {newName}");
        await _events.RaiseAsync(SetNameEvents.NicknameUpdated, new NicknameUpdatedEventArgs
        {
            Player = player,
            OldName = player.NickName,
            NewName = newName
        });
    }
}
