
using EvoSC.Common.Interfaces.Database;
using LinqToDB;
using LinqToDB.Data;

namespace EvoSC.Common.Database.Repository;

public class DbRepository
{
    private readonly DataConnection _db;

    protected DataConnection Database => _db;
    
    protected DbRepository(IDbConnectionFactory dbConnFactory)
    {
        _db = dbConnFactory.GetConnection();
    }
    
    /// <summary>
    /// Get a table for querying.
    /// </summary>
    /// <typeparam name="T">The table to begin an operation on.</typeparam>
    /// <returns></returns>
    protected ITable<T> Table<T>() where T : class => Database.GetTable<T>();
}

