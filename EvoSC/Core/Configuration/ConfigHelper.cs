using System.Reflection;

namespace EvoSC.Core.Configuration;

public static class ConfigHelper
{
    public static bool IsAnyNullOrEmpty(object obj)
    {
        foreach (PropertyInfo pi in obj.GetType().GetProperties())
        {
            if (pi.PropertyType == typeof(string))
            {
                var value = (string)pi.GetValue(obj);
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
