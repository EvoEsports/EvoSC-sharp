<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="EvoSC.Common.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="ForceTeamModule.Components.PlayerRow" as="PlayerRow" />
  
  <property type="IEnumerable<IOnlinePlayer>" name="players" />
  
  <template>
    <UIStyle />
    
    <Window icon="{{ Icons.User }}" title="Teams">
      <PlayerRow foreach="IOnlinePlayer player in players.Where(p => p.IsTeam1())" player="{{ player }}" x="0" y="{{ -__index*3 }}" />
      <PlayerRow foreach="IOnlinePlayer player in players.Where(p => p.IsTeam2())" player="{{ player }}" x="40" y="{{ -__index*3 }}" />
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
