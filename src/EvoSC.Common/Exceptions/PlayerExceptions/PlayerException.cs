namespace EvoSC.Common.Exceptions.PlayerExceptions;

public class PlayerException : EvoSCException
{
    public string AccountId { get; set; }
    
    public PlayerException(string accountId) : base($"An exception occured for player '{accountId}'")
    {
        AccountId = accountId;
    }
    
    public PlayerException(string accountId, string message) : base(message)
    {
        AccountId = accountId;
    }
}
