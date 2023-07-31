using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Tests.Config.Stores.TestModels;
using Moq;
using Xunit;

namespace EvoSC.Common.Tests.Config.Stores;

public class DatabaseStoreTests
{
    [Fact]
    public void Can_Read_And_Write()
    {
        var repo = new Mock<IConfigStoreRepository>();
        var store = new DatabaseStore("", typeof(ISimpleDbConfig), repo.Object);
        
        Assert.True(store.CanRead);
        Assert.True(store.CanWrite);
    }
    
    [Fact]
    public void Default_Settings_Created()
    {
        var repo = new Mock<IConfigStoreRepository>();
        var store = new DatabaseStore("", typeof(ISimpleDbConfig), repo.Object);

        store.SetupDefaultSettingsAsync();

        repo.Verify(c => c
                .AddConfigOptionAsync(It.IsAny<DbConfigOption>())
            , Times.Exactly(3));
    }

    [Fact]
    public void Option_Is_Read_From_Store()
    {
        var repo = new Mock<IConfigStoreRepository>();
        repo.Setup(c => c.GetConfigOptionsByKeyAsync("MyModule.SimpleDbConfig.MyOption"))
            .Returns(() => Task.FromResult(new DbConfigOption
            {
                Key = "MyModule.SimpleDbConfig.MyOption",
                Value = "MyOptionValue"
            }));
        
        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo.Object);

        var value = store.Read("MyOption");
        
        Assert.Equal("MyOptionValue", value);
    }

    [Fact]
    public void Option_Not_Found_In_Store_Throws_Exception()
    {
        var repo = new Mock<IConfigStoreRepository>();
        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo.Object);

        Assert.Throws<KeyNotFoundException>(() => store.Read("Non.Existent.Key"));
    }

    [Fact]
    public void Existing_Key_Value_Is_Written_To_Store()
    {
        var repo = new Mock<IConfigStoreRepository>();
        repo.Setup(c => c.GetConfigOptionsByKeyAsync("MyModule.SimpleDbConfig.MyOption"))
            .Returns(() => Task.FromResult(new DbConfigOption
            {
                Key = "MyModule.SimpleDbConfig.MyOption", Value = "MyOptionValue"
            }));

        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo.Object);
        
        store.Write("MyOption", "Test");

        repo.Verify(c => c.UpdateConfigOptionAsync(It.IsAny<DbConfigOption>()));
    }

    [Fact]
    public void Nonexistent_Key_Value_Is_Written_To_Store()
    {
        var repo = new Mock<IConfigStoreRepository>();
        
        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo.Object);
        
        store.Write("MyOption", "Test");

        repo.Verify(c => c.AddConfigOptionAsync(It.IsAny<DbConfigOption>()));
    }
}
