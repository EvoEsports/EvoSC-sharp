using EvoSC.Modules.Util;

namespace EvoSC.Modules.Tests;

public class ModuleDirectoryUtilsTests
{
    private static string ModulesDirectory => Path.Combine(AppContext.BaseDirectory, "modules");

    [Fact]
    public void FindModulesFromDirectory_ReadsEveryModuleDirectory()
    {
        var modules = ModuleDirectoryUtils.FindModulesFromDirectory(ModulesDirectory).ToArray();

        Assert.Equal(
            new[] { "ContractApiConsumerModule", "ContractApiProviderModule", "CyclicModuleA", "CyclicModuleB" },
            modules.Select(m => m.Id).OrderBy(id => id, StringComparer.Ordinal));
    }

    /// <summary>
    /// Dependencies are declared by module id in info.toml, so the collection has to be keyed by
    /// id and not by the display name.
    /// </summary>
    [Fact]
    public void FindModulesFromDirectory_ResolvesDependenciesDeclaredById()
    {
        var modules = ModuleDirectoryUtils.FindModulesFromDirectory(ModulesDirectory).ToArray();

        var consumer = modules.Single(m => m.Id == "ContractApiConsumerModule");

        Assert.Equal("Contract API Consumer", consumer.Name);
        Assert.Equal("ContractApiProviderModule", consumer.Dependencies.Single().Name);
    }

    [Fact]
    public void FindModulesFromDirectory_SortsDependenciesFirst()
    {
        var ids = ModuleDirectoryUtils.FindModulesFromDirectory(ModulesDirectory).Select(m => m.Id).ToArray();

        Assert.True(
            Array.IndexOf(ids, "ContractApiProviderModule") < Array.IndexOf(ids, "ContractApiConsumerModule"),
            "A module must be ordered before the module depending on it.");
    }

    [Fact]
    public void FindModulesFromDirectory_ThrowsForUnknownDependency()
    {
        var modules = new SortedModuleCollection<Interfaces.IExternalModuleInfo>();
        modules.Add(CreateModule("OrphanModule", "MissingModule"));

        Assert.Throws<Exceptions.ModuleDependency.DependencyNotFoundException>(() => modules.ToArray());
    }

    /// <summary>
    /// Two modules may depend on each other. There is no order that satisfies both, so they are
    /// emitted in a stable order instead of being rejected.
    /// </summary>
    [Fact]
    public void SortedModuleCollection_KeepsModulesThatDependOnEachOther()
    {
        var modules = new SortedModuleCollection<Interfaces.IExternalModuleInfo>();
        modules.Add(CreateModule("CyclicModuleA", "CyclicModuleB"));
        modules.Add(CreateModule("CyclicModuleB", "CyclicModuleA"));

        var first = modules.Select(m => m.Id).ToArray();
        var second = modules.Select(m => m.Id).ToArray();

        Assert.Equal(new[] { "CyclicModuleA", "CyclicModuleB" }, first);
        Assert.Equal(first, second);
    }

    [Fact]
    public void SortedModuleCollection_OrdersCyclicModulesAfterResolvableOnes()
    {
        var modules = new SortedModuleCollection<Interfaces.IExternalModuleInfo>();
        modules.Add(CreateModule("CyclicModuleA", "CyclicModuleB"));
        modules.Add(CreateModule("CyclicModuleB", "CyclicModuleA"));
        modules.Add(CreateModule("StandaloneModule"));
        modules.Add(CreateModule("DependentModule", "StandaloneModule"));

        Assert.Equal(
            new[] { "StandaloneModule", "DependentModule", "CyclicModuleA", "CyclicModuleB" },
            modules.Select(m => m.Id));
    }

    private static Interfaces.IExternalModuleInfo CreateModule(string id, params string[] dependencies)
        => new Models.ExternalModuleInfo
        {
            Id = id,
            Name = id,
            Summary = "Test module.",
            Version = new Version(1, 0, 0),
            Author = "Evo",
            Dependencies = dependencies.Select(d => (Interfaces.IModuleDependency)new Models.ModuleDependency
            {
                Name = d,
                Version = new Version(1, 0, 0)
            }).ToArray(),
            Directory = new DirectoryInfo(ModulesDirectory),
            ModuleFiles = []
        };
}
