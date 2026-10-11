namespace EvoSC.Modules;

/// <summary>
/// Marks a type as part of a module's export surface. Only entry-point types need the
/// attribute: every additional type referenced by their public API (DTOs, enums, nested
/// types, ...) is discovered automatically at build time and emitted into the module's
/// export assembly (<c>&lt;AssemblyName&gt;.Exports.dll</c>).
/// </summary>
[AttributeUsage(AttributeTargets.Interface
    | AttributeTargets.Class
    | AttributeTargets.Struct
    | AttributeTargets.Enum
    | AttributeTargets.Delegate,
    Inherited = false)]
public sealed class ExportAttribute : Attribute
{
}