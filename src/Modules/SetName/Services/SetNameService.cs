using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.SetName.Events;
using EvoSC.Modules.Official.SetName.Interfaces;
using LinqToDB.Reflection;

namespace EvoSC.Modules.Official.SetName.Services;

[Service(LifeStyle = ServiceLifeStyle.Scoped)]
public class SetNameService(IChatService chat, IPlayerRepository playerRepository, IPlayerCacheService playerCache,
        IEventManager events, Locale locale)
    : ISetNameService
{
    private readonly dynamic _locale = locale;

    public async Task SetNicknameAsync(IPlayer player, string newName)
    {
        newName = newName.Trim().Replace(System.Environment.NewLine, " ");
        
        if (player.NickName.Equals(newName, StringComparison.Ordinal))
        {
            await chat.ErrorMessageAsync(_locale.PlayerLanguage.DidNotChangeName, player);
            return;
        }

        switch (newName.Length)
        {
            case 0:
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.NewNameTooShort, player);
                return;
            case > 38:
                await chat.ErrorMessageAsync(_locale.PlayerLanguage.NewNameTooLong, player);
                return;
        }
        
        await playerRepository.UpdateNicknameAsync(player, newName);
        await playerCache.UpdatePlayerAsync(player);
        
        await chat.SuccessMessageAsync(_locale.PlayerLanguage.NameSuccessfullySet(newName), player);
        await chat.InfoMessageAsync(_locale.PlayerChangedTheirName(player.NickName, newName));
        
        await events.RaiseAsync(SetNameEvents.NicknameUpdated, new NicknameUpdatedEventArgs
        {
            Player = player,
            OldName = player.NickName,
            NewName = newName
        });
    }
}
