<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  
  <import component="EvoSC.Controls.Button" as="Button" />
  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Drawing.Rectangle" as="Rectangle" />
  
  <property name="x" type="double" />
  <property name="y" type="double" />
  <property name="width" type="double" />
  <property name="matchSettings" type="IMatchSettings" />
  
  <template>
    <!-- <Panel x="{{ x }}" 
           y="{{ y }}" 
           width="{{ width }}" 
           height="14"
    >
      <label text="name: {{ matchSettings.Name }}" />
    </Panel> -->
    <frame pos="{{ x }} {{ y }}" size="{{ width }} 10">
      <Rectangle width="{{ width }}" 
                 height="10"
                 bgColor="{{ Theme.UI_MatchSettingsEditor_Overview_MatchSettingsRow_Bg }}"
                 cornerRadius="0.5"
      />
      <label text="{{ matchSettings.Name }}"
             class="text-lg text-primary"
             textcolor="{{ Theme.UI_MatchSettingsEditor_Overview_MatchSettingsRow_Name }}"
             pos="2 -2"
      />

      <label text='{{ Theme.UI_MatchSettingsEditor_GetCommonScriptName(matchSettings.GameInfos?.ScriptName) }}'
             class="text-xs"
             textcolor="{{ Theme.UI_MatchSettingsEditor_Overview_MatchSettingsRow_ScriptName }}"
             pos="2 -6"
      />
    </frame>
  </template>
</component>
