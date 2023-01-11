using System.Data.Common;
using SqlKata.Compilers;

namespace EvoSC.Common.Interfaces.Database;

public interface IDbConnectionFactory
{
    /// <summary>
    /// Get a database connection and create it if it doesn't exist.
    /// </summary>
    /// <returns></returns>
    public DbConnection GetConnection();

    /// <summary>
    /// Return the SQL compiler compatible with the current
    /// database connection.
    /// </summary>
    /// <returns></returns>
    public Compiler GetSqlCompiler();
}
