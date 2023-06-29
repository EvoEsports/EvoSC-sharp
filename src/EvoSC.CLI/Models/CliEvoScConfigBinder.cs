using System.CommandLine.Binding;

namespace EvoSC.CLI.Models;

public class CliEvoScConfigBinder : BinderBase<CliEvoScConfig>
{
    
    
    protected override CliEvoScConfig GetBoundValue(BindingContext bindingContext)
    {
        return new CliEvoScConfig {Options = new Dictionary<string, object>()};
    }
    
    
}
