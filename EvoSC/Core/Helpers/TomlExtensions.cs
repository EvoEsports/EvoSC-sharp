using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tomlet;
using Tomlet.Models;

namespace EvoSC.Core.Helpers;

public static class TomlExtensions
{
    /// <summary>
    /// Validate an entry in a Toml document.
    /// </summary>
    /// <param name="document"></param>
    /// <param name="key"></param>
    /// <param name="validator"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="ValidationException"></exception>
    public static T ValidateEntry<T>(this TomlDocument document, string key, Func<TomlValue, bool>? validator = null)
    {
        if (!document.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Property '{key}' doesn't exist in the document.");
        }
        
        if (validator != null && !validator(document.GetValue(key)))
        {
            throw new ValidationException($"The property '{key}' failed validation.");
        }

        return TomletMain.To<T>(document.GetValue(key));
    }
}
