using System.Data.Common;
using Config.Net;
using Dapper;
using Dapper.Contrib.Extensions;
using EvoSC.Common.Database.Models;

namespace EvoSC.Common.Config.Stores;

public class DatabaseStore : IConfigStore
{
    private readonly DbConnection _db;
    private readonly string _prefix;
    
    public DatabaseStore(string prefix, DbConnection db)
    {
        _prefix = prefix;
        _db = db;
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public string? Read(string key)
    {
        var dbKey = $"{_prefix}.{key}";
        var option = _db.QueryAsync<DbConfigOption>($"select * from `configoptions` where `Key`=@Key", new {Key = dbKey})
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
        var option = _db.QueryAsync<DbConfigOption>($"select * from `configoptions` where `Key`=@Key", new {Key = dbKey})
            .GetAwaiter().GetResult().FirstOrDefault();

        if (option == null)
        {
            option = new DbConfigOption {Key = $"{_prefix}.{key}", Value = value};
            // _db.InsertAsync(option).GetAwaiter().GetResult();
            _db.QueryAsync("insert into `configoptions`(`Key`, `Value`) VALUES(@Key, @Value)", new
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
