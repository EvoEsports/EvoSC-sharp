using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;

public interface IGeardownEventApi
{
    public Task UpdateStatus(int eventId, EventStatus statusId);
    public Task<IEnumerable<GdParticipant>?> GetParticipants(int eventId);
}