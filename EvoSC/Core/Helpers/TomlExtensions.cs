using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tomlet.Models;

namespace EvoSC.Core.Helpers;

public static class TomlExtensions
{
    public static T ValidateEntry<T>(this TomlDocument document, string key, Func<TomlValue, bool>? validator = null)
    {
        if (!document.Entries.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Property '{key}' doesn't exist in the document.");
        }
        
        if (validator != null && !validator(document.GetValue(key)))
        {
            throw new ValidationException($"The property '{key}' failed validation.");
        }

        return typeof(T) switch
        {
            Type type when type == typeof(int) => (T)(object)document.GetInteger(key),
            Type type when type == typeof(string) => (T)(object)document.GetString(key),
            Type type when type == typeof(string) => (T)(object)document.GetBoolean(key),
            Type type when type == typeof(string) => (T)(object)document.GetFloat(key),
            Type type when type == typeof(string) => (T)(object)document.GetLong(key),
            Type type when type == typeof(string) => (T)(object)document.GetValue(key),
            Type type when type == typeof(string) => (T)(object)document.GetArray(key),
            Type type when type == typeof(string) => (T)(object)document.GetSubTable(key)
        };
    }
}
