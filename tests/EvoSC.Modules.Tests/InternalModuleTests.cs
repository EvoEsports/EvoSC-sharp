using System.Reflection;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Util;

namespace EvoSC.Modules.Tests;

/// <summary>
/// The modules that ship with EvoSC are registered by id and loaded from a fixed directory, but
/// nothing else about them is special: they get their own load context, their dependencies are
/// resolved the same way, and they are protected from being unloaded or reloaded.
/// </summary>
[Collection("ModuleSystem")]
public class InternalModuleTests
{
    private const string InternalModuleId = "InternalFixtureModule";

    private static string InternalModuleDirectory => Path.Combine(
        AppContext.BaseDirectory, "modules", InternalModuleId);

    [Fact]
    public async Task Registered_Module_Is_Loaded_From_The_Internal_Directory()
    {
        using var harness = new ModuleManagerHarness();

        await harness.Manager.LoadInternalModulesAsync([InternalModuleId], harness.ModulesDirectory);

        var module = Assert.Single(harness.Manager.GetLoadedModules());
        Assert.Equal(InternalModuleId, module.ModuleInfo.Id);
        Assert.Equal(ModuleStatus.Loaded, module.Status);
    }

    [Fact]
    public async Task Registered_Module_Is_Internal()
    {
        using var harness = new ModuleManagerHarness();

        await harness.Manager.LoadInternalModulesAsync([InternalModuleId], harness.ModulesDirectory);

        var module = Assert.Single(harness.Manager.GetLoadedModules());
        Assert.True(module.ModuleInfo.IsInternal);
    }

    [Fact]
    public async Task Registered_Module_Gets_Its_Own_Load_Context()
    {
        using var harness = new ModuleManagerHarness();

        await harness.Manager.LoadInternalModulesAsync([InternalModuleId], harness.ModulesDirectory);

        var module = Assert.Single(harness.Manager.GetLoadedModules());
        Assert.NotNull(module.AsmLoadContext);
        Assert.Equal(ModuleStatus.Loaded, module.Status);
    }

    [Fact]
    public async Task Registered_Module_Cannot_Be_Unloaded()
    {
        using var harness = new ModuleManagerHarness();

        await harness.Manager.LoadInternalModulesAsync([InternalModuleId], harness.ModulesDirectory);
        var module = Assert.Single(harness.Manager.GetLoadedModules());

        await Assert.ThrowsAsync<EvoScModuleException>(
            () => harness.Manager.UnloadAsync(module.LoadId));
    }

    [Fact]
    public async Task Registered_Module_Cannot_Be_Reloaded()
    {
        using var harness = new ModuleManagerHarness();

        await harness.Manager.LoadInternalModulesAsync([InternalModuleId], harness.ModulesDirectory);
        var module = Assert.Single(harness.Manager.GetLoadedModules());

        await Assert.ThrowsAsync<EvoScModuleException>(
            () => harness.Manager.ReloadAsync(module.LoadId));
    }

    [Fact]
    public async Task Only_Registered_Modules_Are_Loaded()
    {
        using var harness = new ModuleManagerHarness();

        // The same directory also holds the external fixtures, which must be left alone.
        await harness.Manager.LoadInternalModulesAsync([InternalModuleId], harness.ModulesDirectory);

        Assert.Equal(InternalModuleId, Assert.Single(harness.Manager.GetLoadedModules()).ModuleInfo.Id);
    }

    [Fact]
    public async Task Module_That_Is_Registered_But_Not_Deployed_Is_Reported()
    {
        using var harness = new ModuleManagerHarness();

        var error = await Assert.ThrowsAsync<EvoScModuleException>(() =>
            harness.Manager.LoadInternalModulesAsync([InternalModuleId, "NotDeployedModule"], harness.ModulesDirectory));

        Assert.Contains("NotDeployedModule", error.Message);
    }

    [Fact]
    public async Task Internal_Modules_Are_Left_Out_Of_External_Discovery()
    {
        // An internal module is deployed to the default module directory, which is also scanned for
        // external modules, so discovery has to skip what the application already registered while
        // still finding the external ones next to it.
        var external = new SortedModuleCollection<IExternalModuleInfo>();
        ModuleDirectoryUtils.FindModulesFromDirectory(
            Path.Combine(AppContext.BaseDirectory, "modules"), external, [InternalModuleId]);

        var discovered = external.Select(m => m.Id).ToArray();
        Assert.DoesNotContain(InternalModuleId, discovered);
        Assert.Contains("ContractApiProviderModule", discovered);
    }

    [Fact]
    public void Module_Can_Classify_Itself_As_Internal()
    {
        // The declaration is the module's own, independent of how it was loaded, so read it from
        // the deployed module rather than from a reference to it.
        var assembly = Assembly.LoadFrom(Path.Combine(InternalModuleDirectory, $"{InternalModuleId}.dll"));
        var mainClass = assembly.GetTypes().Single(t => t.GetCustomAttribute<ModuleAttribute>() != null);

        Assert.True(mainClass.GetCustomAttribute<ModuleAttribute>()!.IsInternal);
    }

    [Fact]
    public async Task Modules_Are_Ordered_After_Their_Dependencies()
    {
        using var harness = new ModuleManagerHarness();

        await harness.LoadAndEnableAsync(harness.ProviderDirectory);
        await harness.LoadAndEnableAsync(harness.ConsumerDirectory);

        var ordered = harness.Manager.GetLoadedModulesByDependency().Select(m => m.ModuleInfo.Id).ToList();

        Assert.Equal(2, ordered.Count);
        Assert.True(
            ordered.IndexOf("ContractApiProviderModule") < ordered.IndexOf("ContractApiConsumerModule"),
            $"Expected the provider before its consumer, got: {string.Join(", ", ordered)}");
    }
}
