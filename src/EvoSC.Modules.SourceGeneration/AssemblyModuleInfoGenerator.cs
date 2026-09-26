using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Tomlet;

namespace EvoSC.Modules.SourceGeneration
{
    /// <summary>
    /// Generates an Info.g.cs file from an info.toml file. The file contains assembly information
    /// for a module which is mostly used for loading internal modules. It gives a way to seamlessly
    /// implement internal and external modules together to ease with development.
    /// </summary>
    [Generator]
    public class AssemblyModuleInfoGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // not needed for this
        }

        private static void FileNotFoundError(GeneratorExecutionContext context)
        {
            var errorMessage = new StringBuilder();

            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var dir);
            
            errorMessage.AppendLine($"#error Source Generator Error (Module: {context.Compilation.Assembly.Name}): ");
            errorMessage.AppendLine("#error Failed to generate the module's assembly info.");
            errorMessage.AppendLine("#error You must provide a \"info.toml\" file in the module's root namespace.");
            errorMessage.AppendLine("#error For more information, refer to the module documentation.");
            errorMessage.AppendLine();
            errorMessage.AppendLine($"#error Module directory: {dir ?? "<does not exist>"}");
                
            context.AddSource("Info.g.cs", errorMessage.ToString());
        }

        private static void ParserError(GeneratorExecutionContext context, string message)
        {
            var errorMessage = new StringBuilder();
                
            errorMessage.AppendLine("#error Source Generator Error: ");
            errorMessage.AppendLine("#error Failed to generate the module's assembly info.");
            errorMessage.AppendLine($"#error Failed to parse info.toml: {message}");
                
            context.AddSource("Info.g.cs", errorMessage.ToString());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var infoToml = context.AdditionalFiles.FirstOrDefault(text =>
                string.Equals(Path.GetFileName(text.Path), "info.toml", StringComparison.OrdinalIgnoreCase));

            if (infoToml == null)
            {
                FileNotFoundError(context);
                return;
            }

            try
            {
                var document = new TomlParser().Parse(infoToml.GetText()?.ToString() ?? string.Empty);

                var moduleIdentifier = document.GetValue("info.id").StringValue;
                var moduleName = document.GetValue("info.name").StringValue;
                var moduleSummary = document.GetValue("info.summary").StringValue;
                var moduleVersion = document.GetValue("info.version").StringValue;
                var moduleAuthor = document.GetValue("info.author").StringValue;

                var source = new StringBuilder();

                source.AppendLine("using EvoSC.Modules.Attributes;");
                source.AppendLine();
                source.AppendLine($"[assembly: ModuleIdentifier(\"{moduleIdentifier}\")]");
                source.AppendLine($"[assembly: ModuleName(\"{moduleName}\")]");
                source.AppendLine($"[assembly: ModuleSummary(\"{moduleSummary}\")]");
                source.AppendLine($"[assembly: ModuleVersion(\"{moduleVersion}\")]");
                source.AppendLine($"[assembly: ModuleAuthor(\"{moduleAuthor}\")]");

                if (document.ContainsKey("dependencies"))
                {
                    var dependencies = document.GetSubTable("dependencies");
                    foreach (var (name, value) in dependencies.Entries
                                 .Select(entry => (entry.Key, Value: dependencies.GetValue(entry.Key).StringValue)))
                    {
                        source.AppendLine($"[assembly: ModuleDependency(\"{name}\", \"{value}\")]");
                    }
                }
                
                context.AddSource("Info.g.cs", source.ToString());
            }
            catch (Exception ex)
            {
                ParserError(context, ex.Message);
            }
        }
    }
}
