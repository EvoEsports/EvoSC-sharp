using System.Runtime.CompilerServices;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Localization;
using EvoSC.Common.Services;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Util;
using SimpleInjector;
using Microsoft.Extensions.Logging.Abstractions;

namespace EvoSC.Modules.Tests;

/// <summary>
/// Builds a real <see cref="ModuleManager"/> backed by a real
/// <see cref="ServiceContainerManager"/> and mocked infrastructure, so the module
/// lifecycle (load/enable/unload/reload) and the export assembly sharing can be
/// exercised end-to-end against modules built on disk.
/// </summary>
public class ModuleManagerHarness : IDisposable
{
    public Container RootServices { get; } = new();
    public ServiceContainerManager ServicesManager { get; }
    public ModuleManager Manager { get; }
    public Mock<IEvoScBaseConfig> Config { get; }
    public Mock<IModuleConfig> ModulesConfig { get; }
    public Mock<IManialinkManager> ManialinkManager { get; }
    public Mock<IConfigStoreRepository> ConfigStoreRepository { get; }

    /// <summary>
    /// The config options which module configs were written to, so tests can assert what a module
    /// persisted and so a reloaded module reads back the same values.
    /// </summary>
    public IReadOnlyDictionary<string, DbConfigOption> ConfigOptions => _configOptions;

    private readonly Dictionary<string, DbConfigOption> _configOptions = new();

    public ModuleManagerHarness(Mock<IManialinkManager>? manialinkManager = null)
    {
        var app = new FakeApp(RootServices);

        ModulesConfig = new Mock<IModuleConfig>();
        ModulesConfig.Setup(c => c.RequireSignatureVerification).Returns(false);
        ModulesConfig.Setup(c => c.DisabledModules).Returns(Array.Empty<string>());

        Config = new Mock<IEvoScBaseConfig>();
        Config.Setup(c => c.Modules).Returns(ModulesConfig.Object);

        ServicesManager = new ServiceContainerManager(app, NullLogger<ServiceContainerManager>.Instance);

        ConfigStoreRepository = new Mock<IConfigStoreRepository>();
        ConfigStoreRepository.Setup(r => r.GetConfigOptionsByKeyAsync(It.IsAny<string>()))
            .Returns<string>(key => Task.FromResult(_configOptions.GetValueOrDefault(key)));
        ConfigStoreRepository.Setup(r => r.AddConfigOptionAsync(It.IsAny<DbConfigOption>()))
            .Returns<DbConfigOption>(option =>
            {
                _configOptions[option.Key] = option;
                return Task.CompletedTask;
            });
        ConfigStoreRepository.Setup(r => r.UpdateConfigOptionAsync(It.IsAny<DbConfigOption>()))
            .Returns<DbConfigOption>(option =>
            {
                _configOptions[option.Key] = option;
                return Task.CompletedTask;
            });

        ManialinkManager = manialinkManager ?? new Mock<IManialinkManager>();

        Manager = new ModuleManager(
            NullLogger<ModuleManager>.Instance,
            NullLogger<LocalizationManager>.Instance,
            Config.Object,
            new Mock<IControllerManager>().Object,
            ServicesManager,
            new Mock<IActionPipelineManager>().Object,
            new Mock<IPermissionManager>().Object,
            ConfigStoreRepository.Object,
            ManialinkManager.Object,
            new Mock<IThemeManager>().Object);
    }

    public string ProviderDirectory => Path.Combine(AppContext.BaseDirectory, "modules", "ContractApiProviderModule");

    public string ConsumerDirectory => Path.Combine(AppContext.BaseDirectory, "modules", "ContractApiConsumerModule");

    public string ModulesDirectory => Path.Combine(AppContext.BaseDirectory, "modules");

    public string CyclicModuleADirectory => Path.Combine(ModulesDirectory, "CyclicModuleA");

    public string ConfigManialinkModuleDirectory => Path.Combine(ModulesDirectory, "ConfigManialinkModule");

    public async Task<IModuleLoadContext> LoadAndEnableAsync(string directory)
    {
        var before = Manager.GetLoadedModules().Count;
        await Manager.LoadAsync(directory);
        var module = Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id.Contains(
            Path.GetFileName(directory), StringComparison.OrdinalIgnoreCase));
        Assert.Equal(ModuleStatus.Loaded, module.Status);
        Assert.Equal(before + 1, Manager.GetLoadedModules().Count);
        await Manager.EnableAsync(module.LoadId);
        Assert.Equal(ModuleStatus.Enabled, module.Status);
        Assert.True(module.IsEnabled);
        return module;
    }

    /// <summary>Forces GC until <paramref name="weak"/> is collected.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ForceCollection(WeakReference? weak)
        => CollectibleLoadContext.WaitForUnload(weak, 20);

    /// <summary>
    /// Runs <see cref="LoadAndEnableAsync"/> without letting the awaited task's state machine
    /// outlive this frame. Module lifecycle tasks capture the module load context (and therefore
    /// its collectible ALC) on the heap; awaited from an async test method their completed boxes
    /// stay strongly referenced by the test's awaiter fields and keep the ALC alive forever.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IModuleLoadContext LoadAndEnableBlocking(ModuleManagerHarness harness, string directory)
        => harness.LoadAndEnableAsync(directory).GetAwaiter().GetResult();

    /// <summary>See <see cref="LoadAndEnableBlocking"/>.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void UnloadBlocking(ModuleManager manager, Guid loadId)
        => manager.UnloadAsync(loadId).GetAwaiter().GetResult();

    /// <summary>See <see cref="LoadAndEnableBlocking"/>.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ReloadBlocking(ModuleManager manager, Guid loadId)
        => manager.ReloadAsync(loadId).GetAwaiter().GetResult();

    /// <summary>See <see cref="LoadAndEnableBlocking"/>.</summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void DisableBlocking(ModuleManager manager, Guid loadId)
        => manager.DisableAsync(loadId).GetAwaiter().GetResult();

    private sealed class FakeApp(Container services) : IEvoSCApplication
    {
        public IStartupPipeline StartupPipeline => null!;
        public CancellationToken MainCancellationToken => CancellationToken.None;
        public Container Services => services;
        public Task RunAsync() => Task.CompletedTask;
        public Task ShutdownAsync() => Task.CompletedTask;
    }

    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            RootServices.Dispose();
        }

        _disposed = true;
    }
}

public static class ModuleManagerHarnessExtensions
{
    public static void ForceCollect(this WeakReference? weak) => ModuleManagerHarness.ForceCollection(weak);
}