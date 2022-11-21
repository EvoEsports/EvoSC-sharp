namespace EvoSC.Common.Exceptions.PlayerExceptions;

public class PlayerNotFoundException : PlayerException
{
    public PlayerNotFoundException(string accountId) : base(accountId, $"Cannot find player '{accountId}'")
    {
    }
    
    public PlayerNotFoundException(string accountId, string message) : base(accountId, message)
    {
    }
}
