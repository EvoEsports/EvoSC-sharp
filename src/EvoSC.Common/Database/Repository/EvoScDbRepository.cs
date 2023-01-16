using System.Data.Common;
using EvoSC.Common.Database.QueryHelpers;
using EvoSC.Common.Interfaces.Database;
using RepoDb;
using RepoDb.Interfaces;

namespace EvoSC.Common.Database.Repository;

public class EvoScDbRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    protected DbConnection Database => _connectionFactory.GetConnection();
    protected IDbSetting DatabaseSetting { get; }

    protected EvoScDbRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        DatabaseSetting = DbSettingMapper.Get(Database);
    }

    /// <summary>
    /// Quote an identifier based on the current database setting.
    /// </summary>
    /// <param name="identifier">The identifier to quote.</param>
    /// <returns></returns>
    protected string Quote(string identifier) =>
        $"{DatabaseSetting.OpeningQuote}{identifier}{DatabaseSetting.ClosingQuote}";

    /// <summary>
    /// Start building a new query.
    /// </summary>
    /// <returns></returns>
    protected CompilableQuery Query() => new(_connectionFactory.GetQueryCompiler());

    /// <summary>
    /// Start building an new query on a specific table.
    /// </summary>
    /// <param name="table">The table to run the query on.</param>
    /// <returns></returns>
    protected CompilableQuery Query(string table) => new(table, _connectionFactory.GetQueryCompiler());

    /// <summary>
    /// Start a new multi query. Allows you to compile multiple
    /// queries in one.
    /// </summary>
    /// <returns></returns>
    protected CompilableMultiQuery MultiQuery() => new(_connectionFactory.GetQueryCompiler());
}
