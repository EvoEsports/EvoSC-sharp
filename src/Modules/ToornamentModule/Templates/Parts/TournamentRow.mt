<!--
Show a row with map information along with action buttons.
-->
<component>
  <using namespace="ToornamentApi.Models.Api.TournamentApi" />

  <import component="EvoSC.Controls.Panel" as="Panel" />
  <import component="EvoSC.Controls.IconButton" as="IconButton" />

  <property type="TournamentBasicData" name="tournament" />

  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="50" />
  <property type="double" name="height" default="7" />
  <property type="int" name="index" default="0" />

  <template>
    <Panel x="{{ x }}" y="{{ y }}" width="{{ width }}" height="{{ height }}" bgColor="{{ Theme.UI_BgHighlight }}" overflow="true">
      <label class="text-primary" text="{{ tournament.Name }}" valign="center" pos="1 {{ -height/2.0+0.2 }}" />
      <IconButton
              icon="{{ Icons.PlayCircle }}"
              y="{{ -(height-5)/2 }}"
              x="{{ width-6 }}"
              id="btnSelectTournament{{ index }}"
              type="secondary"
              action="MatchManialinkController/SelectTournament/{{ tournament.Id }}"
              if='tournament.Status != "completed"'
      />
    </Panel>
  </template>  
</component>