using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace EvoSC.Core.Services.UI;

public class ManialinkAction
{
    private readonly Func<ManialinkActionData, Task> _callback;
    public readonly ManialinkActionData Payload;

    public ManialinkAction(Func<ManialinkActionData, Task> callback)
    {
        var uId = new UniqueId().ToString().Replace("urn:uuid:", string.Empty).Replace("-", string.Empty);
        Payload = new ManialinkActionData() { UId = uId };
        _callback = callback;
    }

    public async Task TriggerManialinkAction(Dictionary<string, object> values)
    {
        Payload.Values = values;
        await _callback(Payload);
    }

    public override string ToString()
    {
        return Payload.UId;
    }
}
