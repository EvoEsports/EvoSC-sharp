using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using EvoSC.Modules;

namespace EvoSC.Modules.Tests;

/// <summary>
/// Proves the runtime binder enforces export binary compatibility: a module compiled against
/// one export API fails with <see cref="MissingMethodException"/> when loaded against an
/// incompatible export of the same identity.
/// </summary>
[Collection("ModuleSystem")]
public class ExportAssemblyBinaryCompatTests
{
    private const string ProviderModuleId = "ContractApiProviderModule";
    private const string ProviderExportName = "ContractApiProviderModule.Exports";
    private const string ProviderExportNamespace = "EvoSC.Modules.Official.ContractApiProviderModule.Contracts";

    private static string ProviderExportPath => Path.Combine(
        AppContext.BaseDirectory,
        "modules",
        "ContractApiProviderModule",
        $"{ProviderExportName}.dll");

    [Fact]
    public void Module_UsingBinaryIncompatibleExport_BinderThrowsMissingMethodException()
    {
        Assert.True(File.Exists(ProviderExportPath), "The provider export assembly must be emitted by the build.");

        var tempDir = Path.Combine(Path.GetTempPath(), "evosc-binary-compat-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        var v1ReferenceAlc = new AssemblyLoadContext("v1-export-reference-" + Guid.NewGuid().ToString("N"), isCollectible: true);
        var store = new ExportAssemblyStore();

        try
        {
            // Capture the API the consumer compiles against from the real (v1) export.
            var v1Export = v1ReferenceAlc.LoadFromAssemblyPath(ProviderExportPath);
            var v1Interface = v1Export.GetType($"{ProviderExportNamespace}.IScoreboardService")!;
            var v1Method = v1Interface.GetMethod("GetCurrentResult")!;

            // Emit an incompatible "v2" export: same assembly identity and interface name,
            // but without the API surface that v1 had.
            var v2Path = EmitIncompatibleExport(tempDir);
            store.RegisterExportPath(ProviderModuleId, v2Path);
            var v2Export = store.AcquireExportForModule(Guid.NewGuid(), ProviderModuleId);
            Assert.True(store.IsExportAssembly(v2Export));

            // Emit a "consumer" compiled against the v1 API (calls GetCurrentResult).
            var consumerPath = EmitConsumer(tempDir, v1Interface, v1Method);

            // Load the consumer into its own module load context whose exports resolve to
            // the incompatible v2 assembly — exactly the module runtime flow.
            var loadId = Guid.NewGuid();
            var moduleAlc = new EvoScModuleLoadContext(
                consumerPath,
                store.CreateExportResolver(loadId));
            try
            {
                var consumerAssembly = moduleAlc.LoadModuleAssembly(consumerPath);
                var call = consumerAssembly.GetType("BinaryIncompatConsumer.Consumer")!.GetMethod(
                    "Call", BindingFlags.Public | BindingFlags.Static)!;

                var exception = Assert.Throws<TargetInvocationException>(() => call.Invoke(null, new object?[] { null }));
                Assert.IsType<MissingMethodException>(exception.InnerException);
            }
            finally
            {
                moduleAlc.Unload();
            }
        }
        finally
        {
            v1ReferenceAlc.Unload();
            Directory.Delete(tempDir, recursive: true);
        }
    }

    private static string EmitIncompatibleExport(string tempDir)
    {
        var v2Path = Path.Combine(tempDir, $"{ProviderExportName}.dll");
        var assemblyBuilder = new PersistedAssemblyBuilder(new AssemblyName(ProviderExportName), typeof(object).Assembly);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("Main");

        var interfaceBuilder = moduleBuilder.DefineType(
            $"{ProviderExportNamespace}.IScoreboardService",
            TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);

        interfaceBuilder.CreateType();
        assemblyBuilder.Save(v2Path);
        return v2Path;
    }

    private static string EmitConsumer(string tempDir, Type scoreboardInterface, MethodInfo scoreboardMethod)
    {
        var consumerPath = Path.Combine(tempDir, "BinaryIncompatConsumer.dll");
        var assemblyBuilder = new PersistedAssemblyBuilder(new AssemblyName("BinaryIncompatConsumer"), typeof(object).Assembly);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("Main");

        var consumerBuilder = moduleBuilder.DefineType(
            "BinaryIncompatConsumer.Consumer",
            TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);

        var methodBuilder = consumerBuilder.DefineMethod(
            "Call",
            MethodAttributes.Public | MethodAttributes.Static,
            typeof(void),
            new[] { scoreboardInterface });

        var il = methodBuilder.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Callvirt, scoreboardMethod);
        il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ret);

        consumerBuilder.CreateType();
        assemblyBuilder.Save(consumerPath);
        return consumerPath;
    }
}