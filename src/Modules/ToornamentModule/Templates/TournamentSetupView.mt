<component>
  <using namespace="ToornamentApi.Models.Api.TournamentApi" />
  <using namespace="System.Linq" />

  <import component="EvoSC.Containers.Window" as="Window" />
  <import component="EvoSC.Containers.Container" as="Container" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  <import component="ToornamentModule.Parts.TournamentRow" as="TournamentRow" />
  <import component="ToornamentModule.Parts.StageRow" as="StageRow" />
  <import component="ToornamentModule.Parts.MatchRow" as="MatchRow" />

  <property type="List<TournamentBasicData>" name="tournaments" />
  <property type="List<StageInfo>" name="stages" />
  <property type="List<GroupInfo>" name="groups" />
  <property type="List<RoundInfo>" name="rounds" />
  <property type="List<MatchInfo>" name="matches" />

  <template>
    <UIStyle />

    <Window title="Tournament Setup" icon="{{ Icons.FlagCheckered }}" width="200" height="160" x="-100" y="80">
      <Container scrollable="true" scrollHeight="{{ tournaments.Count()*8 }}" width="62" height="160" id="tournaments-container">
        <TournamentRow foreach="TournamentBasicData tournament in tournaments" tournament="{{ tournament }}" y="{{ -__index*8 }}" width="60" index="{{ __index }}"/>
      </Container>
      <Container scrollable="true" scrollHeight="{{ stages.Count()*8 }}" width="62" height="160" id="stages-container" x ="62">
        <StageRow foreach="StageInfo stage in stages" stage="{{ stage }}" y="{{ -__index*8 }}" width="60" index="{{ __index }}"/>
      </Container>
      <Container scrollable="true" scrollHeight="{{ matches.Count()*8 }}" width="62" height="160" id="matches-container" x="124">
        <MatchRow foreach="MatchInfo match in matches" 
                  match="{{ match }}" 
                  group ="{{ groups.FirstOrDefault(g => g.Id == match.GroupId) }}"
                  stage ="{{ stages.FirstOrDefault(s => s.Id == match.StageId) }}"
                  round ="{{ rounds.FirstOrDefault(r => r.Id == match.RoundId) }}"
                  y="{{ -__index*8 }}" 
                  width="60" 
                  index="{{ __index }}" />
      </Container>
    </Window>
  </template>

  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>