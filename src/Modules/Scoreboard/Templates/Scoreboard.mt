<component>
    <using namespace="System.Linq" />
    <import component="Scoreboard.Components.PlayerRow" as="PlayerRow" />
    
    <property type="int" name="MaxPlayers" default="0" />
    
    <template>
        <frame id="frame_rows" pos="-80 40">
            <PlayerRow foreach="int rowId in Enumerable.Range(0, MaxPlayers).ToList()" id="{{ rowId }}" y="{{ rowId * -5 }}" />
        </frame>
    </template>
    
    <script>
        <!--
        -->
    </script>
    
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
