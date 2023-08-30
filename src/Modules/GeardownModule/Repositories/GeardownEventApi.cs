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
            .PutStringAsync("/v1/events/status");

    public Task<IEnumerable<GdParticipant>?> GetParticipants(int eventId) =>
        WithAccessToken()
            .WithJsonBody(new { eventId })
            .GetJsonAsync<IEnumerable<GdParticipant>>("/v1/events/participants");

    /* private GeardownHttpClient _client;

    public GeardownEventApi(GeardownHttpClient geardownHttpClient)
    {
        _client = geardownHttpClient;
    }

    public async Task<bool> UpdateStatus(int eventId, EventStatus statusId)
    {
        String response = "";

        try
        {
            response = await _client.Put("/v1/events/status", new[]
            {
                new KeyValuePair<string, int>("eventId", eventId),
                new KeyValuePair<string, int>("statusId", ((int)statusId))
            });
        }
        catch (Exception)
        {
            return false;
        }

        dynamic data = JObject.Parse(response);

        if (data.message)
        {
            return true;
        }

        return false;
    }

    public async Task<List<GdParticipant>> GetParticipants(int eventId)
    {
        var response = await _client.Get("/v1/events/participants", new[]
        {
            new KeyValuePair<string, string>("eventId", eventId.ToString())
        });

        return JsonConvert.DeserializeObject<List<GdParticipant>>(response);
    } */
}
