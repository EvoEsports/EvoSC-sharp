using EvoSC.Common.Interfaces.Parsing;
using EvoSC.Modules.Official.OpenPlanetControl.Interfaces.Models;
using EvoSC.Modules.Official.OpenPlanetControl.Models;

namespace EvoSC.Modules.Official.OpenPlanetControl.Parsing;

public class OpenPlanetInfoValueReader : IValueReader
{
    public IEnumerable<Type> AllowedTypes => new[] {typeof(IOpenPlanetInfo)};
    
    public Task<object> ReadAsync(Type type, string input)
    {
        return Task.FromResult((object)OpenPlanetInfo.Parse(input));
    }
}
