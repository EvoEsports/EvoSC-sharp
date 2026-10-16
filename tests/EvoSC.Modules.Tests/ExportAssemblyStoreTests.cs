using EvoSC.Modules;

namespace EvoSC.Modules.Tests;

/// <summary>
/// Direct <see cref="ExportAssemblyStore"/> tests using the provider export produced by the
/// build. Binary compatibility is intentionally not enforced manually — the runtime binder
/// throws when a module uses an export API it was not compiled against.
/// </summary>
[Collection("ModuleSystem")]
public class ExportAssemblyStoreTests
{
    private const string ProviderModuleId = "ContractApiProviderModule";
    private const string ProviderExportName = "ContractApiProviderModule.Exports";

    private static string ProviderExportPath => Path.Combine(
        AppContext.BaseDirectory,
        "modules",
        "ContractApiProviderModule",
        $"{ProviderExportName}.dll");

    private static ExportAssemblyStore CreateStore()
        => new();

    [Fact]
    public void Store_AcquiresExportFromRegisteredPath()
    {
        Assert.True(File.Exists(ProviderExportPath), "The provider export assembly must be emitted by the build.");

        var store = CreateStore();
        store.RegisterExportPath(ProviderModuleId, ProviderExportPath);

        var assembly = store.AcquireExportForModule(Guid.NewGuid(), ProviderModuleId);

        Assert.NotNull(assembly);
        Assert.True(store.IsExportAssembly(assembly));
    }

    [Fact]
    public void Store_LoadsExportOnDemand_OnlyAfterRegisteringPath()
    {
        var store = CreateStore();

        var exception = Assert.Throws<Exceptions.EvoScModuleException>(
            () => store.AcquireExportForModule(Guid.NewGuid(), "SomeModuleThatShipsNoExport"));
        Assert.Contains("No export assembly registered", exception.Message);
    }
}