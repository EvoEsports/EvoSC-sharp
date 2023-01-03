using System.Data.Common;

namespace EvoSC.Common.Interfaces.Database;

public interface IDbConnectionFactory
{
    public DbConnection GetConnection();
}
