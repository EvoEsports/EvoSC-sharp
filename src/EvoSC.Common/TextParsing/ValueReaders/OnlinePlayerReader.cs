using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.TextParsing.ValueReaders;

public class OnlinePlayerReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[] {typeof(IOnlinePlayer)};

    private readonly IPlayerManagerService _playerManager;
    
    public OnlinePlayerReader(IPlayerManagerService playerManager)
    {
        _playerManager = playerManager;
    }
    
    public async Task<object> Read(Type type, string input)
    {
        var players = await _playerManager.FindOnlinePlayerAsync(input);

        if (players.Count() == 0)
        {
            throw new PlayerNotFoundException(input, $"Failed to find player with name: {input}");
        }
        
        return players.First();
    }
}
