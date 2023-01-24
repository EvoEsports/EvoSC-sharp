using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Extensions;
using Xunit;

namespace EvoSC.Common.Tests.Database;

public class DatabaseConfigTests
{
    class DbConfig : IDatabaseConfig
    {
        public IDatabaseConfig.DatabaseType Type => IDatabaseConfig.DatabaseType.MySql;
        public string Host => "127.0.0.1";
        public int Port => 3306;
        public string Name => "evosc";
        public string Username => "evosc";
        public string Password => "evosc";
        public string TablePrefix => "";
    }
    
    [Fact]
    public void Correct_Connection_String_Returned_From_Config()
    {
        var config = new DbConfig();

        var connectionString = config.GetConnectionString();

        Assert.Equal("Server=127.0.0.1;Port=3306;Database=evosc;Uid=evosc;Pwd=evosc", connectionString);
    }
}
