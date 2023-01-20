using EvoSC.Common.Interfaces.Database;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace EvoSC.Common.Tests.Database;

public class TestDbConnectionFactory : IDbConnectionFactory
{
    private const string ConnectionString = "Data Source=:memory:;Version=3;New=True;";

    private DataConnection? _dbConnection;
    
    public DataConnection GetConnection()
    {
        if (_dbConnection == null)
        {
            var dbOptions = new LinqToDBConnectionOptionsBuilder()
                .UseSQLite(ConnectionString)
                .Build();

            _dbConnection = new DataConnection(dbOptions);
        }

        return _dbConnection;
    }
}
