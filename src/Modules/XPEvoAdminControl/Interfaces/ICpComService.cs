using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Interfaces;

public interface ICpComService
{
    public Task RespondAsync(ICpAction action, Guid actionId);
    public Task UpdateAsync(ICpAction action);
    public Task RespondErrorAsync(string message, Guid actionId);
    public Task RespondSuccessAsync(Guid actionId);
}
