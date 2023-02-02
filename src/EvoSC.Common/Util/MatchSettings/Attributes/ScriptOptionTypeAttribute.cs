namespace EvoSC.Common.Util.MatchSettings.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class ScriptOptionTypeAttribute<T> : Attribute
{
    /// <summary>
    /// The type of this script option.
    /// </summary>
    public Type Type = typeof(T);
}
