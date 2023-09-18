<component>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="rowHeight" default="0.0"/>
    <property type="double" name="textsize" default="1.0"/>
    <property type="double" name="innerSpacing" default="0.0"/>
    <property type="string" name="highlightColor" default="fff"/>

    <template>
        <frame id="player_actions" pos="{{ x }} {{ y }}" hidden="1">
            <!-- SPECTATE -->
            <label id="spectate_player"
                   pos="{{ rowHeight * -0.5 - innerSpacing }}"
                   size="{{ rowHeight }} {{ rowHeight }}"
                   valign="center"
                   halign="center"
                   text=""
                   textsize="{{ textsize }}"
                   ScriptEvents="1"
                   focusareacolor1="0000"
                   focusareacolor2="{{ highlightColor }}"
            />

            <!-- PROFILE -->
            <label id="show_player_profile"
                   pos="{{ rowHeight * -1.5 - innerSpacing }}"
                   size="{{ rowHeight }} {{ rowHeight }}"
                   valign="center"
                   halign="center"
                   text=""
                   textsize="{{ textsize }}"
                   ScriptEvents="1"
                   focusareacolor1="0000"
                   focusareacolor2="{{ highlightColor }}"
            />
        </frame>
    </template>

    <script>
        <!--
        *** OnMouseClick *** 
        ***
            if (Event.Control.ControlId == "spectate_player") {
                declare playerRow = (Event.Control.Parent.Parent as CMlFrame);
                declare CSmScore playerScore for playerRow;
                if(!IsSpectatorClient) RequestSpectatorClient(True);
                SetSpectateTarget(playerScore.User.Login);
                TogglePlayerActions(playerRow);
            }else if (Event.Control.ControlId == "show_player_profile") {
                declare playerRow = (Event.Control.Parent.Parent as CMlFrame);
                declare CSmScore playerScore for playerRow;
                declare Text LibTMxSMRaceScoresTable_OpenProfileUserId for ClientUI = "";
                LibTMxSMRaceScoresTable_OpenProfileUserId = playerScore.User.WebServicesUserId;
                TogglePlayerActions(playerRow);
            }
        ***
        -->
    </script>
</component>