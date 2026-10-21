using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.GroupsModule.Interfaces;
using EvoSC.Modules.Official.GroupsModule.Models;

namespace EvoSC.Modules.Official.GroupsModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class GroupRolesService(IGroupsRepository repository) : IGroupRolesService
{
    public async Task<IReadOnlyDictionary<string, PlayerRole>> GetRolesAsync(IEnumerable<IPlayer> players)
    {
        var playerList = players.ToList();
        var shownIds = await repository.GetScoreboardGroupIdsAsync();

        if (playerList.Count == 0 || shownIds.Count == 0)
        {
            return new Dictionary<string, PlayerRole>();
        }

        var shownGroups = (await repository.GetGroupsAsync())
            .Where(g => shownIds.Contains(g.Id))
            .ToDictionary(g => g.Id);
        var groupIds = await repository.GetPlayersGroupIdsAsync(playerList.Select(p => p.Id));
        var roles = new Dictionary<string, PlayerRole>();

        foreach (var player in playerList)
        {
            var roleGroupId = groupIds.GetValueOrDefault(player.Id, [])
                .Where(shownGroups.ContainsKey)
                .Order()
                .Cast<int?>()
                .FirstOrDefault();

            if (roleGroupId == null)
            {
                continue;
            }

            var group = shownGroups[roleGroupId.Value];
            roles[player.AccountId] = new PlayerRole(group.Id, group.Title, group.Icon ?? "",
                group.Color ?? GroupsService.DefaultColor);
        }

        return roles;
    }
}
