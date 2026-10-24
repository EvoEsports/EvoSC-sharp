<component>
    <using namespace="EvoSC.Modules.Official.ScoreboardModule.Config"/>
    
    <import component="ScoreboardModule.Components.Row.CustomLabelBackground" as="CustomLabelBackground"/>
    <import component="ScoreboardModule.Components.Row.PlayerRowBackground" as="PlayerRowBackground"/>
    <import component="ScoreboardModule.Components.Row.PlayerActions" as="PlayerActions"/>
    <import component="ScoreboardModule.Components.Row.PositionBox" as="PositionBox"/>
    <import component="ScoreboardModule.Components.Row.Flag" as="Flag"/>

    <property type="IScoreboardSettings" name="settings"/>
    <property type="double" name="w"/>
    <property type="double" name="padding"/>
    <property type="double" name="rowHeight"/>
    <property type="double" name="rowSpacing"/>
    <property type="double" name="columnSpacing"/>
    <property type="double" name="innerSpacing"/>
    <property type="double" name="rowInnerHeight"/>
    <property type="double" name="pointsWidth"/>
    <property type="int" name="actionButtonCount"/>

    <property type="double" name="textSize" default="2.4"/>
    <property type="double" name="positionBoxWidth" default="9.6"/>

    <template>
        <framemodel id="player_row">
            <!-- Scroll activation -->
            <quad id="player_row_trigger"
                  size="{{ w }} {{ rowHeight + rowSpacing }}"
                  ScriptEvents="1"
            />

            <!-- Player Row Background -->
            <PlayerRowBackground id="player_row_bg"
                                 rowHeight="{{ rowHeight }}"
                                 padding="{{ padding }}"
                                 w="{{ w - positionBoxWidth }}"
                                 h="{{ rowHeight }}"
                                 x="{{ positionBoxWidth }}"
            />

            <!-- Position Box -->
            <PositionBox width="{{ positionBoxWidth }}"
                         height="{{ rowHeight }}"
                         settings="{{ settings }}"
            />

            <!-- Custom Label Background -->
            <CustomLabelBackground id="custom_gradient"
                                   x="{{ w }}"
                                   width="{{ w }}"
                                   height="{{ rowHeight }}"
            />

            <frame pos="{{ positionBoxWidth + columnSpacing }} {{ rowHeight / -2f }}" z-index="10">
                <!-- Flag -->
                <Flag height="{{ rowInnerHeight }}" />

                <frame pos="{{ rowInnerHeight * 1.5 + columnSpacing }} 0">
                    <quad id="club_bg"
                          size="{{ rowInnerHeight * 1.5 }} {{ rowInnerHeight * 0.75 }}"
                          bgcolor="f00"
                          valign="center"
                          hidden="1"
                    />
                    <label id="club"
                           class="text-primary"
                           text="CLUB"
                           pos="{{ rowInnerHeight * 0.75 }} 0"
                           size="{{ rowInnerHeight * 2f }} {{ rowInnerHeight }}"
                           valign="center2"
                           halign="center"
                           textsize="{{ Theme.UI_FontSize*2 }}"
                           textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                    />
                </frame>

                <!-- Player Name -->
                <label id="name"
                       class="text-xl"
                       pos="{{ rowInnerHeight * 3f + columnSpacing * 2f }} 0"
                       size="{{ w / 3f }} {{ rowHeight }}"
                       valign="center2"
                       textfont="{{ Font.Regular }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                />
            </frame>
            <frame id="details_wrapper" z-index="10">
                <!-- Spec/Disconnected -->
                <label id="spec_disconnected_label" pos="0 {{ rowHeight / -2f }}"
                       class="text-muted"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                />

                <!-- Round Points -->
                <label id="round_points" pos="0 {{ rowHeight / -2f }}"
                       class="text-primary"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_GainedPoints_Color }}"
                />

                <!-- Custom Label (FINALIST, etc) -->
                <label id="custom_label"
                       class="text-primary"
                       pos="0 {{ rowHeight / -2f }}"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                />

                <!-- Best Time -->
                <label id="best_time"
                       class="text-primary"
                       pos="{{ w - columnSpacing - 55f }} {{ rowHeight / -2f }}"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                       text="0:00.000"
                />

                <!-- Player Score -->
                <label id="score"
                       class="text-primary"
                       pos="{{ w - columnSpacing }} {{ rowHeight / -2f }}"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                />
            </frame>
            <PlayerActions x="{{ w }}"
                           y="{{ rowHeight / -2f }}"
                           rowHeight="{{ rowHeight }}"
                           rowSpacing="{{ rowSpacing }}"
                           textsize="{{ textSize }}"
            />
        </framemodel>
    </template>

    <script resource="ScoreboardModule.Scripts.Framemodel" once="true" />
</component>
