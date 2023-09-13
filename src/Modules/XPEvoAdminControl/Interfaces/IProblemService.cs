using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Interfaces;

public interface IProblemService
{
    public void SetProblem(IPlayer player, bool isProblem);

    public IEnumerable<IPlayer> GetPlayersWithAProblem();
}
