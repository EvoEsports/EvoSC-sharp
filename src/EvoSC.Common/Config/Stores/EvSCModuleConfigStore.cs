using System.Globalization;
using Config.Net;

namespace EvoSC.Common.Config.Stores;

public class EvSCModuleConfigStore(string moduleName, DatabaseStore dbStore) : IConfigStore
{
    private const string EnviKeyPrefix = "EVOSC_MODULE_";

    private readonly string _moduleName = moduleName.ToUpper(CultureInfo.InvariantCulture);

    public string? Read(string key)
    {
        var enviKey = MakeEnviKey(key);
        var enviValue = Environment.GetEnvironmentVariable(enviKey);

        if (enviValue != null)
        {
            return enviValue;
        }

        return dbStore.Read(key);
    }

    public void Write(string key, string? value)
    {
        // in-case envis are used to override, set the envi value so that it is updated to be read
        var enviKey = MakeEnviKey(key);
        
        if (Environment.GetEnvironmentVariable(key) != null)
        {
            Environment.SetEnvironmentVariable(enviKey, value);
        }
        
        dbStore.Write(key, value);
    }

    public bool CanRead => true;
    public bool CanWrite => true;

    public void Dispose() => dbStore.Dispose();

    private string MakeEnviKey(string key)
    {
        var enviConverted = key.Replace('.', '_').ToUpper(CultureInfo.InvariantCulture);
        return $"{EnviKeyPrefix}{_moduleName}_{enviConverted}";
    }
}
