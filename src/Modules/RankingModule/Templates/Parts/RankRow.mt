<!--
A single row in the ranking window.
-->
<component>
  <using namespace="EvoSC.Modules.Official.RankingModule.Models" />

  <import component="EvoSC.Controls.Panel" as="Panel" />

  <property type="PlayerRank" name="rank" />
  <property type="bool" name="highlight" default="false" />

  <property type="double" name="x" default="0.0" />
  <property type="double" name="y" default="0.0" />
  <property type="double" name="width" default="50" />
  <property type="double" name="height" default="6" />

  <template>
    <Panel x="{{ x }}" y="{{ y }}"
           width="{{ width }}"
           height="{{ height }}"
           bgColor="{{ highlight ? Theme.UI_AccentPrimary : Theme.UI_BgHighlight }}"
           overflow="true"
    >
      <label class="text-primary" text="{{ rank.Rank }}" pos="1 {{ -height/2.0+0.2 }}" valign="center" />
      <label class="text-primary" text="{{ rank.NickName }}" pos="10 {{ -height/2.0+0.2 }}" valign="center" size="44 4" />
      <label class="text-primary" text="{{ rank.ScoreText }}" pos="62 {{ -height/2.0+0.2 }}" halign="center" valign="center" />
      <label class="text-primary" text="{{ rank.PlaytimeText }}" pos="76 {{ -height/2.0+0.2 }}" halign="center" valign="center" />
      <label class="text-primary" text='{{ rank.MapsWon }}/{{ rank.MapsPlayed }} ({{ rank.WinRate.ToString("P0") }})' pos="94 {{ -height/2.0+0.2 }}" halign="center" valign="center" />
    </Panel>
  </template>
</component>
