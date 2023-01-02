using System.Data;
using RepoDb;

namespace EvoSC.Common.Util.Database;

public static class QueryHelperExtensions
{
    /// <summary>
    /// Select rows from a table with a specific column value.
    /// </summary>
    /// <param name="db"></param>
    /// <param name="table">Table to select from. NOTE: not escaped</param>
    /// <param name="column">Column to compare. NOTE: not escaped</param>
    /// <param name="value">Value to compare with.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Task<IEnumerable<T>> SelectByColumnAsync<T>(this IDbConnection db, string table, string column,
        object value) where T : class
    {
        return db.QueryAsync<T>($"select * from {table} where {column}=@Value",
            new {Table = table, Column = column, Value = value});
    }
}
