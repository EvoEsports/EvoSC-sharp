using System.Globalization;
using Config.Net;

namespace EvoSC.Common.Config.Stores;

public class EvSCModuleConfigStore : IConfigStore
{
    private const string EnviKeyPrefix = "EVOSC_MODULE_";
    
    private readonly DatabaseStore _dbStore;
    private readonly string _moduleName;
    
    public EvSCModuleConfigStore(string moduleName, DatabaseStore dbStore)
    {
        _dbStore = dbStore;
        _moduleName = moduleName.ToUpper(CultureInfo.InvariantCulture);
    }

    public string? Read(string key)
    {
        var enviKey = MakeEnviKey(key);
        var enviValue = Environment.GetEnvironmentVariable(enviKey);

        if (enviValue != null)
        {
            return enviValue;
        }

        return _dbStore.Read(key);
    }

    public void Write(string key, string? value)
    {
        // in-case envis are used to override, set the envi value so that it is updated to be read
        var enviKey = MakeEnviKey(key);
        
        if (Environment.GetEnvironmentVariable(key) != null)
        {
            Environment.SetEnvironmentVariable(enviKey, value);
        }
        
        _dbStore.Write(key, value);
    }

    public bool CanRead => true;
    public bool CanWrite => true;

    public void Dispose() => _dbStore.Dispose();

    private string MakeEnviKey(string key)
    {
        var enviConverted = key.Replace('.', '_').ToUpper(CultureInfo.InvariantCulture);
        return $"{EnviKeyPrefix}{_moduleName}_{enviConverted}";
    }
}
