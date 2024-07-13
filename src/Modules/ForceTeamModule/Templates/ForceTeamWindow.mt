<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="EvoSC.Containers.Window" as="Window" />
  
  <property type="IEnumerable<IOnlinePlayer>" name="players" />
  
  <template>
    <UIStyle />
    
    <Window icon="{{ Icons.User }}" title="Teams">
      <label foreach="IOnlinePlayer player in players.Where(p => p.Team == PlayerTeam.Team1)" text="{{ player.NickName }}" pos="0 {{ -__index*3 }}" class="text-primary" />
      <label foreach="IOnlinePlayer player in players.Where(p => p.Team == PlayerTeam.Team2)" text="{{ player.NickName }}" pos="40 {{ -__index*3 }}" class="text-primary" />
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
