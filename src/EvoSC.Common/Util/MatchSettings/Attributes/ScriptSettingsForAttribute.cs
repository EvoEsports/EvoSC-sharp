using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Common.Util.MatchSettings.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ScriptSettingsForAttribute(string scriptName) : Attribute
{
    public string Name { get; set; } = scriptName;

    public ScriptSettingsForAttribute(DefaultModeScriptName mode) : this(mode.GetIdentifier())
    {
    }
}
