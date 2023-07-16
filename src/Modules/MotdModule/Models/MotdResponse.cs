namespace EvoSC.Modules.Official.MotdModule.Models;

public class Data
{
    public int id { get; }
    public string message { get; }
    public string server { get; }
}

public class MotdResponse
{
    public List<Data> data { get; }
}
