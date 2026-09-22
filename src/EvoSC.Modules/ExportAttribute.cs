namespace EvoSC.Modules;

/// <summary>
/// Marks a type as part of a module's export surface. Apply it to the entry-point
/// interfaces (and any other types) whose types you want to expose to other modules.
///
/// Only the entry-point types need the attribute: every additional type referenced by
/// their public methods and properties (DTOs, enums, nested types, ...) is discovered
/// automatically at build time and emitted into the module's export assembly
/// (<c>&lt;AssemblyName&gt;.Exports.dll</c>) along with the annotated types.
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