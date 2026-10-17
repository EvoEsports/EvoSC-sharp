<!--
Shows a window with the top ranked players. The player who opened it is highlighted,
or shown pinned below the list with their rank if they are not in the top.
-->
<component>
  <using namespace="EvoSC.Modules.Official.RankingModule.Models" />
  <using namespace="System.Linq" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="RankingModule.Parts.RankRow" as="RankRow" />

  <property type="IEnumerable<PlayerRank>" name="ranking" />
  <property type="PlayerRank?" name="footer" />
  <property type="string" name="selfAccountId" />

  <template>
    <UIStyle />

    <Window title="Ranking"
            icon="{{ Icons.Trophy }}"
            width="110"
            height="85"
    >
      <label class="text-muted" text="#" pos="1 -3" valign="center" />
      <label class="text-muted" text="Player" pos="10 -3" valign="center" />
      <label class="text-muted" text="Score" pos="62 -3" halign="center" valign="center" />
      <label class="text-muted" text="Playtime" pos="76 -3" halign="center" valign="center" />
      <label class="text-muted" text="Wins" pos="94 -3" halign="center" valign="center" />

      <Container scrollable="true"
                 scrollHeight="{{ ranking.Count()*7 }}"
                 y="-6"
                 width="106"
                 height="56"
                 id="ranking-container"
      >
        <RankRow
                foreach="PlayerRank rank in ranking"
                rank="{{ rank }}"
                y="{{ -__index*7 }}"
                width="102"
                highlight="{{ rank.AccountId == selfAccountId }}"
        />
      </Container>

      <RankRow
              if="footer != null"
              rank="{{ footer }}"
              y="-64"
              width="102"
              highlight="true"
      />
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
