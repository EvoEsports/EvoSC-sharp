using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Modules.Evo.GeardownModule.Interfaces.Repositories;
using EvoSC.Modules.Evo.GeardownModule.Models;
using EvoSC.Modules.Evo.GeardownModule.Models.API;
using EvoSC.Modules.Evo.GeardownModule.Settings;
using Hawf.Attributes;

namespace EvoSC.Modules.Evo.GeardownModule.Repositories;

[ApiClient]
public class GeardownEventApi : GeardownApiBase<GeardownEventApi>, IGeardownEventApi
{
    public GeardownEventApi(IGeardownSettings settings) : base(settings)
    {
    }

    public Task UpdateStatus(int eventId, EventStatus statusId) =>
        WithAccessToken()
            .WithJsonBody(new { eventId, statusId })
            .PutStringAsync("/api/v1/events/status");

    public Task<IEnumerable<GdParticipant>?> GetParticipants(int eventId) =>
        WithAccessToken()
            .WithJsonBody(new { eventId })
            .GetJsonAsync<IEnumerable<GdParticipant>>("/api/v1/events/participants");
}
