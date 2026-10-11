using System.Dynamic;
using System.Reflection;
using System.Resources;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Manialinks;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Manialinks.Models;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.ModuleManagerModule.Services;
using GbxRemoteNet.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EvoSC.Modules.Official.ModuleManagerModule.Tests;

/// <summary>
/// Renders the module manager window through the real ManiaTemplates engine. A template is only
/// ever compiled at runtime (see ModuleManager.RegisterManialinksTemplatesAsync), so a successful
/// `dotnet build` says nothing about whether ModuleManagerWindow.mt or its ModuleRow component are
/// even valid ManiaScript/C# - only actually rendering them does.
/// </summary>
public class ModuleManagerWindowTemplateTests
{
    private const string WindowTemplateName = "ModuleManagerModule.ModuleManagerWindow";
    private const string RowTemplateName = "ModuleManagerModule.Components.ModuleRow";

    [Fact]
    public async Task Window_Renders_Every_Module_Status_And_Hides_Lifecycle_Buttons_For_Internal_Modules()
    {
        var manager = CreateManialinkManager();
        await manager.AddDefaultTemplatesAsync();
        RegisterTemplate(manager, WindowTemplateName, "ModuleManagerWindow.mt");
        RegisterTemplate(manager, RowTemplateName, "Components/ModuleRow.mt");

        var modules = new[]
        {
            TestModuleLoader.CreateModule("EnabledModule", ModuleStatus.Enabled),
            TestModuleLoader.CreateModule("LoadedModule", ModuleStatus.Loaded),
            TestModuleLoader.CreateModule("DisabledModule", ModuleStatus.Disabled),
            TestModuleLoader.CreateModule("ErroredModule", ModuleStatus.Error),
            TestModuleLoader.CreateModule("CoreModule", ModuleStatus.Enabled, isInternal: true),
        };

        var output = await manager.PrepareAndRenderAsync(WindowTemplateName,
            new { modules, Locale = new TestLocale() });

        Assert.Contains("EnabledModule", output);
        Assert.Contains("LoadedModule", output);
        Assert.Contains("DisabledModule", output);
        Assert.Contains("ErroredModule", output);
        Assert.Contains("CoreModule", output);

        // Internal modules can't be reloaded, unloaded or uninstalled, so the window must not offer
        // those actions for one - only the Enable/Disable toggle, which internal modules allow too.
        Assert.DoesNotContain("ReloadModule/CoreModule", output);
        Assert.DoesNotContain("UnloadModule/CoreModule", output);
        Assert.DoesNotContain("UninstallModule/CoreModule", output);
        Assert.Contains("DisableModule/CoreModule", output);

        // A non-internal, enabled module gets the full set of lifecycle actions.
        Assert.Contains("DisableModule/EnabledModule", output);
        Assert.Contains("ReloadModule/EnabledModule", output);
        Assert.Contains("UnloadModule/EnabledModule", output);
        Assert.Contains("UninstallModule/EnabledModule", output);

        // A disabled module offers Enable, not Disable.
        Assert.Contains("EnableModule/DisabledModule", output);
        Assert.DoesNotContain("DisableModule/DisabledModule", output);
    }

    [Fact]
    public async Task Window_Renders_With_No_Modules_Loaded()
    {
        var manager = CreateManialinkManager();
        await manager.AddDefaultTemplatesAsync();
        RegisterTemplate(manager, WindowTemplateName, "ModuleManagerWindow.mt");
        RegisterTemplate(manager, RowTemplateName, "Components/ModuleRow.mt");

        var output = await manager.PrepareAndRenderAsync(WindowTemplateName,
            new { modules = Array.Empty<IModuleLoadContext>(), Locale = new TestLocale() });

        // The Window component upper-cases the title it's given.
        Assert.Contains("MODULEMANAGERWINDOWTITLE", output);
    }

    private static void RegisterTemplate(ManialinkManager manager, string name, string relativePath)
    {
        var path = Path.Combine(TemplatesDirectory, relativePath);
        manager.AddTemplate(new ManialinkTemplateInfo
        {
            Assemblies = [typeof(ModuleManagerService).Assembly],
            Name = name,
            Content = File.ReadAllText(path)
        });
    }

    private static string TemplatesDirectory =>
        Path.Combine(FindRepoRoot(AppContext.BaseDirectory), "src", "Modules", "ModuleManagerModule", "Templates");

    private static string FindRepoRoot(string start)
    {
        var dir = new DirectoryInfo(start);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "EvoSC.sln")))
        {
            dir = dir.Parent;
        }

        return dir?.FullName ?? throw new InvalidOperationException("Could not find the repository root (EvoSC.sln).");
    }

    private static ManialinkManager CreateManialinkManager()
    {
        var themes = new Mock<IThemeManager>();
        themes.Setup(t => t.ComponentReplacements).Returns(new Dictionary<string, string>());
        themes.Setup(t => t.Theme).Returns(new PermissiveThemeOptions());

        var server = new Mock<IServerClient>();
        server.Setup(s => s.Remote).Returns(new Mock<IGbxRemoteClient>().Object);

        return new ManialinkManager(
            NullLogger<ManialinkManager>.Instance,
            server.Object,
            new Mock<IEventManager>().Object,
            themes.Object,
            new Mock<IPlayerManagerService>().Object);
    }

    /// <summary>
    /// Stands in for the real theme options proxy, which computes its properties and helper
    /// methods (like Button.mt's Theme.UI_Button_Size(size, height)) from a chain of registered
    /// Theme&lt;T&gt; classes - too deep a system to replicate here. Answers any property or method
    /// with a placeholder value, so shared components that read from Theme still compile and
    /// render; this test only cares that they do, not what they look like.
    /// </summary>
    private sealed class PermissiveThemeOptions : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            // Numeric by default so arithmetic on a size/opacity-like property doesn't throw; a
            // handful of properties are read as strings (a color, the font family) and need a
            // value of that shape instead.
            result = binder.Name switch
            {
                "UI_Font" => "GameFont", // FontManialinkHelper only accepts a handful of font families.
                _ when binder.Name.Contains("Opacity", StringComparison.OrdinalIgnoreCase) => 1.0,
                _ when binder.Name.Contains("Bg", StringComparison.OrdinalIgnoreCase)
                    || binder.Name.Contains("Color", StringComparison.OrdinalIgnoreCase)
                    || binder.Name.Contains("Text", StringComparison.OrdinalIgnoreCase) => "FFFFFF",
                _ => 5.0
            };
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            result = 5.0;
            return true;
        }
    }

    /// <summary>Echoes back whatever locale key is asked for, so rendering never fails on a missing key.</summary>
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
