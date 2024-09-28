﻿<component>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="rowHeight" default="0.0"/>
    <property type="double" name="rowSpacing" default="0.5"/>

    <template>
        <frame id="player_actions" pos="{{ x - (rowHeight*1.2)/2.0 }} {{ y }}" hidden="1">
            <!-- SPECTATE -->
          <label id="spectate_player"
                 class="text-primary"
                 size="{{ rowHeight*1.2 }} {{ rowHeight }}"
                 valign="center"
                 halign="center"
                 text="{{ Icons.Eye }}"
                 textsize="{{ Theme.UI_FontSize*2 }}"
                 ScriptEvents="1"
                 focusareacolor1="{{ Theme.UI_BgPrimary }}"
                 focusareacolor2="{{ Theme.UI_AccentPrimary }}"
                 textcolor="{{ Theme.ScoreboardModule_PlayerRow_Text }}"/>

          <!-- PROFILE -->
          <label id="show_player_profile"
                 class="text-primary"
                 pos="{{ rowHeight*-1.2 - rowSpacing }}"
                 size="{{ rowHeight*1.2 }} {{ rowHeight }}"
                 valign="center"
                 halign="center"
                 text="{{ Icons.VcardO }}"
                 textsize="{{ Theme.UI_FontSize*2 }}"
                 ScriptEvents="1"
                 focusareacolor1="{{ Theme.UI_BgPrimary }}"
                 focusareacolor2="{{ Theme.UI_AccentPrimary }}"
                 textcolor="{{ Theme.ScoreboardModule_PlayerRow_Text }}"/>
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
                declare Text TMGame_ScoresTable_OpenProfileUserId for ClientUI = "";
                TMGame_ScoresTable_OpenProfileUserId = playerScore.User.WebServicesUserId;
                TogglePlayerActions(playerRow);
            }
        ***
        -->
    </script>
</component>