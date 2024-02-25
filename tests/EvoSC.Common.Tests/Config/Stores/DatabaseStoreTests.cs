using System.Collections.Generic;
using System.Threading.Tasks;
using EvoSC.Common.Config.Stores;
using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Tests.Config.Stores.TestModels;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Org.BouncyCastle.Utilities;
using Xunit;

namespace EvoSC.Common.Tests.Config.Stores;

public class DatabaseStoreTests
{
    [Fact]
    public void Can_Read_And_Write()
    {
        var repo = Substitute.For<IConfigStoreRepository>();
        var store = new DatabaseStore("", typeof(ISimpleDbConfig), repo);
        
        Assert.True(store.CanRead);
        Assert.True(store.CanWrite);
    }
    
    [Fact]
    public void Default_Settings_Created()
    {
        var repo = Substitute.For<IConfigStoreRepository>();
        var store = new DatabaseStore("", typeof(ISimpleDbConfig), repo);

        store.SetupDefaultSettingsAsync();

        repo.Received(3)
            .AddConfigOptionAsync(Arg.Any<DbConfigOption>());
    }

    [Fact]
    public void Option_Is_Read_From_Store()
    {
        var repo = Substitute.For<IConfigStoreRepository>();
        repo.GetConfigOptionsByKeyAsync("MyModule.SimpleDbConfig.MyOption")!
            .Returns(_ => Task.FromResult(new DbConfigOption
            {
                Key = "MyModule.SimpleDbConfig.MyOption",
                Value = "MyOptionValue"
            }));
        
        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo);

        var value = store.Read("MyOption");
        
        Assert.Equal("MyOptionValue", value);
    }

    [Fact]
    public void Option_Not_Found_In_Store_Throws_Exception()
    {
        var repo = Substitute.For<IConfigStoreRepository>();
        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo);

        Assert.Throws<KeyNotFoundException>(() => store.Read("Non.Existent.Key"));
    }

    [Fact]
    public void Existing_Key_Value_Is_Written_To_Store()
    {
        var repo = Substitute.For<IConfigStoreRepository>();
        repo.GetConfigOptionsByKeyAsync("MyModule.SimpleDbConfig.MyOption")!
            .Returns(_ => Task.FromResult(new DbConfigOption
            {
                Key = "MyModule.SimpleDbConfig.MyOption", Value = "MyOptionValue"
            }));

        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo);
        
        store.Write("MyOption", "Test");

        repo.Received().UpdateConfigOptionAsync(Arg.Any<DbConfigOption>());
    }

    [Fact]
    public void Nonexistent_Key_Value_Is_Written_To_Store()
    {
        var repo = Substitute.For<IConfigStoreRepository>();
        
        var store = new DatabaseStore("MyModule", typeof(ISimpleDbConfig), repo);
        
        store.Write("MyOption", "Test");

        repo.Received().AddConfigOptionAsync(Arg.Any<DbConfigOption>());
    }
}
