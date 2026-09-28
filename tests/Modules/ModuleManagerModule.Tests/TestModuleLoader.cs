using System.Reflection;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Middleware;
using EvoSC.Modules;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Models;
using Moq;
using SimpleInjector;

namespace EvoSC.Modules.Official.ModuleManagerModule.Tests;

internal static class TestModuleLoader
{
    private static readonly MethodInfo SetEnabledMethod = typeof(IModuleLoadContext)
        .GetMethod("SetEnabled", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly MethodInfo SetStatusMethod = typeof(IModuleLoadContext)
        .GetMethod("SetStatus", BindingFlags.Instance | BindingFlags.NonPublic)!;

    public static IModuleLoadContext CreateModule(string id, ModuleStatus status = ModuleStatus.Enabled,
        bool isInternal = false, bool isEnabled = true, List<Guid>? loadedDependencies = null)
    {
        var module = new ModuleLoadContext
        {
            Instance = null,
            Services = new Container(),
            AsmLoadContext = null,
            LoadId = Guid.NewGuid(),
            MainClass = null,
            ModuleInfo = CreateModuleInfo(id, isInternal),
            Assemblies = Array.Empty<Assembly>(),
            Pipelines = new Dictionary<PipelineType, IActionPipeline>(),
            Permissions = new List<IPermission>(),
            LoadedDependencies = loadedDependencies ?? new List<Guid>(),
            ManialinkTemplates = new List<IModuleManialinkTemplate>(),
            RootNamespace = "Test",
            Localization = null,
            Themes = Array.Empty<Type>()
        };

        SetEnabledMethod.Invoke(module, new object[] {isEnabled});
        SetStatusMethod.Invoke(module, new object[] {status});

        return module;
    }

    public static IModuleInfo CreateModuleInfo(string id, bool isInternal = false)
    {
        var moduleInfo = new Mock<IModuleInfo>();
        moduleInfo.SetupGet(m => m.Id).Returns(id);
        moduleInfo.SetupGet(m => m.Name).Returns($"{id} name");
        moduleInfo.SetupGet(m => m.Version).Returns(new Version(1, 2, 3));
        moduleInfo.SetupGet(m => m.IsInternal).Returns(isInternal);

        return moduleInfo.Object;
    }
}