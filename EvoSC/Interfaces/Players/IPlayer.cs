namespace EvoSC.Interfaces.Players;

public interface IPlayer
{
    public string Login { get; }
    public string Name { get; }

    /// <summary>
    /// Check if a user has the required permission.
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    public bool HasPermission(string requiredPermission);
}
