using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;

namespace EvoSC.Common.Services;

public class PermissionService : IPermissionService
{
    public async Task<bool> HasPermissionAsync(IPlayer player, string permission)
    {
        return false;
    }
}
