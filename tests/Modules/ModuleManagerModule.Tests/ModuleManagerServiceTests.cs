using System.Dynamic;
using System.Resources;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Util.Auditing;
using EvoSC.Common.Util.TextFormatting;
using EvoSC.Modules;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Events;
using EvoSC.Modules.Official.ModuleManagerModule.Services;
using EvoSC.Testing;
using Microsoft.CSharp.RuntimeBinder;
using Moq;

namespace EvoSC.Modules.Official.ModuleManagerModule.Tests;

public class ModuleManagerServiceTests
{
    private readonly Mock<IModuleManager> _modules = new();
    private readonly Mock<IChatService> _chat = new();
    private readonly Mock<IContextService> _context = new();
    private readonly Mock<IAuditEventBuilder> _audit = Mocking.NewAuditEventBuilderMock();
    private readonly Mock<IPlayer> _actor = new();

    private readonly List<string> _info = new();
    private readonly List<string> _success = new();
    private readonly List<string> _errors = new();

    private readonly ModuleManagerService _service;

    public ModuleManagerServiceTests()
    {
        _context.Setup(c => c.Audit()).Returns(_audit.Object);
        _audit.SetupGet(a => a.Actor).Returns(_actor.Object);

        _chat.Setup(c => c.InfoMessageAsync(It.IsAny<TextFormatter>(), It.IsAny<IPlayer[]>()))
            .Callback((TextFormatter text, IPlayer[] _) => _info.Add(text.ToString()))
            .Returns(Task.CompletedTask);
        _chat.Setup(c => c.InfoMessageAsync(It.IsAny<string>(), It.IsAny<IPlayer[]>()))
            .Callback((string text, IPlayer[] _) => _info.Add(text))
            .Returns(Task.CompletedTask);
        _chat.Setup(c => c.SuccessMessageAsync(It.IsAny<string>(), It.IsAny<IPlayer[]>()))
            .Callback((string text, IPlayer[] _) => _success.Add(text))
            .Returns(Task.CompletedTask);
        _chat.Setup(c => c.ErrorMessageAsync(It.IsAny<string>(), It.IsAny<IPlayer[]>()))
            .Callback((string text, IPlayer[] _) => _errors.Add(text))
            .Returns(Task.CompletedTask);

        _service = new ModuleManagerService(_context.Object, _modules.Object, _chat.Object, new TestLocale());
    }

    private static IModuleLoadContext CreateModule(string id, ModuleStatus status = ModuleStatus.Enabled,
        bool isInternal = false, bool isEnabled = true, List<Guid>? loadedDependencies = null) =>
        TestModuleLoader.CreateModule(id, status, isInternal, isEnabled, loadedDependencies);

    private void SetLoadedModules(params IModuleLoadContext[] modules) =>
        _modules.Setup(m => m.GetLoadedModules()).Returns(modules);

    [Fact]
    public async Task Enable_Enables_Module()
    {
        var module = CreateModule("OpenPlanet");

        await _service.EnableModuleAsync(module);

        _modules.Verify(m => m.EnableAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);
        _audit.Verify(a => a.WithEventName(AuditEvents.ModuleEnabled), Times.Once);
        Assert.Contains(_success, s => s.Contains("ModuleWasEnabled(OpenPlanet)"));
    }

    [Fact]
    public async Task Enable_Reports_Failure_Without_Throwing()
    {
        var module = CreateModule("OpenPlanet");
        _modules.Setup(m => m.EnableAsync(It.IsAny<Guid>())).ThrowsAsync(new InvalidOperationException("boom"));

        await _service.EnableModuleAsync(module);

        _audit.Verify(a => a.Error(), Times.Once);
        Assert.Contains(_errors, e => e.Contains("boom"));
    }

    [Fact]
    public async Task Disable_Disables_Module_And_Audits_ModuleDisabled()
    {
        var module = CreateModule("OpenPlanet");
        SetLoadedModules(module);

        await _service.DisableModuleAsync(module);

        _modules.Verify(m => m.DisableAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);

        // a disable must never be recorded as an enable
        _audit.Verify(a => a.WithEventName(AuditEvents.ModuleDisabled), Times.Once);
        _audit.Verify(a => a.WithEventName(AuditEvents.ModuleEnabled), Times.Never);
        Assert.Contains(_success, s => s.Contains("ModuleWasDisabled(OpenPlanet)"));
    }

    [Fact]
    public async Task Disable_Allows_Otherwise_Essential_Modules()
    {
        var module = CreateModule("Player");
        SetLoadedModules(module);

        await _service.DisableModuleAsync(module);

        _modules.Verify(m => m.DisableAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);
        Assert.Contains(_success, s => s.Contains("ModuleWasDisabled(Player)"));
    }

    [Fact]
    public async Task Disable_Refuses_Module_Used_By_Enabled_Module()
    {
        var module = CreateModule("Scores");
        var dependent = CreateModule("OpenPlanet", loadedDependencies: new List<Guid> {module.LoadId});
        SetLoadedModules(module, dependent);

        await _service.DisableModuleAsync(module);

        _modules.Verify(m => m.DisableAsync(It.IsAny<Guid>()), Times.Never);
        _audit.Verify(a => a.Error(), Times.Once);
        Assert.Contains(_errors, e => e.Contains("CannotDisableModuleWithDependents(OpenPlanet)"));
    }

    [Fact]
    public async Task Disable_Ignores_Disabled_Dependents()
    {
        var module = CreateModule("OpenPlanet");
        var dependent = CreateModule("PlayerRecords", ModuleStatus.Disabled, isEnabled: false,
            loadedDependencies: new List<Guid> {module.LoadId});
        SetLoadedModules(module, dependent);

        await _service.DisableModuleAsync(module);

        _modules.Verify(m => m.DisableAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);
    }

    [Fact]
    public async Task Disable_Ignores_Enabled_Dependents_Of_Other_Modules()
    {
        var module = CreateModule("OpenPlanet");
        var other = CreateModule("SpectatorCamMode");
        var dependent = CreateModule("PlayerRecords", loadedDependencies: new List<Guid> {other.LoadId});
        SetLoadedModules(module, other, dependent);

        await _service.DisableModuleAsync(module);

        _modules.Verify(m => m.DisableAsync(module.LoadId), Times.Once);
    }

    [Fact]
    public async Task Reload_Reloads_Module()
    {
        var module = CreateModule("OpenPlanet");

        await _service.ReloadModuleAsync(module);

        _modules.Verify(m => m.ReloadAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);
        _audit.Verify(a => a.WithEventName(AuditEvents.ModuleReloaded), Times.Once);
        Assert.Contains(_success, s => s.Contains("ModuleWasReloaded(OpenPlanet)"));
    }

    [Fact]
    public async Task Reload_Reports_Failure_Without_Throwing()
    {
        var module = CreateModule("OpenPlanet");
        _modules.Setup(m => m.ReloadAsync(It.IsAny<Guid>())).ThrowsAsync(new InvalidOperationException("boom"));

        await _service.ReloadModuleAsync(module);

        _audit.Verify(a => a.Error(), Times.Once);
        Assert.Contains(_errors, e => e.Contains("boom"));
    }

    [Fact]
    public async Task Unload_Unloads_Module()
    {
        var module = CreateModule("OpenPlanet");

        await _service.UnloadModuleAsync(module);

        _modules.Verify(m => m.UnloadAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);
        _audit.Verify(a => a.WithEventName(AuditEvents.ModuleUnloaded), Times.Once);
        Assert.Contains(_success, s => s.Contains("ModuleWasUnloaded(OpenPlanet)"));
    }

    [Fact]
    public async Task Unload_Reports_Failure_Without_Throwing()
    {
        var module = CreateModule("OpenPlanet");
        _modules.Setup(m => m.UnloadAsync(It.IsAny<Guid>())).ThrowsAsync(new InvalidOperationException("boom"));

        await _service.UnloadModuleAsync(module);

        _audit.Verify(a => a.Error(), Times.Once);
        Assert.Contains(_errors, e => e.Contains("boom"));
    }

    [Fact]
    public async Task Install_Loads_Module_From_Directory()
    {
        var directory = CreateModuleDirectory("TestModule");

        try
        {
            await _service.InstallModuleAsync(directory);

            _modules.Verify(m => m.LoadAsync(directory), Times.Once);
            _audit.Verify(a => a.Success(), Times.Once);
            _audit.Verify(a => a.WithEventName(AuditEvents.ModuleInstalled), Times.Once);
            Assert.Contains(_success, s => s.Contains("ModuleWasInstalled(TestModule)"));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task Install_Refuses_Directory_Without_Module_Info()
    {
        var directory = Directory.CreateTempSubdirectory("EvoSC-EmptyModule").FullName;

        try
        {
            await _service.InstallModuleAsync(directory);

            _modules.Verify(m => m.LoadAsync(It.IsAny<string>()), Times.Never);
            _audit.Verify(a => a.Error(), Times.Once);
            Assert.Contains(_errors, e => e.Contains("FailedInstallingModule"));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task Load_Loads_Module_Without_Running_Install()
    {
        var directory = CreateModuleDirectory("TestModule");

        try
        {
            await _service.LoadModuleAsync(directory);

            _modules.Verify(m => m.LoadAsync(It.IsAny<IExternalModuleInfo>(), false), Times.Once);
            _modules.Verify(m => m.LoadAsync(It.IsAny<string>()), Times.Never);
            _audit.Verify(a => a.Success(), Times.Once);
            _audit.Verify(a => a.WithEventName(AuditEvents.ModuleLoaded), Times.Once);
            Assert.Contains(_success, s => s.Contains("ModuleWasLoaded(TestModule)"));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task Load_Refuses_Directory_Without_Module_Info()
    {
        var directory = Directory.CreateTempSubdirectory("EvoSC-EmptyModule").FullName;

        try
        {
            await _service.LoadModuleAsync(directory);

            _modules.Verify(m => m.LoadAsync(It.IsAny<IExternalModuleInfo>(), false), Times.Never);
            _audit.Verify(a => a.Error(), Times.Once);
            Assert.Contains(_errors, e => e.Contains("FailedLoadingModule"));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public async Task Uninstall_Uninstalls_Module()
    {
        var module = CreateModule("OpenPlanet");

        await _service.UninstallModuleAsync(module);

        _modules.Verify(m => m.UninstallAsync(module.LoadId), Times.Once);
        _audit.Verify(a => a.Success(), Times.Once);
        _audit.Verify(a => a.WithEventName(AuditEvents.ModuleUninstalled), Times.Once);
        Assert.Contains(_success, s => s.Contains("ModuleWasUninstalled(OpenPlanet)"));
    }

    [Fact]
    public async Task Uninstall_Reports_Failure_Without_Throwing()
    {
        var module = CreateModule("OpenPlanet");
        _modules.Setup(m => m.UninstallAsync(It.IsAny<Guid>())).ThrowsAsync(new InvalidOperationException("boom"));

        await _service.UninstallModuleAsync(module);

        _audit.Verify(a => a.Error(), Times.Once);
        Assert.Contains(_errors, e => e.Contains("boom"));
    }

    [Fact]
    public async Task List_Sends_Header_And_One_Message_Per_Module()
    {
        var disabled = CreateModule("AaaModule", ModuleStatus.Disabled, isEnabled: false);
        var internalModule = CreateModule("BbbModule", ModuleStatus.Loaded, isInternal: true, isEnabled: false);
        var enabled = CreateModule("Player", ModuleStatus.Enabled);
        var errored = CreateModule("ZzzModule", ModuleStatus.Error);
        SetLoadedModules(errored, internalModule, disabled, enabled);

        await _service.ListModulesAsync(_actor.Object);

        // header plus one message per module
        Assert.Equal(5, _info.Count);
        Assert.Equal("GetLoadedModules", _info[0]);

        // modules are sorted by id
        Assert.Contains("AaaModule", _info[1]);
        Assert.Contains("BbbModule", _info[2]);
        Assert.Contains("Player", _info[3]);
        Assert.Contains("ZzzModule", _info[4]);

        Assert.Contains("Status.Disabled", _info[1]);
        Assert.Contains("Status.Loaded", _info[2]);
        Assert.Contains("Status.Enabled", _info[3]);
        Assert.Contains("Status.Error", _info[4]);

        Assert.Contains("(AaaModule name)", _info[1]);
        Assert.Contains("v1.2.3", _info[1]);

        Assert.Contains("(InternalModule)", _info[2]);
    }

    [Fact]
    public async Task List_Without_Modules_Sends_Only_Header()
    {
        SetLoadedModules();

        await _service.ListModulesAsync(_actor.Object);

        Assert.Single(_info);
        Assert.Equal("GetLoadedModules", _info[0]);
    }

    private static string CreateModuleDirectory(string id)
    {
        var directory = Directory.CreateTempSubdirectory("EvoSC-TestModule").FullName;
        File.WriteAllText(Path.Combine(directory, "info.toml"),
            "[info]\n" +
            $"id = \"{id}\"\n" +
            "name = \"Test Module\"\n" +
            "summary = \"A module used by the tests\"\n" +
            "version = \"1.0.0\"\n" +
            "author = \"EvoSC\"\n");

        return directory;
    }

    /// <summary>
    /// A locale that returns the key it was asked for, so the tests can tell which string the
    /// service picked and with which arguments.
    /// </summary>
    private sealed class TestLocale : Locale
    {
        public override string this[string name, params object[] args] =>
            args.Length > 0 ? $"{name}({string.Join(", ", args)})" : name;

        public override Locale PlayerLanguage => this;

        public override ResourceSet? GetResourceSet() => null;

        public override string Translate(string pattern, params object[] args) => pattern;

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = this[binder.Name.Replace("_", ".", StringComparison.Ordinal)];
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            result = this[binder.Name.Replace("_", ".", StringComparison.Ordinal), args!];
            return true;
        }
    }
}
