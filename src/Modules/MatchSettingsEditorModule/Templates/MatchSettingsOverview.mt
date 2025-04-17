<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  <import component="EvoSC.Controls.TextInput" as="TextInput" />
  <import component="EvoSC.Controls.ScrollBar" as="ScrollBar" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="MatchSettingsEditorModule.Overview.MatchSettingsRow" as="MatchSettingsRow" />
  
  <property type="IEnumerable<IMatchSettings>" name="matchSettingsList" />
  <property type="double" name="winH" default="67.5" />
  <property type="double" name="winW" default="100" />
  
  <template>
    <UIStyle />
    
    <Window title="Match Settings"
            icon="{{ Icons.Gears }}"
            width="{{ winW }}"
            height="{{ winH }}"
    >
      <TextInput id="txtSearch"
                 placeholder="Search"
                 width="{{ winW - 6 - 14 }}"
      />
      
      <IconButton id="btnNew" 
                  text="New"
                  hasText="true"
                  icon="{{ Icons.PlusSquare }}"
                  width="12"
                  x="{{ winW-6 - 12 }}"
      />
      
      <Container scrollable="true"
                 scrollHeight="{{ matchSettingsList.Count()*12 - (winH - 12.5 - 7) }}"
                 width="{{ winW - 9 }}"
                 height="{{ winH - 6 - 7 }}"
                 y="-7"
                 id="msListContainer"
      >
        <MatchSettingsRow foreach="IMatchSettings matchSettings in matchSettingsList"
                          matchSettings="{{ matchSettings }}"
                          x="0"
                          y="{{ -__index*(10 + 2) }}"
                          width="{{ winW - 9 }}"
        />
      </Container>

      <ScrollBar id="scrollMSList" 
                 forFrame="msListContainer"
                 y="-7"
                 x="{{ winW - 8 }}"
                 max="{{ matchSettingsList.Count()*12 - (winH - 12.5 - 7) }}"
                 length="{{ winH - 8.5 - 6 - 7 }}"
      />
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
