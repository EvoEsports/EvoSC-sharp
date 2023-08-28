<component>
    <import component="Scoreboard.Components.PlayerRow.CustomLabelBackground" as="CustomLabelBackground"/>
    <import component="Scoreboard.Components.PlayerRow.PlayerRowBackground" as="PlayerRowBackground"/>
    <import component="Scoreboard.Components.PlayerRow.PointsBox" as="PointsBox"/>
    <import component="Scoreboard.Components.PlayerRow.PositionBox" as="Position"/>
    <import component="Scoreboard.Components.PlayerRow.PlayerActions" as="PlayerActions"/>

    <property type="string" name="positionBackgroundColor"/>
    <property type="string" name="backgroundColor"/>
    <property type="string" name="primaryColor"/>
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

            <!-- Position Box -->
            <Position id="position_box"
                      w="{{ positionBoxWidth }}"
                      h="{{ rowHeight }}"
                      z-index="5"
                      positionBackgroundColor="{{ positionBackgroundColor }}"
            />

            <!-- Player Row Background -->
            <PlayerRowBackground id="player_row_bg"
                                 rowHeight="{{ rowHeight }}"
                                 backgroundColor="{{ backgroundColor }}"
                                 padding="{{ padding }}"
                                 w="{{ w - scrollBarWidth - rowSpacing - positionBoxWidth - rowSpacing }}"
                                 x="{{ positionBoxWidth + rowSpacing }}"
            />

            <!-- Custom Label Background -->
            <CustomLabelBackground id="custom_gradient"
                                   x="{{ w - padding }}"
                                   rowHeight="{{ rowHeight }}"
                                   primaryColor="{{ primaryColor }}"
                                   w="{{ w }}"
            />

            <frame pos="{{ innerSpacing + positionBoxWidth + innerSpacing * 1.5 }} {{ rowHeight / -2.0 }}" z-index="10">
                <!-- Flag -->
                <quad id="flag"
                      size="{{ rowInnerHeight * 2 }} {{ rowInnerHeight }}"
                      valign="center"
                      image="file://Media/Manialinks/Nadeo/TMNext/Menus/Common/Common_Flag_Mask.dds"
                      alphamask="file://Media/Manialinks/Nadeo/TMNext/Menus/Common/Common_Flag_Mask.dds"
                />

                <!-- Club Tag Background -->
                <quad id="club_bg"
                      size="{{ rowInnerHeight * 2 }} {{ rowInnerHeight }}"
                      pos="{{ rowInnerHeight * 2 }}"
                      valign="center"
                      modulatecolor="000"
                      image="file://Media/Manialinks/Nadeo/TMNext/Menus/Common/Common_Flag_Mask.dds"
                      alphamask="file://Media/Manialinks/Nadeo/TMNext/Menus/Common/Common_Flag_Mask.dds"
                />

                <!-- Club Tag Text -->
                <label id="club"
                       pos="{{ rowInnerHeight * 3 }} 0.2"
                       size="5 3"
                       valign="center"
                       halign="center"
                       textsize="0.9"
                       textfont="GameFontSemiBold"
                />

                <!-- Player Name -->
                <label id="name"
                       pos="{{ rowInnerHeight * 4 + innerSpacing }} 0.4"
                       valign="center"
                       textsize="2.6"
                       textfont="GameFontSemiBold"/>
            </frame>
            <frame id="details_wrapper" z-index="10">
                <!-- Spec/Disconnected -->
                <label id="spec_disconnected_label" pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textcolor="fff"
                       opacity="0.5"
                       textfont="GameFontSemiBold"/>

                <!-- Round Points -->
                <label id="round_points" pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textcolor="{{ primaryColor }}"
                       textfont="GameFontSemiBold"/>

                <!-- Custom Label (FINALIST, etc) -->
                <label id="custom_label"
                       pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textfont="GameFontRegular"/>

                <!-- Player Score -->
                <label id="score"
                       pos="0 {{ rowHeight / -2.0 + 0.4 }}"
                       valign="center"
                       halign="right"
                       textsize="{{ textSize }}"
                       textfont="GameFontRegular"/>

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
                           highlightColor="{{ primaryColor }}"
                           textsize="{{ textSize }}"
            />
        </framemodel>
    </template>

    <script once="true">
        <!--
        Void TogglePlayerActions(CMlFrame playerRow) {
            declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
            declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
            declare Boolean RowIsLocked for playerRow = False;
            
            if(!RowIsLocked){
                playerActions.Show();
                detailsWrapper.Hide();
                RowIsLocked = True;
            }else{
                playerActions.Hide();
                detailsWrapper.Show();
                RowIsLocked = False;
            }
        }
        
        *** OnMouseClick ***
        ***
            if(Event.Control.ControlId == "player_row_trigger"){
                TogglePlayerActions(Event.Control.Parent as CMlFrame);
            }
        ***
        -->
    </script>
</component>
