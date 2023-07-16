namespace EvoSC.Modules.Official.MotdModule.Models;

public class Data
{
    public int id { get; set; }
    public string message { get; set; }
    public string server { get; set; }
}

public class MotdResponse
{
    public List<Data> data { get; set; }
}
