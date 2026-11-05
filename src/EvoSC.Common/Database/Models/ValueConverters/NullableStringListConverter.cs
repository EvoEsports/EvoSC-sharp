using LinqToDB.Common;
using LinqToDB.Internal.Common;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.ValueConverters;

public class NullableStringListConverter() : ValueConverter<List<string>, string?>(
    newValue => newValue.IsNullOrEmpty() ? null : string.Join(",", newValue),
    dbValue => dbValue == null
        ? new List<string>()
        : dbValue.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList(),
    true
);
