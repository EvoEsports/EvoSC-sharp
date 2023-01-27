using System.IO;
using EvoSC.Common.Interfaces.Database;
using LinqToDB.Configuration;
using LinqToDB.Data;

namespace EvoSC.Common.Tests.Database;

public class TestDbConnectionFactory : IDbConnectionFactory
{
    private DataConnection? _dbConnection;
    private readonly string _connectionString;

    public string ConnectionString => _connectionString;

    public TestDbConnectionFactory(string identifier)
    {
        var filePath = Path.GetFullPath(identifier);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        _connectionString = $"Data Source={identifier};";
    }
    
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
