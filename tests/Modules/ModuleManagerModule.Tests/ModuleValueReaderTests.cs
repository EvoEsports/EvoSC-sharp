using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;
using Moq;

namespace EvoSC.Modules.Official.ModuleManagerModule.Tests;

public class ModuleValueReaderTests
{
    [Fact]
    public async Task Finds_Loaded_Module()
    {
        var testModule = TestModuleLoader.CreateModule("MyTestModule");

        var moduleManager = new Mock<IModuleManager>();
        moduleManager.Setup(mm => mm.GetLoadedModules()).Returns(new[] {testModule});

        var valueReader = new ModuleValueReader(moduleManager.Object);

        var foundModule = await valueReader.ReadAsync(typeof(IModuleLoadContext), "MyTestModule") as IModuleLoadContext;

        Assert.NotNull(foundModule);
        Assert.Equal(testModule.LoadId, foundModule.LoadId);
    }

    [Fact]
    public async Task Fails_On_Nonexistent_Module()
    {
        var moduleManager = new Mock<IModuleManager>();
        moduleManager.Setup(mm => mm.GetLoadedModules()).Returns(Array.Empty<IModuleLoadContext>());
        var valueReader = new ModuleValueReader(moduleManager.Object);

        await Assert.ThrowsAsync<ValueConversionException>(() =>
            valueReader.ReadAsync(typeof(IModuleLoadContext), "DoesNotExist"));
    }
}
