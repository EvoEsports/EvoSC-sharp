using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

using ITaskItem = Microsoft.Build.Framework.ITaskItem;

namespace EvoSC.Modules.Build;

/// <summary>
/// Detects types decorated with the module export attribute (<c>[Export]</c>) in a set
/// of source files, computes the transitive closure of all in-project types their public
/// members (interface methods and properties) reference, and emits those into a separate
/// export assembly (<c>$(AssemblyName).Exports.dll</c>).
///
/// Only the entry-point interfaces need the <c>[Export]</c> attribute: every additional
/// type they mention (DTOs, enums, nested types, ...) is discovered automatically from
/// the interface's methods and properties.
///
/// The export assembly is produced at whole-file granularity: any source file that
/// contains at least one closure type is moved entirely out of the main assembly and
/// into the export assembly. The main assembly then references the export assembly.
/// </summary>
public sealed class ExportAssemblyAnalyzer : Microsoft.Build.Utilities.Task
{
    /// <summary>All <c>Compile</c> items of the module project.</summary>
    [Required]
    public ITaskItem[] Sources { get; set; } = [];

    /// <summary>Resolved references of the module project (<c>ReferencePathWithRefAssemblies</c>).</summary>
    [Required]
    public ITaskItem[] References { get; set; } = [];

    /// <summary>
    /// Fully qualified name of the attribute that marks export entry-point types,
    /// e.g. <c>EvoSC.Modules.ExportAttribute</c>.
    /// </summary>
    public string ExportAttribute { get; set; } = "EvoSC.Modules.ExportAttribute";

    /// <summary>Full path of the export assembly to emit, or empty to only analyze.</summary>
    [Required]
    public string OutputAssembly { get; set; } = "";

    /// <summary>Assembly name for the export assembly, e.g. <c>MyModule.Exports</c>.</summary>
    public string ExportAssemblyName { get; set; } = "";

    public string LangVersion { get; set; } = "";

    /// <summary>Semicolon-separated preprocessor symbols.</summary>
    public string DefineConstants { get; set; } = "";

    public bool AllowUnsafeBlocks { get; set; }

    public bool CheckForOverflowUnderflow { get; set; }

    /// <summary><c>enable</c>, <c>disable</c>, <c>warnings</c>, or <c>annotations</c>.</summary>
    public string Nullable { get; set; } = "disable";

    public bool Deterministic { get; set; } = true;

    public bool EmitDebugInformation { get; set; }

    /// <summary>The <c>Compile</c> items that belong to the export assembly.</summary>
    [Output]
    public ITaskItem[] ExportCompileItems { get; set; } = [];

    /// <summary>True when the project declares at least one export entry-point type.</summary>
    [Output]
    public bool HasExportAssembly { get; set; }

    public override bool Execute()
    {
        if (Sources.Length == 0)
        {
            HasExportAssembly = false;
            return true;
        }

        var languageVersion = ParseLanguageVersion();
        var defines = (DefineConstants ?? string.Empty)
            .Split([';', ','], StringSplitOptions.RemoveEmptyEntries);
        var parseOptions = new CSharpParseOptions(languageVersion, preprocessorSymbols: defines);

        var trees = Sources
            .Select(s => ToTree(s, parseOptions))
            .Where(t => t is not null)
            .Select(t => t!)
            .ToArray();

        var references = References
            .Select(r => r.GetMetadata("FullPath"))
            .Where(p => !string.IsNullOrEmpty(p) && System.IO.File.Exists(p))
            .Select(p => (PortableExecutableReference)MetadataReference.CreateFromFile(p))
            .ToArray();

        var nullableOptions = NullableContextOptionsFromProperty(Nullable);
        var probeOptions = new CSharpCompilationOptions(
            OutputKind.DynamicallyLinkedLibrary,
            nullableContextOptions: nullableOptions,
            allowUnsafe: AllowUnsafeBlocks,
            checkOverflow: CheckForOverflowUnderflow,
            optimizationLevel: OptimizationLevel.Debug,
            deterministic: Deterministic);

        var probe = CSharpCompilation.Create(
            ExportAssemblyName.Length > 0 ? ExportAssemblyName : "ExportProbe",
            trees, references, probeOptions);

        var roots = FindExportTypes(probe);
        if (roots.Count == 0)
        {
            HasExportAssembly = false;
            return true;
        }

        HasExportAssembly = true;

        var exportFiles = ComputeClosureTypes(probe, roots);
        ExportCompileItems = Sources
            .Where(s => exportFiles.Contains(s.GetMetadata("FullPath")))
            .ToArray();

        if (string.IsNullOrEmpty(OutputAssembly))
        {
            return true;
        }

        if (IsUpToDate(OutputAssembly, exportFiles, references))
        {
            return true;
        }

        var exportTrees = trees.Where(t => exportFiles.Contains(t.FilePath)).ToHashSet();

        // Export files rely on the project's implicit/global usings when compiled on
        // their own, so carry over every source that only declares global usings
        // (the SDK's generated <Project>.GlobalUsings.g.cs and any user-defined
        // global-usings file). Whole-file export granularity never pulls them in,
        // because such files declare no types.
        foreach (var tree in trees)
        {
            if (IsGlobalUsingsOnly(tree))
            {
                exportTrees.Add(tree);
            }
        }

        var fullAssemblyName = ExportAssemblyName.Length > 0 ? ExportAssemblyName : "ExportAssembly";
        var exportCompilation = CSharpCompilation.Create(
            fullAssemblyName,
            exportTrees,
            references,
            probeOptions.WithModuleName(fullAssemblyName + ".dll"));

        var outputDirectory = System.IO.Path.GetDirectoryName(OutputAssembly!);
        if (!string.IsNullOrEmpty(outputDirectory))
        {
            System.IO.Directory.CreateDirectory(outputDirectory);
        }

        using var peStream = new System.IO.MemoryStream();
        using var pdbStream = EmitDebugInformation
            ? new System.IO.MemoryStream()
            : null;

        var emitOptions = new EmitOptions(
            debugInformationFormat: DebugInformationFormat.PortablePdb,
            pdbFilePath: EmitDebugInformation ? OutputAssembly + ".pdb" : null);

        var result = exportCompilation.Emit(
            peStream,
            pdbStream: pdbStream,
            xmlDocumentationStream: null,
            win32Resources: null,
            manifestResources: null,
            options: emitOptions);

        if (!result.Success)
        {
            foreach (var diagnostic in result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
            {
                Log.LogError(diagnostic.ToString());
            }

            return false;
        }

        System.IO.File.WriteAllBytes(OutputAssembly, peStream.ToArray());
        if (pdbStream is not null)
        {
            System.IO.File.WriteAllBytes(OutputAssembly + ".pdb", pdbStream.ToArray());
        }

        return true;
    }

    private static SyntaxTree? ToTree(ITaskItem item, CSharpParseOptions parseOptions)
    {
        var path = item.GetMetadata("FullPath");
        if (string.IsNullOrEmpty(path) || !System.IO.File.Exists(path))
        {
            return null;
        }

        return SyntaxFactory.ParseSyntaxTree(
            System.IO.File.ReadAllText(path), parseOptions, path: path,
            encoding: System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// True when the tree contains nothing but <c>global using</c> directives
    /// (i.e. it is a global-usings carrier, not a type-bearing source file).
    /// </summary>
    private static bool IsGlobalUsingsOnly(SyntaxTree tree)
    {
        if (tree.GetRoot() is not Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax unit)
        {
            return false;
        }

        return unit.Usings.Count > 0
               && unit.Usings.All(u => u.GlobalKeyword.Kind() == SyntaxKind.GlobalKeyword);
    }

    private LanguageVersion ParseLanguageVersion()
    {
        var value = string.IsNullOrWhiteSpace(LangVersion) ? "latest" : LangVersion;

        return value switch
        {
            "default" => LanguageVersion.Default,
            "latest" => LanguageVersion.Latest,
            "latestMajor" => LanguageVersion.LatestMajor,
            "preview" => LanguageVersion.Preview,
            _ when Enum.TryParse<LanguageVersion>(value, out var parsed) => parsed,
            _ => LanguageVersion.Latest
        };
    }

    private static NullableContextOptions NullableContextOptionsFromProperty(string nullable)
    {
        return nullable switch
        {
            "enable" => NullableContextOptions.Enable,
            "annotations" => NullableContextOptions.Annotations,
            "warnings" => NullableContextOptions.Warnings,
            _ => NullableContextOptions.Disable
        };
    }

    private HashSet<INamedTypeSymbol> FindExportTypes(CSharpCompilation compilation)
    {
        var roots = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);

        foreach (var tree in compilation.SyntaxTrees)
        {
            var model = compilation.GetSemanticModel(tree);
            var declarations = tree.GetRoot().DescendantNodes()
                .OfType<BaseTypeDeclarationSyntax>();

            foreach (var declaration in declarations)
            {
                var declared = model.GetDeclaredSymbol(declaration);
                if (declared is null)
                {
                    continue;
                }

                if (declared is not INamedTypeSymbol symbol || !HasExportAttribute(symbol))
                {
                    continue;
                }

                roots.Add(symbol);
            }
        }

        return roots;
    }

    private bool HasExportAttribute(INamedTypeSymbol type)
    {
        foreach (var attribute in type.GetAttributes())
        {
            if (attribute.AttributeClass is null)
            {
                continue;
            }

            var name = attribute.AttributeClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (Matches(name, ExportAttribute))
            {
                return true;
            }
        }

        return false;
    }

    private static bool Matches(string candidate, string fullName)
    {
        if (candidate.StartsWith("global::", StringComparison.Ordinal))
        {
            candidate = candidate.Substring("global::".Length);
        }

        var normalized = fullName.StartsWith("global::", StringComparison.Ordinal)
            ? fullName.Substring("global::".Length)
            : fullName;

        return string.Equals(candidate, normalized, StringComparison.Ordinal);
    }

    private HashSet<string> ComputeClosureTypes(CSharpCompilation compilation, HashSet<INamedTypeSymbol> roots)
    {
        var visited = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        var files = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var assembly = compilation.Assembly;

        void VisitNamed(INamedTypeSymbol type)
        {
            type = type.OriginalDefinition;
            if (!visited.Add(type))
            {
                return;
            }

            if (!SymbolEqualityComparer.Default.Equals(type.ContainingAssembly, assembly))
            {
                return;
            }

            foreach (var location in type.Locations)
            {
                if (location.IsInSource && location.SourceTree is not null)
                {
                    files.Add(location.SourceTree.FilePath);
                }
            }

            if (type.BaseType is not null)
            {
                VisitNamed(type.BaseType);
            }

            foreach (var interfaceType in type.Interfaces)
            {
                VisitNamed(interfaceType);
            }

            foreach (var member in type.GetMembers())
            {
                if (member.DeclaredAccessibility != Accessibility.Public)
                {
                    continue;
                }

                switch (member)
                {
                    case IMethodSymbol method:
                        VisitType(method.ReturnType);
                        foreach (var parameter in method.Parameters)
                        {
                            VisitType(parameter.Type);
                        }

                        break;
                    case IPropertySymbol property:
                        VisitType(property.Type);
                        foreach (var parameter in property.Parameters)
                        {
                            VisitType(parameter.Type);
                        }

                        break;
                    case IEventSymbol eventType:
                        VisitType(eventType.Type);
                        break;
                    case IFieldSymbol field:
                        VisitType(field.Type);
                        break;
                }
            }
        }

        void VisitType(ITypeSymbol type)
        {
            switch (type)
            {
                case IArrayTypeSymbol array:
                    VisitType(array.ElementType);
                    break;
                case IPointerTypeSymbol pointer:
                    VisitType(pointer.PointedAtType);
                    break;
                case IFunctionPointerTypeSymbol functionPointer:
                    VisitType(functionPointer.Signature.ReturnType);
                    foreach (var parameter in functionPointer.Signature.Parameters)
                    {
                        VisitType(parameter.Type);
                    }

                    break;
                case INamedTypeSymbol named when named.TypeKind != TypeKind.Error:
                    foreach (var typeArgument in named.TypeArguments)
                    {
                        VisitType(typeArgument);
                    }

                    VisitNamed(named);
                    break;
            }
        }

        foreach (var root in roots)
        {
            VisitNamed(root);
        }

        return files;
    }

    private static bool IsUpToDate(string outputAssembly,
        HashSet<string> exportFiles,
        IEnumerable<PortableExecutableReference> references)
    {
        if (!System.IO.File.Exists(outputAssembly) || new System.IO.FileInfo(outputAssembly).Length == 0)
        {
            return false;
        }

        var outputTime = System.IO.File.GetLastWriteTimeUtc(outputAssembly);
        foreach (var file in exportFiles)
        {
            if (!System.IO.File.Exists(file) || System.IO.File.GetLastWriteTimeUtc(file) > outputTime)
            {
                return false;
            }
        }

        foreach (var reference in references)
        {
            if (reference.FilePath is { } path &&
                (!System.IO.File.Exists(path) || System.IO.File.GetLastWriteTimeUtc(path) > outputTime))
            {
                return false;
            }
        }

        return true;
    }
}