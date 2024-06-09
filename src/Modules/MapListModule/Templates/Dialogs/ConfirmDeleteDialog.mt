<!--
Shows a confirmation dialog when deleting a map from the leaderboard.
-->
<component>
  <import component="EvoSC.Controls.ConfirmDialog" as="ConfirmDialog" />
  <import component="EvoSC.Style.UIStyle" as="UIStyle" />
  
  <property type="string" name="mapName" />
  <property type="string" name="mapUid" />
  
  <template>
    <UIStyle />
    
    <ConfirmDialog 
            title="You are about to delete a map"
            text="Delete the map '{{ mapName }}'?"
            action="MapListManialinkController/ConfirmDelete/{{ mapUid }}"
    />
  </template>
  
  <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>