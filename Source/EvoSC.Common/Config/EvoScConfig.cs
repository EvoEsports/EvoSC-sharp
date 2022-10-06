using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Common.Config;

public class EvoScConfig : IConfig
{
    public const string ConfigDir = "config";
    public const string MainConfigFile = "main.toml";
    
    public const string ServerConfigKey = "server";
    public const string LoggingConfigKey = "logging";
    public const string DatabaseConfigKey = "database";

    private TomlDocument _document;
    
    public EvoScConfig(string configDir)
    {
        var mainConfigFile = GetConfigFilePath(configDir);

        if (mainConfigFile == null)
        {
            _document = CreateDefaultConfig();
            mainConfigFile = Path.Combine(configDir, MainConfigFile);
            File.WriteAllText(mainConfigFile, _document.SerializedValue);
        }
        
        _document = TomlParser.ParseFile(mainConfigFile);
    }

    private TomlDocument CreateDefaultConfig()
    {
        var document = TomlDocument.CreateEmpty();
        
        document.Put(LoggingConfigKey, new LoggingConfig());
        document.Put(ServerConfigKey, new ServerConfig());
        document.Put(DatabaseConfigKey, new DatabaseConfig());

        return document;
    }

    private string? GetConfigFilePath(string configDir)
    {
        configDir = Path.GetFullPath(configDir);

        if (!Directory.Exists(configDir))
        {
            Directory.CreateDirectory(configDir);
        }

        var mainFile = Path.Combine(configDir, MainConfigFile);

        if (!File.Exists(mainFile))
        {
            return null;
        }

        return mainFile;
    }

    public TOption Get<TOption>(string key)
    {
        var value = _document.GetValue(key);
        return TomletMain.To<TOption>(value);
    }
}
