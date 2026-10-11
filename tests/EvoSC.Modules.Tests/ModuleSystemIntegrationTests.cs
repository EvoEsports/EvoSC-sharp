using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Exceptions;
using EvoSC.Common.Util;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Exceptions;
using EvoSC.Modules.Exceptions.ModuleDependency;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ContractApiProviderModule.Contracts;
using EvoSC.Modules.Util;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Modules.Tests;

[Collection("ModuleSystem")]
public class ModuleSystemIntegrationTests
{
    private const string ProviderContractType =
        "EvoSC.Modules.Official.ContractApiProviderModule.Contracts.IScoreboardService";

    /// <summary>
    /// The test assembly compiles directly against the provider's export assembly,
    /// declared as a module dependency in the project file, exactly like a consuming
    /// module does. The exported contract types are available statically at compile time.
    /// </summary>
    [Fact]
    public void TestAssembly_CompilesAgainstProviderExportAssembly()
    {
        // Compile-time proof: these types are only visible through the provider's export assembly.
        _ = typeof(IScoreboardService);
        _ = typeof(MatchResult);

        // The export assembly is deliberately never copied next to the test assembly: at runtime it
        // is only ever loaded through the shared export store. So assert it was built and deployed
        // with the module rather than that this assembly references it.
        var exportAssembly = Path.Combine(
            AppContext.BaseDirectory, "modules", "ContractApiProviderModule", "ContractApiProviderModule.Exports.dll");

        Assert.True(File.Exists(exportAssembly), $"Expected the provider's export assembly at: {exportAssembly}");
    }

    [Fact]
    public async Task ProviderAndConsumer_ShareContractTypeIdentity()
    {
        using var harness = new ModuleManagerHarness();

        var provider = await harness.LoadAndEnableAsync(harness.ProviderDirectory);
        var consumer = await harness.LoadAndEnableAsync(harness.ConsumerDirectory);

        // The export assembly is loaded once into the shared store and referenced by both modules.
        var store = harness.Manager.ExportAssemblies;
        Assert.True(store.HasExport(provider.ModuleInfo.Id));
        var exportAssembly = store.AcquireExportForModule(consumer.LoadId, provider.ModuleInfo.Id);
        Assert.NotNull(exportAssembly);
        Assert.True(store.IsExportAssembly(exportAssembly));

        var contractType = exportAssembly!.GetType(ProviderContractType)
                           ?? throw new InvalidOperationException("Failed to locate the exported contract type in the shared store.");

        // Both module containers resolve the very same interface type and singleton instance.
        var providerService = provider.Services.GetInstance(contractType);
        var consumerService = consumer.Services.GetInstance(contractType);

        Assert.NotNull(providerService);
        Assert.NotNull(consumerService);
        Assert.Same(providerService, consumerService);
    }

    [Fact]
    public async Task Consumer_CanUseProviderContract_ThroughSharedExport()
    {
        using var harness = new ModuleManagerHarness();

        await harness.LoadAndEnableAsync(harness.ProviderDirectory);
        var consumer = await harness.LoadAndEnableAsync(harness.ConsumerDirectory);

        var reporterInterfaceType = consumer.Assemblies
            .SelectMany(a => a.GetTypes())
            .Single(t => t.Name == "IScoreboardReporter");

        var reporter = consumer.Services.GetInstance(reporterInterfaceType);
        var reportAsync = reporterInterfaceType.GetMethod("ReportAsync")!;
        var currentResult = reporterInterfaceType.GetMethod("CurrentResult")!;

        // The reporter posts via the provider's shared contract type identity.
        dynamic resultTask = reportAsync.Invoke(reporter, new object[] { "match-1", "Blue", "Red", 3, 2 })!;
        var posted = (object?)resultTask.GetAwaiter().GetResult();

        var dPosted = (dynamic)posted!;
        Assert.Equal("match-1", (string)dPosted.MatchId);
        Assert.Equal(3, (int)dPosted.BlueScore);

        // The provider's singleton holds the posted value -> state is shared across the ALC boundary.
        var stored = currentResult.Invoke(reporter, null);
        Assert.Same(posted, stored);
    }

    /// <summary>
    /// A dependency that is loaded but not enabled yet is enabled along with the module that needs
    /// it. Two modules that depend on each other can therefore both end up enabled.
    /// </summary>
    [Fact]
    public async Task Enable_DependsOnDisabledDependency_EnablesIt()
    {
        using var harness = new ModuleManagerHarness();

        var provider = await harness.LoadAndEnableAsync(harness.ProviderDirectory);
        await harness.Manager.DisableAsync(provider.LoadId);

        await harness.Manager.LoadAsync(harness.ConsumerDirectory);
        var consumer = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "ContractApiConsumerModule");

        await harness.Manager.EnableAsync(consumer.LoadId);

        Assert.True(consumer.IsEnabled);
        Assert.True(provider.IsEnabled);
    }

    /// <summary>
    /// A dependency that is turned off in the configuration is never enabled implicitly, since
    /// that would override an explicit choice.
    /// </summary>
    [Fact]
    public async Task Enable_DependencyDisabledInConfig_Throws()
    {
        using var harness = new ModuleManagerHarness();
        harness.ModulesConfig.Setup(c => c.DisabledModules).Returns(new[] { "ContractApiProviderModule" });

        await harness.Manager.LoadAsync(harness.ProviderDirectory);
        await harness.Manager.LoadAsync(harness.ConsumerDirectory);
        var consumer = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "ContractApiConsumerModule");

        await Assert.ThrowsAsync<EvoScModuleException>(() => harness.Manager.EnableAsync(consumer.LoadId));
    }

    [Fact]
    public void Disable_MarksModuleDisabledAndKeepsLoaded()
    {
        using var harness = new ModuleManagerHarness();
        var provider = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ProviderDirectory);

        Assert.Equal(ModuleStatus.Enabled, provider.Status);
        Assert.True(provider.IsEnabled);

        ModuleManagerHarness.DisableBlocking(harness.Manager, provider.LoadId);

        Assert.Equal(ModuleStatus.Disabled, provider.Status);
        Assert.False(provider.IsEnabled);
        Assert.Contains(harness.Manager.GetLoadedModules(), m => m.LoadId == provider.LoadId);
    }

    [Fact]
    public void Unload_Dependency_CascadesToDependents()
    {
        var (providerAlcWeak, consumerAlcWeak) = RunCascadeUnload();
        ForceCollectAll(providerAlcWeak, consumerAlcWeak);

        Assert.False(providerAlcWeak.IsAlive, "The provider load context should have been collected after the cascade.");
        Assert.False(consumerAlcWeak.IsAlive, "The consumer load context should have been collected after the cascade.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference ProviderAlcWeak, WeakReference ConsumerAlcWeak) RunCascadeUnload()
    {
        using var harness = new ModuleManagerHarness();
        var provider = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ProviderDirectory);
        var consumer = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ConsumerDirectory);
        var providerId = provider.LoadId;
        var providerAlcWeak = new WeakReference(provider.AsmLoadContext!);
        var consumerAlcWeak = new WeakReference(consumer.AsmLoadContext!);
        ModuleManagerHarness.UnloadBlocking(harness.Manager, providerId);

        Assert.Empty(harness.Manager.GetLoadedModules());

        return (providerAlcWeak, consumerAlcWeak);
    }

    [Fact]
    public void Unload_DisposesModuleContainerAndReleasesInstance()
    {
        var instanceWeak = RunUnloadTeardown();
        ForceCollectAll(instanceWeak);

        Assert.False(instanceWeak.IsAlive, "The module instance should be collected after its container is disposed on unload.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference RunUnloadTeardown()
    {
        using var harness = new ModuleManagerHarness();
        var provider = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ProviderDirectory);
        var loadId = provider.LoadId;
        var instanceWeak = new WeakReference(provider.Instance);

        ModuleManagerHarness.UnloadBlocking(harness.Manager, loadId);

        Assert.DoesNotContain(harness.Manager.GetLoadedModules(), m => m.LoadId == loadId);

        bool containerRemoved = false;
        try
        {
            harness.ServicesManager.RemoveContainer(loadId);
        }
        catch (ServicesException)
        {
            containerRemoved = true;
        }
        Assert.True(containerRemoved, "The module service container must be removed from the manager on unload.");

        // The container must not survive the helper frame; if it did, its disposed
        // registrations would keep the module instance reachable forever.
        return instanceWeak;
    }

    [Fact]
    public void Unload_CollectsLoadContextAndExportAssembly()
    {
        var (providerAlcWeak, consumerAlcWeak, exportWeak) = RunSharedExportUnload();
        ForceCollectAll(providerAlcWeak, consumerAlcWeak, exportWeak);

        Assert.False(providerAlcWeak.IsAlive, "The provider load context should have been collected.");
        Assert.False(consumerAlcWeak.IsAlive, "The consumer load context should have been collected.");
        Assert.False(exportWeak.IsAlive, "The export assembly should have been unloaded once no module references it.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference ProviderAlcWeak, WeakReference ConsumerAlcWeak, WeakReference ExportWeak) RunSharedExportUnload()
    {
        using var harness = new ModuleManagerHarness();
        var provider = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ProviderDirectory);
        var consumer = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ConsumerDirectory);
        var providerId = provider.LoadId;
        var consumerId = consumer.LoadId;
        var providerModuleId = provider.ModuleInfo.Id;
        var providerAlcWeak = new WeakReference(provider.AsmLoadContext!);
        var consumerAlcWeak = new WeakReference(consumer.AsmLoadContext!);
        var exportAssembly = harness.Manager.ExportAssemblies
            .AcquireExportForModule(consumerId, providerModuleId)!;
        var exportWeak = new WeakReference(exportAssembly);

        ModuleManagerHarness.UnloadBlocking(harness.Manager, consumerId);
        Assert.DoesNotContain(harness.Manager.GetLoadedModules(),
            m => m.ModuleInfo.Id == "ContractApiConsumerModule");

        // Provider is still loaded, so its export must still be shared and alive.
        exportWeak.ForceCollect();
        Assert.True(exportWeak.IsAlive, "The export assembly should remain alive while its provider is loaded.");

        ModuleManagerHarness.UnloadBlocking(harness.Manager, providerId);
        Assert.Empty(harness.Manager.GetLoadedModules());

        return (providerAlcWeak, consumerAlcWeak, exportWeak);
    }

    [Fact]
    public void Reload_ReloadsModuleFromDirectory()
    {
        var providerAlcWeak = RunReloadAndAssertStates();
        ForceCollectAll(providerAlcWeak);

        Assert.False(providerAlcWeak.IsAlive, "The old load context should have been collected after reload.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference RunReloadAndAssertStates()
    {
        using var harness = new ModuleManagerHarness();
        var provider = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ProviderDirectory);
        var providerId = provider.LoadId;
        var providerAlcWeak = new WeakReference(provider.AsmLoadContext!);
        ModuleManagerHarness.ReloadBlocking(harness.Manager, providerId);

        var reloaded = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "ContractApiProviderModule");
        Assert.Equal(ModuleStatus.Loaded, reloaded.Status);
        Assert.NotEqual(providerId, reloaded.LoadId);

        harness.Manager.EnableAsync(reloaded.LoadId).GetAwaiter().GetResult();
        Assert.Equal(ModuleStatus.Enabled, reloaded.Status);

        return providerAlcWeak;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ForceCollectAll(params WeakReference[] weaks)
    {
        var deadline = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        while (Array.Exists(weaks, w => w.IsAlive) && DateTime.UtcNow < deadline)
        {
            foreach (var w in weaks)
            {
                w.ForceCollect();
            }
        }
    }

    [Fact]
    public void Unload_ReleasesExportReference()
    {
        using var harness = new ModuleManagerHarness();

        string providerModuleId = LoadAndUnloadProvider(harness);

        Assert.Empty(harness.Manager.GetLoadedModules());
        Assert.False(harness.Manager.ExportAssemblies.HasExport(providerModuleId));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string LoadAndUnloadProvider(ModuleManagerHarness harness)
    {
        var provider = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ProviderDirectory);
        ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ConsumerDirectory);
        var providerModuleId = provider.ModuleInfo.Id;

        ModuleManagerHarness.UnloadBlocking(harness.Manager, provider.LoadId);

        return providerModuleId;
    }

    [Fact]
    public void ConsumerModuleDirectory_DoesNotShipProviderExportAssembly()
    {
        using var harness = new ModuleManagerHarness();

        var providerExportExpectedLocation = Path.Combine(
            Path.GetDirectoryName(harness.ConsumerDirectory)!,
            "ContractApiProviderModule",
            "ContractApiProviderModule.Exports.dll");
        Assert.True(File.Exists(providerExportExpectedLocation),
            "The provider export assembly must be emitted into the provider's module directory by the build.");

        // The consumer must never carry a copy of the provider's export assembly: at runtime it is
        // loaded dynamically from the provider via the export store.
        Assert.False(File.Exists(Path.Combine(harness.ConsumerDirectory, "ContractApiProviderModule.Exports.dll")));
        Assert.False(File.Exists(Path.Combine(harness.ConsumerDirectory, "ContractApiProviderModule.dll")));
    }

    /// <summary>
    /// Two modules may depend on each other. Loading is done in phases for exactly this reason:
    /// every module is loaded before any of them is linked to its dependencies, so no module has to
    /// be loaded before the one depending on it.
    /// </summary>
    [Fact]
    public async Task LoadCollection_LoadsModulesThatDependOnEachOther()
    {
        using var harness = new ModuleManagerHarness();
        var collection = ModuleDirectoryUtils.FindModulesFromDirectory(harness.ModulesDirectory);

        await harness.Manager.LoadAsync(collection);

        var a = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "CyclicModuleA");
        var b = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "CyclicModuleB");

        Assert.Equal(ModuleStatus.Loaded, a.Status);
        Assert.Equal(ModuleStatus.Loaded, b.Status);
        Assert.Equal(new[] { b.LoadId }, a.LoadedDependencies);
        Assert.Equal(new[] { a.LoadId }, b.LoadedDependencies);
    }

    [Fact]
    public async Task Enable_EnablesModulesThatDependOnEachOther()
    {
        using var harness = new ModuleManagerHarness();
        await harness.Manager.LoadAsync(ModuleDirectoryUtils.FindModulesFromDirectory(harness.ModulesDirectory));

        var a = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "CyclicModuleA");
        var b = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "CyclicModuleB");

        await harness.Manager.EnableAsync(a.LoadId);

        Assert.True(a.IsEnabled);
        Assert.True(b.IsEnabled, "Enabling a module must enable the dependency that depends back on it.");
    }

    [Fact]
    public async Task Enable_FromTheOtherSideOfTheCycle_EnablesBoth()
    {
        using var harness = new ModuleManagerHarness();
        await harness.Manager.LoadAsync(ModuleDirectoryUtils.FindModulesFromDirectory(harness.ModulesDirectory));

        var a = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "CyclicModuleA");
        var b = harness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "CyclicModuleB");

        await harness.Manager.EnableAsync(b.LoadId);

        Assert.True(a.IsEnabled);
        Assert.True(b.IsEnabled);
    }

    /// <summary>
    /// A cyclic pair can only be loaded as a set: loaded on its own, a module's dependency is
    /// genuinely absent and the load fails instead of leaving the module half-registered.
    /// </summary>
    [Fact]
    public async Task Load_ModuleInCycleWithoutItsDependency_ThrowsAndLeavesNothingLoaded()
    {
        using var harness = new ModuleManagerHarness();

        await Assert.ThrowsAsync<DependencyNotFoundException>(
            () => harness.Manager.LoadAsync(harness.CyclicModuleADirectory));

        Assert.Empty(harness.Manager.GetLoadedModules());
    }
}