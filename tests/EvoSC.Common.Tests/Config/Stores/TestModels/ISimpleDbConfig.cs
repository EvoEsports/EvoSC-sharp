using Config.Net;

namespace EvoSC.Common.Tests.Config.Stores.TestModels;

public interface ISimpleDbConfig
{
    [Option(DefaultValue = "Test")]
    public string MyOption { get; set; }
    
    [Option(Alias = "myAlias")]
    public string MyOption2 { get; set; }
    
    [Option]
    public ISimpleSubOption MySubOption { get; set; }
}
