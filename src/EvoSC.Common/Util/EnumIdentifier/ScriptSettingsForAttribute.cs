using EvoSC.Common.Util.MatchSettings;

namespace EvoSC.Common.Util.EnumIdentifier;

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
