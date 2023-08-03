using System.Diagnostics.CodeAnalysis;
using Config.Net;

namespace EvoSC.Common.Config.Mapping;

public class VersionParser : ITypeParser
{
    public bool TryParse(string? value, Type t, [UnscopedRef] out object? result)
    {
        bool success = Version.TryParse(value, out var version);
        result = version;
        return success;
    }

    public string? ToRawString(object? value)
    {
        var version = value as Version;
        return version?.ToString();
    }

    public IEnumerable<Type> SupportedTypes => new[] {typeof(Version)};
}
