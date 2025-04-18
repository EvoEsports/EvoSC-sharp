<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.TabGroup" as="TabGroup" />
  <import component="EvoSC.Containers.TabPage" as="TabPage" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <import component="MatchSettingsEditorModule.Editor.TabGeneral" as="TabGeneral" />
  <import component="MatchSettingsEditorModule.Editor.TabScriptSettings" as="TabScriptSettings" />
  <import component="MatchSettingsEditorModule.Editor.TabMaps" as="TabMaps" />
  
  <property type="IMatchSettings" name="matchSettings" />
  <property type="double" name="winH" default="80.5" />
  <property type="double" name="winW" default="120" />
  
  <template>
    <UIStyle />
    
    <Window title="Match Settings {{ Icons.AngleRight }} {{ matchSettings.Name }}"
            icon="{{ Icons.Edit }}"
            width="{{ winW }}"
            height="{{ winH }}"
    >
      <TabGroup width="{{ winW - 6 }}" height="{{ winH - 14.5 - 8 }}" selectedTab="tabGeneral">
        <TabPage id="tabGeneral" title="General">
          <TabGeneral matchSettings="{{ matchSettings }}" width="{{ winW-6 }}" />
        </TabPage>
        <TabPage id="tabScriptSettings" title="Script Settings" headerWidth="25">
          <TabScriptSettings matchSettings="{{ matchSettings }}" width="{{ winW-6 }}" height="{{ winH - 14.5 - 8 }}" />
        </TabPage>
        <TabPage id="tabMaps" title="Maps">
          <TabMaps matchSettings="{{ matchSettings }}" />
        </TabPage>
      </TabGroup>
      
      <Separator length="{{ winW - 6 }}" y="{{ -winH+14.5 + 7 }}" />
      
      <IconButton id="btnBack" y="{{ -winH+14.5 + 5 }}" hasText="true" icon="{{ Icons.ArrowLeft }}" text="Back" type="secondary" />
      <IconButton id="btnSave" x="{{ winW - 23 }}" y="{{ -winH+14.5 + 5 }}" hasText="true" icon="{{ Icons.Check }}" text="Save" />
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
