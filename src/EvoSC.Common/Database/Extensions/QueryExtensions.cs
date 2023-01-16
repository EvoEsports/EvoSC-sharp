using EvoSC.Common.Interfaces.Database;
using SqlKata;

namespace EvoSC.Common.Database.Extensions;

public static class QueryExtensions
{
    public static (string sql, Dictionary<string, object>) Compile(this Query query)
    {
        var compilableQuery = query as ICompilableQuery;

        if (compilableQuery == null)
        {
            throw new InvalidOperationException("Cannot compile a query that isn't auto-compilable.");
        }

        return compilableQuery.Compile();
    }
}
