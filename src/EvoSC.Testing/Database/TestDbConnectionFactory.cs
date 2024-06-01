using EvoSC.Common.Interfaces.Database;
using LinqToDB;

namespace EvoSC.Testing.Database;

public class TestDbConnectionFactory : IDbConnectionFactory
{
    private DataContext? _dbConnection;
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
    
    public DataContext GetConnection()
    {
        if (_dbConnection == null)
        {
            var dbOptions = new DataOptions()
                .UseSQLite(ConnectionString);

            _dbConnection = new DataContext(dbOptions);
        }

        return _dbConnection;
    }
}
