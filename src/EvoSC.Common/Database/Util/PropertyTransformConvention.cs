using Dapper.FluentMap.Conventions;

namespace EvoSC.Common.Database.Util;

public class PropertyTransformConvention : Convention
{
    public PropertyTransformConvention()
    {
        Properties()
            .Configure(c => c.Transform(s =>
                {
                    char capChar = char.ToUpper(s[0]);
                    return capChar + s.Substring(1);
                }
            ));
    }
}
