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
        _ = typeof(IScoreboardService);
        _ = typeof(MatchResult);
    }

    [Fact]
    public async Task ProviderAndConsumer_ShareContractTypeIdentity()
    {
        using var harness = new ModuleManagerHarness();

        var provider = await harness.LoadAndEnableAsync(harness.ProviderDirectory);
        var consumer = await harness.LoadAndEnableAsync(harness.ConsumerDirectory);

        // The export assembly is loaded once into the shared store and referenced by both modules.
        var store = harness.Manager.ExportAssemblies;
        var exportAssembly = store.ResolveExportForModule(consumer.LoadId, "ContractApiProviderModule.Exports");
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
        var stored = (object?)currentResult.Invoke(reporter, null);
        Assert.Equal((object)posted, stored);
    }

    [Fact]
    public async Task Enable_DependentBeforeDependency_Throws()
    {
        using var harness = new ModuleManagerHarness();

        var provider = await harness.LoadAndEnableAsync(harness.ProviderDirectory);
        await harness.Manager.DisableAsync(provider.LoadId);

        await harness.Manager.LoadAsync(harness.ConsumerDirectory);

        var consumer = harness.Manager.LoadedModules.Single(m => m.ModuleInfo.Name == "ContractApiConsumerModule");

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
        Assert.Contains(harness.Manager.LoadedModules, m => m.LoadId == provider.LoadId);
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

        Assert.Empty(harness.Manager.LoadedModules);

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

        Assert.DoesNotContain(harness.Manager.LoadedModules, m => m.LoadId == loadId);

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
        var providerAlcWeak = new WeakReference(provider.AsmLoadContext!);
        var consumerAlcWeak = new WeakReference(consumer.AsmLoadContext!);
        var exportAssembly = harness.Manager.ExportAssemblies
            .ResolveExportForModule(consumerId, "ContractApiProviderModule.Exports")!;
        var exportWeak = new WeakReference(exportAssembly);

        ModuleManagerHarness.UnloadBlocking(harness.Manager, consumerId);
        Assert.DoesNotContain(harness.Manager.LoadedModules,
            m => m.ModuleInfo.Name == "ContractApiConsumerModule");

        // Provider is still loaded, so its export must still be shared and alive.
        exportWeak.ForceCollect();
        Assert.True(exportWeak.IsAlive, "The export assembly should remain alive while its provider is loaded.");

        ModuleManagerHarness.UnloadBlocking(harness.Manager, providerId);
        Assert.Empty(harness.Manager.LoadedModules);

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

        var reloaded = harness.Manager.LoadedModules.Single(m => m.ModuleInfo.Name == "ContractApiProviderModule");
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
        while (weaks.Any(w => w.IsAlive) && DateTime.UtcNow < deadline)
        {
            foreach (var w in weaks)
            {
                w.ForceCollect();
            }
        }
    }

    [Fact]
    public async Task Unload_ReleasesExportReference()
    {
        using var harness = new ModuleManagerHarness();

        Guid providerId;
        Guid consumerId;
        {
            var provider = await harness.LoadAndEnableAsync(harness.ProviderDirectory);
            var consumer = await harness.LoadAndEnableAsync(harness.ConsumerDirectory);
            providerId = provider.LoadId;
            consumerId = consumer.LoadId;
        }

        await harness.Manager.UnloadAsync(providerId);

        Assert.Empty(harness.Manager.LoadedModules);
        Assert.Null(harness.Manager.ExportAssemblies.ResolveExportForModule(consumerId, "ContractApiProviderModule.Exports"));
        Assert.Null(harness.Manager.ExportAssemblies.ResolveExportForModule(providerId, "ContractApiProviderModule.Exports"));
    }
}