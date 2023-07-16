namespace EvoSC.Modules.Official.MotdModule.Models;

public class ResponseData
{
    public int id { get; set; }
    public string message { get; set; }
    public string server { get; set; }
}

public class MotdResponse
{
    public List<ResponseData> data { get; set; }
}
