using System.Data.Common;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Interfaces.Database;
using RepoDb;
using RepoDb.Interfaces;
using SqlKata;

namespace EvoSC.Common.Database.Repository;

public abstract class EvoScDbRepository<T> where T : class
{
    private readonly IDbConnectionFactory _connectionFactory;

    protected DbConnection Database => _connectionFactory.GetConnection();
    protected IDbSetting DatabaseSetting { get; private set; }
    protected IEnumerable<Field> Fields { get; private set; }

    public EvoScDbRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        DatabaseSetting = DbSettingMapper.Get(Database);
        Fields = FieldCache.Get<T>();
    }

    /// <summary>
    /// Quote an identifier based on the current database setting.
    /// </summary>
    /// <param name="identifier">The identifier to quote.</param>
    /// <returns></returns>
    protected string Quote(string identifier) =>
        $"{DatabaseSetting.OpeningQuote}{identifier}{DatabaseSetting.ClosingQuote}";

    protected CompilableQuery Query() => new(_connectionFactory.GetSqlCompiler());
    protected CompilableQuery Query(string table) => new(_connectionFactory.GetSqlCompiler());
    protected CompilableMultiQuery MultiQuery() => new CompilableMultiQuery(_connectionFactory.GetSqlCompiler());
}
