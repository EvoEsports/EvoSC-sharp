<component>
  <using namespace="EvoSC.Common.Interfaces.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Window" as="Container" />
  <import component="MatchSettingsEditorModule.Overview.MatchSettingsRow" as="MatchSettingsRow" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <property type="IEnumerable<IMatchSettings>" name="matchSettingsList" />
  
  <template>
    <UIStyle />
    
    <Window title="Match Settings"
            icon="{{ Icons.Gears }}"
            width="100"
            height="60"
    >
      <Container id="matchSettingsListContainer"
                 scrollable="true"
                 scrollHeight="{{ 54 - matchSettingsList.Count()*12 }}"
                 width="{{ 100-6 }}"
                 height="60 - 6"
      >
        <MatchSettingsRow foreach="IMatchSettings matchSettings in matchSettingsList"
                          matchSettings="{{ matchSettings }}"
                          x="0"
                          y="{{ -__index*(10 + 2) }}"
                          width="{{ 100 - 6 }}"
        />
      </Container>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
