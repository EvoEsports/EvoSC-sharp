using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Interfaces.Services;

public interface IPermissionService
{
    public Task<bool> HasPermissionAsync(IPlayer player, string permission);
}
