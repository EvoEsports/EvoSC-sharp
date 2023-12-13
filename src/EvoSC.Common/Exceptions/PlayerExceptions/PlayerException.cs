namespace EvoSC.Common.Exceptions.PlayerExceptions;

public class PlayerException(string accountId, string message) : EvoSCException(message)
{
    public string AccountId { get; set; } = accountId;

    public PlayerException(string accountId) : this(accountId, $"An exception occured for player '{accountId}'")
    {
    }
}
