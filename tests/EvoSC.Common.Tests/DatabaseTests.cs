using EvoSC.Common.Config.Models;
using EvoSC.Common.Database;
using Xunit;

namespace EvoSC.Common.Tests;

public class DatabaseTests
{
    [Fact]
    public void Correct_Connection_String_Returned_From_Config()
    {
        var config = new DatabaseConfig();

        var connectionString = config.GetConnectionString();

        Assert.Equal("Server=127.0.0.1;Port=3306;Database=evosc;Uid=evosc;Pwd=evosc", connectionString);
    }
}
