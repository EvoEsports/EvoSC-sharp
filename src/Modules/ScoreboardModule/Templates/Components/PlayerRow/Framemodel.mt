<component>
    <import component="ScoreboardModule.Components.PlayerRow.CustomLabelBackground" as="CustomLabelBackground"/>
    <import component="ScoreboardModule.Components.PlayerRow.PlayerRowBackground" as="PlayerRowBackground"/>
    <import component="ScoreboardModule.Components.PlayerRow.PointsBox" as="PointsBox"/>
    <import component="ScoreboardModule.Components.PlayerRow.PositionBox" as="Position"/>
    <import component="ScoreboardModule.Components.PlayerRow.PlayerActions" as="PlayerActions"/>
    <import component="EvoSC.Advanced.ClubTag" as="ClubTag"/>

    <property type="double" name="w"/>
    <property type="double" name="padding"/>
    <property type="double" name="rowHeight"/>
    <property type="double" name="rowSpacing"/>
    <property type="double" name="innerSpacing"/>
    <property type="double" name="rowInnerHeight"/>
    <property type="double" name="pointsWidth"/>
    <property type="double" name="scrollBarWidth"/>

    <property type="double" name="textSize" default="2.4"/>
    <property type="double" name="positionBoxWidth" default="9.6"/>

    <template>
        <framemodel id="player_row">
            <!-- Scroll activation -->
            <quad id="player_row_trigger" size="{{ w }} {{ rowHeight + rowSpacing }}" ScriptEvents="1"/>

            <!-- Player Row Background -->
            <PlayerRowBackground id="player_row_bg"
                                 rowHeight="{{ rowHeight }}"
                                 padding="{{ padding }}"
                                 w="{{ w - scrollBarWidth - rowSpacing - positionBoxWidth - rowSpacing }}"
                                 x="{{ positionBoxWidth + rowSpacing }}"
            />

            <!-- Position Box -->
            <Position id="position_box"
                      w="{{ positionBoxWidth }}"
                      h="{{ rowHeight }}"
                      z-index="5"
            />

            <!-- Custom Label Background -->
            <CustomLabelBackground id="custom_gradient"
                                   x="{{ w - padding }}"
                                   rowHeight="{{ rowHeight }}"
                                   w="{{ w }}"
            />

            <frame pos="{{ innerSpacing + positionBoxWidth + innerSpacing * 1.5 }} {{ rowHeight / -2.0 }}" z-index="10">
                <!-- Flag -->
                <quad id="flag"
                      size="{{ rowInnerHeight * 2 }} {{ rowInnerHeight }}"
                      valign="center"
                      image="file://Media/Manialinks/Nadeo/Trackmania/Menus/PageClub/ClubActivities/Clubs_ActivityIcon_Mask.dds"
                      alphamask="file://Media/Manialinks/Nadeo/Trackmania/Menus/PageClub/ClubActivities/Clubs_ActivityIcon_Mask.dds"
                />

               <ClubTag h="{{ rowInnerHeight }}" />

                <!-- Player Name -->
                <label id="name"
                       pos="{{ rowInnerHeight * 4 + innerSpacing }} 0.4"
                       size="{{ w / 3.0 }} {{ rowHeight }}"
                       valign="center"
                       textsize="2.6"
                       textfont="{{ Font.Regular }}"
                       textcolor="{{ Theme.ScoreboardModule_PlayerRow_Text }}"/>
            </frame>
            <frame id="details_wrapper" z-index="10">
                <!-- Spec/Disconnected -->
                <label id="spec_disconnected_label" pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textcolor="{{ Theme.ScoreboardModule_PlayerRow_FrameModel_Text }}"
                       opacity="0.5"
                       textfont="{{ Font.Regular }}"/>

                <!-- Round Points -->
                <label id="round_points" pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textcolor="{{ Theme.ScoreboardModule_PlayerRow_FrameModel_TextRoundPoints }}"
                       textfont="{{ Font.Regular }}"/>

                <!-- Custom Label (FINALIST, etc) -->
                <label id="custom_label"
                       pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textfont="{{ Font.Thin }}"
                       textcolor="{{ Theme.ScoreboardModule_PlayerRow_Text }}"/>

                <!-- Player Score -->
                <label id="score"
                       pos="0 {{ rowHeight / -2.0 + 0.4 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textfont="{{ Font.Thin }}"
                       textcolor="{{ Theme.ScoreboardModule_PlayerRow_Text }}"/>

                <!-- Points Box -->
                <PointsBox id="points_box"
                           x="{{ w - padding - pointsWidth * 0.65 - rowHeight * 0.4 * 0.5 }}"
                           y="{{ rowHeight * 0.33 * -0.5 }}"
                           w="{{ pointsWidth }}"
                           h="{{ rowHeight }}"
                           scale="0.65"
                           hidden="1"
                           z-index="10"
                           rowHeight="{{ rowHeight }}"
                           pointsWidth="{{ pointsWidth }}"
                           padding="{{ padding }}"
                />
            </frame>
            <PlayerActions x="{{ w - padding }}" 
                           y="{{ rowHeight / -2.0 }}" 
                           rowHeight="{{ rowHeight }}"
                           innerSpacing="{{ innerSpacing }}"
                           textsize="{{ textSize }}"
            />
        </framemodel>
    </template>

    <script once="true">
        <!--
        Void ShowPlayerActions(CMlFrame playerRow) {
            declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
            declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
            playerActions.Show();
            detailsWrapper.Hide();
        }
        
        Void HidePlayerActions(CMlFrame playerRow) {
            declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
            declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
            playerActions.Hide();
            detailsWrapper.Show();
        }
        
        Void ResetPlayerActions() {
            declare playerRows <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            foreach(playerRowControl in playerRows.Controls){
                declare playerRow = (playerRowControl as CMlFrame);
                HidePlayerActions(playerRow);
                declare Boolean RowIsLocked for playerRow = False;
                RowIsLocked = False;
            }
        }
        
        Void TogglePlayerActions(CMlFrame playerRow) {
            declare Boolean RowIsLocked for playerRow = False;
            
            if(RowIsLocked){
                RowIsLocked = False;
                HidePlayerActions(playerRow);
            }else{
                ResetPlayerActions();
                RowIsLocked = True;
                ShowPlayerActions(playerRow);
            }
        }
        
        *** OnMouseClick ***
        ***
            if(Event.Control.ControlId == "player_row_trigger"){
                TogglePlayerActions(Event.Control.Parent as CMlFrame);
                continue;
            }
        ***
        
        *** OnMouseOver ***
        ***
            if(Event.Control.ControlId == "player_row_trigger"){
                declare parentFrame = (Event.Control.Parent as CMlFrame);
                declare backgroundFrame <=> (parentFrame.GetFirstChild("player_row_bg") as CMlFrame);
                SetPlayerBackgroundColor(backgroundFrame, CL::HexToRgb("{{ Theme.ScoreboardModule_PlayerRow_FrameModel_BgRow }}"));
                continue;
            }
        ***
        
        *** OnMouseOut ***
        ***
            if(Event.Control.ControlId == "player_row_trigger"){
                declare parentFrame = (Event.Control.Parent as CMlFrame);
                declare backgroundFrame <=> (parentFrame.GetFirstChild("player_row_bg") as CMlFrame);
                continue;
                ResetPlayerBackgroundColor(backgroundFrame);
            }
        ***
        -->
    </script>
</component>
