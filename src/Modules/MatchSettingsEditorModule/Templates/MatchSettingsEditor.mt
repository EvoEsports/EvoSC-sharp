<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.TabGroup" as="TabGroup" />
  <import component="EvoSC.Containers.TabPage" as="TabPage" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="MatchSettingsEditorModule.Overview.MatchSettingsRow" as="MatchSettingsRow" />
  
  <property type="IMatchSettings" name="matchSettings" />
  <property type="double" name="winH" default="67.5" />
  <property type="double" name="winW" default="100" />
  
  <template>
    <UIStyle />
    
    <Window title="Match Settings {{ Icons.AngleRight }} {{ matchSettings.Name }}"
            icon="{{ Icons.Edit }}"
            width="{{ winW }}"
            height="{{ winH }}"
    >
      <TabGroup width="{{ winW - 6 }}" height="{{ winH - 8.5 }}">
        <TabPage id="tabGeneral" title="General">
          <label text="general" />
        </TabPage>
        
        <TabPage id="tabScriptSettings" title="Script Settings" headerWidth="25">
          <label text="script settings" />
        </TabPage>
        
        <TabPage id="tabMaps" title="Maps">
          <label text="maps" />
        </TabPage>
      </TabGroup>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
