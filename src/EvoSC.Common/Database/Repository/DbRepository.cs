using EvoSC.Common.Interfaces.Database;
using LinqToDB;

namespace EvoSC.Common.Database.Repository;

public class DbRepository
{
    private readonly IDbConnectionFactory _dbConnFactory;

    protected DataContext Database => _dbConnFactory.GetConnection();
    
    protected DbRepository(IDbConnectionFactory dbConnFactory)
    {
        _dbConnFactory = dbConnFactory;
    }
    
    /// <summary>
    /// Get a table for querying.
    /// </summary>
    /// <typeparam name="T">The table to begin an operation on.</typeparam>
    /// <returns></returns>
    protected ITable<T> Table<T>() where T : class => Database.GetTable<T>();
}

