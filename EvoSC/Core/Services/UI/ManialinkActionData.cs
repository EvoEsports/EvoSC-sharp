using System.Collections.Generic;

namespace EvoSC.Core.Services.UI;

public class ManialinkActionData
{
    public string UId { get; set; }

    public object Data { get; set; }

    public Dictionary<string, object> Values { get; set; }
}
