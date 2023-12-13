using EvoSC.Common.Exceptions.PlayerExceptions;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.TextParsing.ValueReaders;

/// <summary>
/// Find a player online on the server from an input search pattern.
/// Best match is returned.
/// </summary>
public class OnlinePlayerReader(IPlayerManagerService playerManager) : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[] {typeof(IOnlinePlayer)};

    public async Task<object> ReadAsync(Type type, string input)
    {
        var players = await playerManager.FindOnlinePlayerAsync(input);

        if (!players.Any())
        {
            throw new PlayerNotFoundException(input, $"Failed to find player with name '{input}'.");
        }
        
        return players.First();
    }
}
