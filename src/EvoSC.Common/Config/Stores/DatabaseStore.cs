using System.Reflection;
using Config.Net;
using EvoSC.Common.Database.Models.Config;
using EvoSC.Common.Interfaces.Database.Repository;
using EvoSC.Common.Util;

namespace EvoSC.Common.Config.Stores;

public class DatabaseStore(string prefix, Type type, IConfigStoreRepository configStoreRepository)
    : IConfigStore
{
    private readonly string _prefix = $"{prefix}.{GetSettingsName(type)}";

    private static string GetSettingsName(Type type)
    {
        return type.Name[0] == 'I' ? type.Name.Substring(1) : type.Name;
    }

    public Task SetupDefaultSettingsAsync() => SetupDefaultSettingsRecursiveAsync(type, _prefix);

    private async Task SetupDefaultSettingsRecursiveAsync(Type type, string name)
    {
        foreach (var property in type.GetProperties())
        {
            var keyName = name;
            var optionAttr = property.GetCustomAttribute<OptionAttribute>();

            // get property's key name
            if (optionAttr?.Alias != null)
            {
                keyName += $".{optionAttr.Alias}";
            }
            else
            {
                keyName += $".{property.Name}";
            }
            
            if (property.PropertyType.IsInterface)
            {
                await SetupDefaultSettingsRecursiveAsync(property.PropertyType, keyName);
            }
            else
            {
                var option = await configStoreRepository.GetConfigOptionsByKeyAsync(keyName);

                if (option == null)
                {
                    // option not set, so add it's defaults to the db
                    await configStoreRepository.AddConfigOptionAsync(new DbConfigOption
                    {
                        Key = keyName,
                        Value = optionAttr?.DefaultValue?.ToString() ??
                                ReflectionUtils.GetDefaultTypeValue(property.PropertyType)?.ToString() ??
                                throw new InvalidOperationException("Failed to set config option as it became null.")
                    });
                }
            }
        }
    }

    public void Dispose()
    {
        // nothing to dispose
    }

    public string? Read(string key)
    {
        var dbKey = $"{_prefix}.{key}";
        var option = configStoreRepository.GetConfigOptionsByKeyAsync(dbKey).GetAwaiter().GetResult();

        if (option == null)
        {
            throw new KeyNotFoundException($"Database configuration option '{key}' for prefix '{_prefix}' not found");
        }

        return option.Value;
    }

    public void Write(string key, string? value)
    {
        var dbKey = $"{_prefix}.{key}";
        var option = configStoreRepository.GetConfigOptionsByKeyAsync(dbKey).Result;

        if (option == null)
        {
            option = new DbConfigOption {Key = $"{_prefix}.{key}", Value = value};
            
            configStoreRepository.AddConfigOptionAsync(new DbConfigOption
            {
                Key = option.Key,
                Value = option.Value ?? ""
            }).GetAwaiter().GetResult();
        }
        else
        {
            option.Value = value;
            configStoreRepository.UpdateConfigOptionAsync(option).GetAwaiter().GetResult();
        }
    }

    public bool CanRead => true;
    public bool CanWrite => true;
}
