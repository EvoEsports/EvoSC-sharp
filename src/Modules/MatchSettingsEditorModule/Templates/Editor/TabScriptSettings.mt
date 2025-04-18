<component>
  <using namespace="System.Linq" />
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="EvoSC.Common.Util.MatchSettings.Models" />
  
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.ScrollBar" as="ScrollBar" />
  
  <import component="MatchSettingsEditorModule.Editor.ScriptSettingRow" as="ScriptSettingRow" />

  <property type="IMatchSettings" name="matchSettings" />
  <property type="double" name="width" />
  <property type="double" name="height" />
  
  <template>
    <Container width="{{ width - 3 }}" 
               height="{{ height - 1 }}" 
               scrollable="true"
               scrollHeight="{{ Math.Max(0, (matchSettings.ModeScriptSettings.Count() + 1)*7 - (height + 1)) }}"
               id="scriptSettingsListContainer"
               y="-1"
    >
      <ScriptSettingRow foreach="KeyValuePair<string, ModeScriptSettingInfo> kv in matchSettings.ModeScriptSettings"
                        y="{{ -__index*7 }}"
                        name="{{ kv.Key }}"
                        type="{{ kv.Value.Type }}"
                        value='{{ kv.Value.Value?.ToString() ?? "" }}'
                        width="{{ width - 3 }}"
      />
    </Container>

    <ScrollBar id="scrollScriptSettingsList"
               forFrame="scriptSettingsListContainer"
               y="-1"
               x="{{ width - 2 }}"
               max="{{ Math.Max(0, (matchSettings.ModeScriptSettings.Count() + 1)*7 - (height + 1)) }}"
               length="{{ height - 1 - 7 }}"
               if="Math.Max(0, (matchSettings.ModeScriptSettings.Count() + 1)*7 - (height + 1)) > 0"
    />
  </template>
</component>
