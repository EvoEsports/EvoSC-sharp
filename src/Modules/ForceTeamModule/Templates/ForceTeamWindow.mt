<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="EvoSC.Common.Util" />
  <using namespace="System.Linq" />
  
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="ForceTeamModule.Components.PlayerRow" as="PlayerRow" />
  
  <property type="IEnumerable<IOnlinePlayer>" name="players" />
  
  <template>
    <UIStyle />
    
    <Window icon="{{ Icons.Users }}" title="Teams" width="91" height="{{ 14 + Math.Max(players.Where(p => p.IsTeam1()).Count(), players.Where(p => p.IsTeam2()).Count())*5 }}">
      <Container width="91" height="49">
        <PlayerRow foreach="IOnlinePlayer player in players.Where(p => p.IsTeam1())" player="{{ player }}" x="0" y="{{ -__index*5 }}" width="40" />
        <PlayerRow foreach="IOnlinePlayer player in players.Where(p => p.IsTeam2())" player="{{ player }}" x="45" y="{{ -__index*5 }}" width="40" />
        <Separator direction="vertical" x="42" length="{{ Math.Max(players.Where(p => p.IsTeam1()).Count(), players.Where(p => p.IsTeam2()).Count())*5 }}" />
      </Container>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
