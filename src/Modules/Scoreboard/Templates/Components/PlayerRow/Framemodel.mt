<component>
    <import component="Scoreboard.Components.PlayerRow.CustomLabelBackground" as="CustomLabelBackground"/>
    <import component="Scoreboard.Components.PlayerRow.PlayerRowBackground" as="PlayerRowBackground"/>
    <import component="Scoreboard.Components.PlayerRow.PointsBox" as="PointsBackground"/>
    <import component="Scoreboard.Components.PlayerRow.PositionBox" as="Position"/>
    
    <property type="string" name="headerColor" />
    <property type="string" name="primaryColor" />
    <property type="double" name="w" />
    <property type="double" name="padding" />
    <property type="double" name="rowHeight" />
    <property type="double" name="rowSpacing" />
    <property type="double" name="innerSpacing" />
    <property type="double" name="rowInnerHeight" />
    <property type="double" name="pointsWidth" />
    
    <template>
        <framemodel id="player_row">
            <!-- Scroll activation -->
            <quad size="{{ w }} {{ rowHeight + rowSpacing }}" ScriptEvents="1"/>

            <!-- Player Row Background -->
            <PlayerRowBackground rowHeight="{{ rowHeight }}"
                                 headerColor="{{ headerColor }}"
                                 padding="{{ padding }}"
                                 w="{{ w }}"
            />

            <!-- Custom Label Background -->
            <CustomLabelBackground id="custom_gradient"
                                   x="{{ w - padding }}"
                                   rowHeight="{{ rowHeight }}"
                                   primaryColor="{{ primaryColor }}"
                                   w="{{ w }}"
            />

            <!-- Position Box -->
            <Position id="position_box"
                      x="{{ padding }}"
                      z-index="5"
                      rowHeight="{{ rowHeight }}"
                      primaryColor="{{ primaryColor }}"
            />

            <frame pos="{{ padding + (rowHeight * 1.2) + innerSpacing * 2.0 }} {{ rowHeight / -2.0 }}" z-index="10">
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
                      modulatecolor="{{ headerColor }}"
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
            <frame z-index="10">
                <!-- Spec/Disconnected -->
                <label id="spec_disconnected_label" pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="2.6"
                       textcolor="fff"
                       opacity="0.5"
                       textfont="GameFontSemiBold"/>

                <!-- Round Points -->
                <label id="round_points" pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="2.6"
                       textcolor="{{ primaryColor }}"
                       textfont="GameFontBlack"/>

                <!-- Custom Label (FINALIST, etc) -->
                <label id="custom_label"
                       pos="0 {{ rowHeight / -2.0 + 0.3 }}"
                       valign="center"
                       halign="right"
                       textsize="2.4"
                       textfont="GameFontRegular"/>

                <!-- Player Score -->
                <label id="score_two" pos="0 {{ rowHeight / -2.0 + 0.4 }}"
                       valign="center"
                       halign="right"
                       textsize="2.4"
                       textfont="GameFontRegular"
                       opacity="0.1"/>
                <label id="score"
                       pos="0 {{ rowHeight / -2.0 + 0.4 }}"
                       valign="center"
                       halign="right"
                       textsize="2.4"
                       textfont="GameFontRegular"/>

                <!-- Points Box -->
                <PointsBackground id="points_box"
                                  x="{{ w - padding - pointsWidth * 0.65 - rowHeight * 0.4 * 0.5 -0.05 }}"
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
                <label id="points"
                       pos="{{ w - padding - (pointsWidth * 0.75) / 2.0 - rowHeight * 0.25 * 0.5 }} {{ rowHeight / -2.0 + 0.4 }}"
                       text="x"
                       valign="center"
                       halign="center"
                       textsize="2"
                       textcolor="333"
                       textfont="GameFontSemiBold"
                       z-index="11"
                />
            </frame>
        </framemodel>
    </template>
</component>
