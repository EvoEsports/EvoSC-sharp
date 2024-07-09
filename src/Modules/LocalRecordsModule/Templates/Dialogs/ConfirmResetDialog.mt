<!--
Shows a confirmation dialog when deleting a map from the leaderboard.
-->
<component>
  <import component="EvoSC.Controls.ConfirmDialog" as="ConfirmDialog" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <template>
    <UIStyle />
    
    <ConfirmDialog 
            title="Reset local records?"
            text="This will re-calculate local records on all maps based PBs."
            action="LocalRecordsManialinkController/ConfirmReset"
    />
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>