using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Modules.Evo.GeardownModule.Models.API;

namespace EvoSC.Modules.Evo.GeardownModule.Interfaces;

public interface IGeardownGroupApi
{
    public Task<IEnumerable<GdParticipant>?> GetParticipantsAsync(int groupId);
    public Task<GdGroup?> CreateGroupAsync(string name, int eventId, bool isTypeTree);
    public Task<GdGroup?> UpdateGroupParticipantsAsync(int groupId, IEnumerable<GdParticipant> participants);
}