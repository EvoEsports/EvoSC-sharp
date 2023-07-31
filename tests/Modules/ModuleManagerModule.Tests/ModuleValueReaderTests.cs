using EvoSC.Common.Exceptions.Parsing;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using EvoSC.Modules.Official.ModuleManagerModule.ValueReaders;
using Moq;

namespace EvoSC.Modules.Official.ModuleManagerModule.Tests;

public class ModuleValueReaderTests
{
    [Fact]
    public async Task Finds_Loaded_Module()
    {
        var testModule = new ModuleLoadContext
        {
            Instance = null,
            Services = null,
            AsmLoadContext = null,
            LoadId = Guid.NewGuid(),
            MainClass = null,
            ModuleInfo = new InternalModuleInfo
            {
                Name = "MyTestModule",
                Title = null,
                Summary = null,
                Version = null,
                Author = null,
                Dependencies = null,
                Assembly = null
            },
            Assemblies = null,
            Pipelines = null,
            Permissions = null,
            LoadedDependencies = null,
            ManialinkTemplates = null,
            RootNamespace = null,
            Localization = null
        };

        var moduleManager = new Mock<IModuleManager>();
        moduleManager.Setup(mm => mm.LoadedModules).Returns(new[] {testModule});
        
        var valueReader = new ModuleValueReader(moduleManager.Object);

        var foundModule = await valueReader.ReadAsync(typeof(IModuleLoadContext), "MyTestModule") as IModuleLoadContext;
        
        Assert.NotNull(foundModule);
        Assert.Equal(testModule.LoadId, foundModule.LoadId);
    }

    [Fact]
    public async Task Fails_On_Nonexistent_Module()
    {
        var moduleManager = new Mock<IModuleManager>();
        moduleManager.Setup(mm => mm.LoadedModules).Returns(Array.Empty<IModuleLoadContext>());
        var valueReader = new ModuleValueReader(moduleManager.Object);

        await Assert.ThrowsAsync<ValueConversionException>(() =>
            valueReader.ReadAsync(typeof(IModuleLoadContext), "DoesNotExist"));
    }
}
