<component>
  <using namespace="EvoSC.Common.Interfaces.Models" />

  <import component="EvoSC.Controls.IconButton" as="IconButton" />
  
  <property type="IOnlinePlayer" name="player" />
  <property type="double" name="x" />
  <property type="double" name="y" />
  <property type="double" name="width" />
  
  <template>
    <frame pos="{{ x }} {{ y }}">
      <label if="player.IsTeam1()"
             class="text-primary" 
             text="{{ player.NickName }}"
             size="{{ width }} 5"
             halign="left" />
      <IconButton if="player.IsTeam1()"
                  id="btnMove-{{ player.AccountId }}" 
                  icon="{{ Icons.ArrowRight }}" 
                  style="round" x="{{ width-5 }}"
                  size="small"
                  action="ForceTeamManialinkController/SwitchPlayer/{{ player.AccountId }}" />
      
      <label if="player.IsTeam2()"
             class="text-primary" 
             text="{{ player.NickName }}"
             size="{{ width }} 5"
             pos="{{ width }} 0"
             halign="right" />
      <IconButton if="player.IsTeam2()"
                  id="btnMove-{{ player.AccountId }}" 
                  icon="{{ Icons.ArrowLeft }}" 
                  style="round"
                  size="small"
                  action="ForceTeamManialinkController/SwitchPlayer/{{ player.AccountId }}" />
    </frame>
  </template>
</component>
