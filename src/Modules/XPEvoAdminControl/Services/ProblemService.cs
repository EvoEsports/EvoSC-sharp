using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class ProblemService : IProblemService
{
    private readonly HashSet<IPlayer> _playersWithProblems = new();
    private readonly object _playersWithProblemsLock = new();
    
    public void SetProblem(IPlayer player, bool isProblem)
    {
        lock (_playersWithProblemsLock)
        {
            if (isProblem)
            {
                _playersWithProblems.Add(player);
            } 
            else if (_playersWithProblems.Contains(player))
            {
                _playersWithProblems.Remove(player);
            }
        }
    }

    public IEnumerable<IPlayer> GetPlayersWithAProblem()
    {
        lock (_playersWithProblemsLock)
        {
            return _playersWithProblems.ToList();
        }
    }
}
