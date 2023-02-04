using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ScriptSettingsForAttribute : Attribute
{
    public string Name { get; set; }

    public ScriptSettingsForAttribute(string scriptName)
    {
        Name = scriptName;
    }

    public ScriptSettingsForAttribute(DefaultModeScriptName mode)
    {
        Name = mode.GetIdentifier();
    }
}
