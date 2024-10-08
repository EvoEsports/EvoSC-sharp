<!--
Shows a confirmation dialog when deleting a map from the leaderboard.
-->
<component>
  <import component="EvoSC.Controls.ConfirmDialog" as="ConfirmDialog" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <property type="string" name="tournamentId" />
  <property type="string" name="stageId" />
  <property type="string" name="matchId" />
  
  <template>
    <UIStyle />
    
    <ConfirmDialog 
            title="Override in-progress match"
            text="This match is already in progress. Are you sure you want to setup the server again?"
            action="MatchManialinkController/ConfirmMatch/{{ tournamentId }}/{{ stageId }}/{{ matchId }}"
    />
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>
