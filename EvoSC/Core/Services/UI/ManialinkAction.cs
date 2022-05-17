using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore.Storage;
using NLog.Common;

namespace EvoSC.Core.Services.UI;

public class ManialinkAction
{
    private Func<ManialinkActionData, Task> _callback;
    private Dictionary<string, object> _values;
    public ManialinkActionData Payload;


    public ManialinkAction(Func<ManialinkActionData, Task> callback)
    {
        var UId = new UniqueId().ToString().Replace("urn:uuid:", "").Replace("-", "");
        Payload = new ManialinkActionData() {UId = UId};
        _callback = callback;
    }

    public async Task TriggerManialinkEvent(Dictionary<string, object> values)
    {
        Payload.Values = values; 
        await _callback(Payload);
        return;
    }

    public override string ToString()
    {
        return Payload.UId;
    }
}
