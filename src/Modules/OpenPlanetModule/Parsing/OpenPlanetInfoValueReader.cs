using System.Web;
using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Modules.Official.OpenPlanetModule.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetModule.Models;

namespace EvoSC.Modules.Official.OpenPlanetModule.Parsing;

public class OpenPlanetInfoValueReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[] {typeof(IOpenPlanetInfo)};
    
    public Task<object> ReadAsync(Type type, string input)
    {
        return Task.FromResult((object)OpenPlanetInfo.Parse(HttpUtility.UrlDecode(input)));
    }
}
