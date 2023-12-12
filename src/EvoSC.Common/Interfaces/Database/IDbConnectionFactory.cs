using LinqToDB;

namespace EvoSC.Common.Interfaces.Database;

public interface IDbConnectionFactory
{
    /// <summary>
    /// Get a database connection and create it if it doesn't exist.
    /// </summary>
    /// <returns></returns>
    public DataContext GetConnection();
}
