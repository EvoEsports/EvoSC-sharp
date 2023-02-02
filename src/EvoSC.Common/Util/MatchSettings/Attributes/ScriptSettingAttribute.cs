namespace EvoSC.Common.Util.MatchSettings.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ScriptSettingAttribute : Attribute
{
    public string Name { get; set; }
}
