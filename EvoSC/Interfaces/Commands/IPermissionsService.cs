using System.Threading.Tasks;

namespace EvoSC.Interfaces.Commands;

public interface IPermissionsService
{
    /// <summary>
    /// Register a new permission to the database.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public Task AddPermission(string name, string description);
}
