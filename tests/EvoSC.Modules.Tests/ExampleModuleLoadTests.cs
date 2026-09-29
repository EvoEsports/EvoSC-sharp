using EvoSC.Modules.Util;
using Xunit;

namespace EvoSC.Modules.Tests;

/// <summary>
/// The example module is what a third-party author starts from, so it has to be a module the
/// loader accepts: metadata the generator produced, and a directory it can be loaded from.
/// </summary>
public class ExampleModuleTests
{
    /// <summary>
    /// The example stages its module directory into its own output, which sits next to the test
    /// project in the repository. The test assembly's location ends in the configuration and target
    /// framework, which is what the example's output path is built from as well.
    /// </summary>
    private static string ExampleModuleDirectory
    {
        get
        {
            var testOutput = new DirectoryInfo(AppContext.BaseDirectory);
            var configuration = testOutput.Parent!.Name;
            var targetFramework = testOutput.Name;

            var exampleOutput = Path.GetFullPath(Path.Combine(
                testOutput.FullName, "..", "..", "..", "..", "..",
                "examples", "SimpleModule", "bin", configuration, targetFramework, "modules", "SimpleModule"));

            Assert.True(Directory.Exists(exampleOutput),
                $"The example module was not staged at: {exampleOutput}");

            return exampleOutput;
        }
    }

    [Fact]
    public void ExampleModule_Declares_Its_Metadata()
    {
        var module = ModuleInfoUtils.CreateFromDirectory(new DirectoryInfo(ExampleModuleDirectory));

        Assert.Equal("SimpleModule", module.Id);
        Assert.Equal("Simple Module", module.Name);
        Assert.Equal(new Version(1, 0, 0), module.Version);
        Assert.False(module.IsInternal);
    }

    [Fact]
    public async Task ExampleModule_Loads_Like_Any_Other_External_Module()
    {
        using var harness = new ModuleManagerHarness();

        await harness.Manager.LoadAsync(ExampleModuleDirectory);
        var module = Assert.Single(harness.Manager.GetLoadedModules());

        Assert.Equal("SimpleModule", module.ModuleInfo.Id);
        Assert.Equal(ModuleStatus.Loaded, module.Status);

        // It is an external module, so it can be unloaded again.
        await harness.Manager.UnloadAsync(module.LoadId);
        Assert.Empty(harness.Manager.GetLoadedModules());
    }
}
