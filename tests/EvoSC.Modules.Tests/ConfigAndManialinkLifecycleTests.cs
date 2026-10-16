using System.Reflection;
using System.Runtime.CompilerServices;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Interfaces;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Tests;

/// <summary>
/// A module that has both a Config.Net settings interface and a manialink template exercises the
/// parts of the lifecycle which are only reachable when both are present: the settings are built
/// into a collectible proxy, the template is compiled into the same collectible load context, and
/// both have to be released again on unload or the module can never be collected.
/// </summary>
[Collection("ModuleSystem")]
public class ConfigAndManialinkLifecycleTests
{
    private const string SettingsTypeName =
        "EvoSC.Modules.Official.ConfigManialinkModule.Config.IConfigManialinkSettings";

    private const string TemplateName = "ConfigManialinkModule.ConfigManialinkWidget";

    [Fact]
    public void ModuleConfig_IsBuiltWithDefaults_AndStaysWritable()
    {
        using var harness = new ModuleManagerHarness();
        var module = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ConfigManialinkModuleDirectory);

        var settingsType = GetSettingsType(module);
        var settings = GetSettings(module);

        Assert.Equal("greeting", Get<string>(settingsType, settings, "Greeting"));
        Assert.Equal(42, Get<int>(settingsType, settings, "Answer"));

        // The collectible proxy has to route writes through to the backing store, not just reads.
        Set(settingsType, settings, "Greeting", "hi");
        Set(settingsType, settings, "Answer", 7);

        Assert.Equal("hi", Get<string>(settingsType, settings, "Greeting"));
        Assert.Equal(7, Get<int>(settingsType, settings, "Answer"));

        // Writes must reach the config store, otherwise a reload would lose them.
        Assert.Equal("hi", PersistedConfigValue(harness, "Greeting"));
        Assert.Equal("7", PersistedConfigValue(harness, "Answer"));
    }

    [Fact]
    public void ModuleManialinkTemplate_IsRegisteredWithTheModuleAssemblies_AndRendersItsConfig()
    {
        var manialinks = new Mock<IManialinkManager>();
        var manager = CreateManialinkManager();
        ForwardTo(manialinks, manager);

        using var forwardingHarness = new ModuleManagerHarness(manialinks);
        var module = ModuleManagerHarness.LoadAndEnableBlocking(forwardingHarness, forwardingHarness.ConfigManialinkModuleDirectory);

        var added = Assert.Single(manialinks.Invocations
            .Where(i => i.Method.Name == nameof(IManialinkManager.AddTemplate))
            .Select(i => (IManialinkTemplateInfo)i.Arguments[0]!));

        Assert.Equal(TemplateName, added.Name);
        Assert.Contains(module.Assemblies, a => a.GetName().Name == "ConfigManialinkModule");

        // The template refers to the module's settings interface, so rendering it proves the config
        // proxy and the compiled template resolve to the same types inside the module's load context.
        var settingsType = GetSettingsType(module);
        var settings = GetSettings(module);
        Set(settingsType, settings, "Greeting", "hello");
        Set(settingsType, settings, "Answer", 1337);

        var output = manager.PrepareAndRenderAsync(TemplateName,
            new Dictionary<string, object?> { ["settings"] = settings }).GetAwaiter().GetResult();

        Assert.Contains("hello", output);
        Assert.Contains("1337", output);
    }

    [Fact]
    public void Unload_RemovesTheTemplate_AndCollectsTheModule()
    {
        var (alcWeak, settingsWeak) = RunRenderThenUnload();
        ForceCollectAll(alcWeak, settingsWeak);

        Assert.False(alcWeak.IsAlive, "The module load context should be collected after its template was removed.");
        Assert.False(settingsWeak.IsAlive, "The config proxy should be collected with its load context.");
    }

    [Fact]
    public void Reload_RegistersConfigAndTemplateAgain()
    {
        var manialinks = new Mock<IManialinkManager>();
        var manager = CreateManialinkManager();
        ForwardTo(manialinks, manager);

        using var forwardingHarness = new ModuleManagerHarness(manialinks);
        var module = ModuleManagerHarness.LoadAndEnableBlocking(forwardingHarness, forwardingHarness.ConfigManialinkModuleDirectory);
        var loadId = module.LoadId;

        ModuleManagerHarness.ReloadBlocking(forwardingHarness.Manager, loadId);

        // A reload loads the module again from disk, so it comes back disabled under a new load id.
        var reloaded = forwardingHarness.Manager.GetLoadedModules().Single(m => m.ModuleInfo.Id == "ConfigManialinkModule");
        Assert.Equal(ModuleStatus.Loaded, reloaded.Status);
        Assert.NotEqual(loadId, reloaded.LoadId);

        forwardingHarness.Manager.EnableAsync(reloaded.LoadId).GetAwaiter().GetResult();
        Assert.Equal(ModuleStatus.Enabled, reloaded.Status);

        var settingsType = GetSettingsType(reloaded);
        var settings = GetSettings(reloaded);
        Assert.Equal("greeting", Get<string>(settingsType, settings, "Greeting"));

        var output = manager.PrepareAndRenderAsync(TemplateName,
            new Dictionary<string, object?> { ["settings"] = settings }).GetAwaiter().GetResult();

        Assert.Contains("greeting", output);
        Assert.Contains("42", output);
    }

    /// <summary>
    /// Renders the module's template, unloads it, and hands back weak references to the things that
    /// would keep the load context alive: the context itself and the config proxy built inside it.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference Alc, WeakReference Settings) RunRenderThenUnload()
    {
        var manialinks = new Mock<IManialinkManager>();
        var manager = CreateManialinkManager();
        ForwardTo(manialinks, manager);

        using var harness = new ModuleManagerHarness(manialinks);
        var module = ModuleManagerHarness.LoadAndEnableBlocking(harness, harness.ConfigManialinkModuleDirectory);
        var loadId = module.LoadId;

        var settings = GetSettings(module);
        manager.PrepareAndRenderAsync(TemplateName,
            new Dictionary<string, object?> { ["settings"] = settings }).GetAwaiter().GetResult();

        var alcWeak = new WeakReference(module.AsmLoadContext!);
        var settingsWeak = new WeakReference(settings);

        ModuleManagerHarness.UnloadBlocking(harness.Manager, loadId);

        Assert.Contains(manialinks.Invocations, i => i.Method.Name == nameof(IManialinkManager.RemoveAndHideTemplateAsync)
                                                     && Equals(i.Arguments[0], TemplateName));
        Assert.DoesNotContain(harness.Manager.GetLoadedModules(), m => m.LoadId == loadId);

        // Rendering the template of an unloaded module must no longer work: its compiled form has
        // to be gone together with the module, otherwise the load context could not be collected.
        Assert.ThrowsAny<Exception>(() => manager.PrepareAndRenderAsync(TemplateName,
            new Dictionary<string, object?> { ["settings"] = null }).GetAwaiter().GetResult());

        return (alcWeak, settingsWeak);
    }

    private static ManialinkManager CreateManialinkManager()
    {
        var themes = new Mock<IThemeManager>();
        themes.Setup(t => t.ComponentReplacements).Returns(new Dictionary<string, string>());

        var server = new Mock<IServerClient>();
        server.Setup(s => s.Remote).Returns(new Mock<IGbxRemoteClient>().Object);

        return new ManialinkManager(
            NullLogger<ManialinkManager>.Instance,
            server.Object,
            new Mock<IEventManager>().Object,
            themes.Object,
            new Mock<IPlayerManagerService>().Object);
    }

    private static void ForwardTo(Mock<IManialinkManager> mock, ManialinkManager real)
    {
        mock.Setup(m => m.AddTemplate(It.IsAny<IManialinkTemplateInfo>()))
            .Callback<IManialinkTemplateInfo>(info => real.AddTemplate(info));
        mock.Setup(m => m.AddManiaScript(It.IsAny<IManiaScriptInfo>()))
            .Callback<IManiaScriptInfo>(info => real.AddManiaScript(info));
        mock.Setup(m => m.RemoveAndHideTemplateAsync(It.IsAny<string>()))
            .Returns<string>(name => real.RemoveAndHideTemplateAsync(name));
        mock.Setup(m => m.RemoveManiaScript(It.IsAny<string>()))
            .Callback<string>(name => real.RemoveManiaScript(name));
    }

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

    private static string? PersistedConfigValue(ModuleManagerHarness harness, string property) =>
        harness.ConfigOptions
            .Where(o => o.Key.EndsWith(property, StringComparison.Ordinal))
            .Select(o => o.Value.Value)
            .SingleOrDefault();

    private static object GetSettings(IModuleLoadContext module) =>
        module.Services.GetInstance(GetSettingsType(module));

    private static Type GetSettingsType(IModuleLoadContext module) =>
        module.Assemblies
            .Select(a => a.GetType(SettingsTypeName, throwOnError: false))
            .FirstOrDefault(t => t != null)
        ?? throw new InvalidOperationException($"The module did not declare {SettingsTypeName}.");

    // The emitted proxy implements the settings interface explicitly, so members have to be invoked
    // through the interface itself, which is also how a module consumes its config.
    private static T Get<T>(Type settingsType, object settings, string property) =>
        (T)settingsType.GetProperty(property)!.GetValue(settings)!;

    private static void Set(Type settingsType, object settings, string property, object value) =>
        settingsType.GetProperty(property)!.SetValue(settings, value);
}
