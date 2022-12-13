using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
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

        private void FileNotFoundError(GeneratorExecutionContext context)
        {
            var errorMessage = new StringBuilder();

            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var dir);
            
            errorMessage.AppendLine($"#error Source Generator Error (Module: {context.Compilation.Assembly.Name}): ");
            errorMessage.AppendLine("#error Failed to generate the module's assembly info.");
            errorMessage.AppendLine("#error You must provide a info.toml file in the module's root namespace.");
            errorMessage.AppendLine("#error For more information, refer to the module documentation.");
            errorMessage.AppendLine();
            errorMessage.AppendLine($"#error Module directory: {dir ?? "<does not exist>"}");
                
            context.AddSource("Info.g.cs", errorMessage.ToString());
        }

        private void ParserError(GeneratorExecutionContext context, string message)
        {
            var errorMessage = new StringBuilder();
                
            errorMessage.AppendLine("#error Source Generator Error: ");
            errorMessage.AppendLine("#error Failed to generate the module's assembly info.");
            errorMessage.AppendLine($"#error Failed to parse info.toml: {message}");
                
            context.AddSource("Info.g.cs", errorMessage.ToString());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.projectdir", out var dir);

            if (dir == null)
            {
                return;
            }
            
            var infoFile = Path.Combine(dir, "info.toml");

            if (!File.Exists(infoFile))
            {
                FileNotFoundError(context);
                return;
            }

            try
            {
                var document = TomlParser.ParseFile(infoFile);

                var moduleIdentifier = document.GetValue("info.name").StringValue;
                var moduleTitle = document.GetValue("info.title").StringValue;
                var moduleSummary = document.GetValue("info.summary").StringValue;
                var moduleVersion = document.GetValue("info.version").StringValue;
                var moduleAuthor = document.GetValue("info.author").StringValue;

                var source = new StringBuilder();

                source.AppendLine("using EvoSC.Modules.Attributes;");
                source.AppendLine();
                source.AppendLine($"[assembly: ModuleIdentifier(\"{moduleIdentifier}\")]");
                source.AppendLine($"[assembly: ModuleTitle(\"{moduleTitle}\")]");
                source.AppendLine($"[assembly: ModuleSummary(\"{moduleSummary}\")]");
                source.AppendLine($"[assembly: ModuleVersion(\"{moduleVersion}\")]");
                source.AppendLine($"[assembly: ModuleAuthor(\"{moduleAuthor}\")]");

                context.AddSource("Info.g.cs", source.ToString());
            }
            catch (Exception ex)
            {
                ParserError(context, ex.Message);
            }
        }
    }
}
