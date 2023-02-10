namespace EvoSC.Common.Interfaces.Models.Audit;

public interface IAuditAction
{
    public string Id { get; }
    public dynamic? Data { get; }
}
