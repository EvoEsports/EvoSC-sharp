using System.Data.Common;
using System.Reflection;
using Config.Net;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models;
using EvoSC.Common.Util;

namespace EvoSC.Common.Config.Stores;

public class DatabaseStore : IConfigStore
{
    private readonly DbConnection _db;
    private readonly Type _type;
    private readonly string _prefix;
    
    public DatabaseStore(string prefix, Type type, DbConnection db)
    {
        _prefix = $"{prefix}.{GetSettingsName(type)}";
        _db = db;
        _type = type;
    }
    
    private string GetSettingsName(Type type)
    {
        return type.Name[0] == 'I' ? type.Name.Substring(1) : type.Name;
    }

    public Task SetupDefaultSettingsAsync() => SetupDefaultSettingsRecursiveAsync(_type, _prefix);

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
                var option = (await _db
                        .QueryAsync<DbConfigOptions>("select * from `ConfigOptions` where `Key`=@Key",
                            new {Key = keyName}))
                    .FirstOrDefault();

                if (option == null)
                {
                    // option not set, so add it's defaults to the db
                    await _db.QueryAsync("insert into `ConfigOptions`(`Key`, `Value`) VALUES(@Key, @Value)", new
                    {
                        Key = keyName,
                        Value = optionAttr?.DefaultValue ?? ReflectionUtils.GetDefaultTypeValue(property.PropertyType)
                    });
                }
            }
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public string? Read(string key)
    {
        var dbKey = $"{_prefix}.{key}";
        var option = _db.QueryAsync<DbConfigOptions>($"select * from `ConfigOptions` where `Key`=@Key", new {Key = dbKey})
            .GetAwaiter().GetResult().FirstOrDefault();

        if (option == null)
        {
            throw new KeyNotFoundException($"Database configuration option '{key}' for prefix '{_prefix}' not found");
        }

        return option.Value;
    }

    public void Write(string key, string? value)
    {
        var dbKey = $"{_prefix}.{key}";
        var option = _db.QueryAsync<DbConfigOptions>($"select * from `ConfigOptions` where `Key`=@Key", new {Key = dbKey})
            .GetAwaiter().GetResult().FirstOrDefault();

        if (option == null)
        {
            option = new DbConfigOptions {Key = $"{_prefix}.{key}", Value = value};
            
            _db.QueryAsync("insert into `ConfigOptions`(`Key`, `Value`) VALUES(@Key, @Value)", new
            {
                Key = option.Key,
                Value = option.Value
            }).GetAwaiter().GetResult();
        }
        else
        {
            option.Value = value;
            _db.UpdateAsync(option).GetAwaiter().GetResult();
        }
    }

    public bool CanRead => true;
    public bool CanWrite => true;
}
