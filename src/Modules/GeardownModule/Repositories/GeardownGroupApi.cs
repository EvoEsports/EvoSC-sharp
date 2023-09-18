using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Modules.Evo.GeardownModule.Interfaces;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using Hawf.Attributes;
using Hawf.Client;

namespace EvoSC.Modules.Evo.GeardownModule.Repositories;

[ApiClient]
public class GeardownGroupApi : GeardownApiBase<GeardownGroupApi>, IGeardownGroupApi
{
    private readonly IGeardownSettings _settings;
    
    public GeardownGroupApi(IGeardownSettings settings) : base(settings)
    {
    }
    
    public Task<IEnumerable<GdParticipant>?> GetParticipantsAsync(int groupId) =>
        WithAccessToken()
            .WithJsonBody(new { groupId })
            .GetJsonAsync<IEnumerable<GdParticipant>>("/v1/groups/participants");


    public Task<GdGroup?> CreateGroupAsync(string name, int eventId, bool isTypeTree) =>
        WithAccessToken()
            .WithJsonBody(new { name, eventId, isTypeTree })
            .PostJsonAsync<GdGroup>("/v1/groups/create");

    public Task<GdGroup?> UpdateGroupParticipantsAsync(int groupId, IEnumerable<GdParticipant> participants) =>
        WithAccessToken()
            .WithJsonBody(new { groupId, participants })
            .PutJsonAsync<GdGroup>("/v1/groups/participants");
}
