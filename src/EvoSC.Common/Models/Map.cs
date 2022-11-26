using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Common.Models;

public class Map: IMap
{
    public string Uid { get; set; }
    public string Name { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int MxId { get; set; }
    public int TmIoId { get; set; }
    public DateTime? MxVersion { get; set; }
    public DateTime? TmIoVersion { get; set; }
}
