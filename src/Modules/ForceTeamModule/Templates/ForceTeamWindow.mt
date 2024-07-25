<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />
  <using namespace="EvoSC.Common.Util" />
  <using namespace="System.Linq" />
  <using namespace="GbxRemoteNet.Structs" />
  
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Controls.Separator" as="Separator" />
  <import component="ForceTeamModule.Components.PlayerRow" as="PlayerRow" />
  
  <property type="IEnumerable<IOnlinePlayer>" name="players" />
  <property type="TmTeamInfo" name="team1" />
  <property type="TmTeamInfo" name="team2" />
  
  <template>
    <UIStyle />
    
    <Window icon="{{ Icons.Users }}" title="Force Teams" width="91" height="{{ 14 + Math.Max(players.Where(p => p.IsTeam1()).Count(), players.Where(p => p.IsTeam2()).Count())*5 + 8 }}">
      <label text="$s{{ team1.Name }}" textcolor="{{ team1.RGB }}" halign="center" pos="20 0" class="text-xl text-primary" />
      <label text="$s{{ team2.Name }}" textcolor="{{ team2.RGB }}" halign="center" pos="65 0" class="text-xl text-primary" />
      
      <Container width="91" height="49" y="-8">
        <PlayerRow foreach="IOnlinePlayer player in players.Where(p => p.IsTeam1())" player="{{ player }}" x="0" y="{{ -__index*5 }}" width="40" />
        <PlayerRow foreach="IOnlinePlayer player in players.Where(p => p.IsTeam2())" player="{{ player }}" x="45" y="{{ -__index*5 }}" width="40" />
        <Separator direction="vertical" x="42" length="{{ Math.Max(players.Where(p => p.IsTeam1()).Count(), players.Where(p => p.IsTeam2()).Count())*5 }}" />
      </Container>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
