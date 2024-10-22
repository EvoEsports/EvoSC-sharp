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

            <frame pos="{{ positionBoxWidth + columnSpacing }} {{ rowHeight / -2.0 }}" z-index="10">
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
                           size="{{ rowInnerHeight * 2.0 }} {{ rowInnerHeight }}"
                           valign="center2"
                           halign="center"
                           textsize="{{ Theme.UI_FontSize*2 }}"
                           textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                    />
                </frame>

                <!-- Player Name -->
                <label id="name"
                       pos="{{ rowInnerHeight * 3.0 + columnSpacing * 2.0 }} 0"
                       size="{{ w / 3.0 }} {{ rowHeight }}"
                       valign="center2"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textfont="{{ Font.Regular }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                />
            </frame>
            <frame id="details_wrapper" z-index="10">
                <!-- Spec/Disconnected -->
                <label id="spec_disconnected_label" pos="0 {{ rowHeight / -2.0 }}"
                       class="text-muted"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                />

                <!-- Round Points -->
                <label id="round_points" pos="0 {{ rowHeight / -2.0 }}"
                       class="text-primary"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_GainedPoints_Color }}"
                />

                <!-- Custom Label (FINALIST, etc) -->
                <label id="custom_label"
                       class="text-primary"
                       pos="0 {{ rowHeight / -2.0 }}"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                />

                <!-- Best Time -->
                <label id="best_time"
                       class="text-primary"
                       pos="{{ w - columnSpacing - 55.0 }} {{ rowHeight / -2.0 }}"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                       text="0:00.000"
                />

                <!-- Player Score -->
                <label id="score"
                       class="text-primary"
                       pos="{{ w - columnSpacing }} {{ rowHeight / -2.0 }}"
                       valign="center2"
                       halign="right"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       textcolor="{{ Theme.ScoreboardModule_Text_Color }}"
                />
            </frame>
            <PlayerActions x="{{ w }}"
                           y="{{ rowHeight / -2.0 }}"
                           rowHeight="{{ rowHeight }}"
                           rowSpacing="{{ rowSpacing }}"
                           textsize="{{ textSize }}"
            />
        </framemodel>
    </template>

    <script once="true">
        <!--
        Void ShowPlayerActions(CMlFrame playerRow) {
            declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
            declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
            declare backgroundWrapper = (playerRow.GetFirstChild("player_row_bg") as CMlFrame);
            playerActions.Show();
            detailsWrapper.Hide();
            backgroundWrapper.Size.X = ({{ (w - positionBoxWidth) - actionButtonCount*(rowHeight*1.2) - actionButtonCount*rowSpacing - 0.1 }}) * 1.0;
        }
        
        Void HidePlayerActions(CMlFrame playerRow) {
            declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
            declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
            declare backgroundWrapper = (playerRow.GetFirstChild("player_row_bg") as CMlFrame);
            playerActions.Hide();
            detailsWrapper.Show();
            backgroundWrapper.Size.X = {{ (w - positionBoxWidth) }} * 1.0;
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
        
        Vec3 GetCustomLabelColor(Text value, Text defaultColorHex){
            declare customLabelColorHex = "";
            switch(value){
                case _("|Status|Winner"): customLabelColorHex = "{{ Theme.ScoreboardModule_WinnerColor }}";
                case _("|Status|Finalist"): customLabelColorHex = "{{ Theme.ScoreboardModule_FinalistColor }}";
            }
            
            if(customLabelColorHex == ""){
                customLabelColorHex = defaultColorHex;
            }
            
            return CL::HexToRgb(customLabelColorHex);
        }
        
        Void SetCustomLabel(CMlFrame playerRow, Text value, Text hexColor){
            declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
            customLabel.Value = value;
            customLabel.TextColor = GetCustomLabelColor(value, hexColor);
            SetPlayerHighlightColor(playerRow, customLabel.TextColor);
        }
        
        Void HideCustomLabel(CMlFrame playerRow){
            declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
            customLabel.Value = "";
            ResetPlayerHighlightColor(playerRow);
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
                RowMouseOver(backgroundFrame);
                continue;
            }
        ***
        
        *** OnMouseOut ***
        ***
            if(Event.Control.ControlId == "player_row_trigger"){
                declare parentFrame = (Event.Control.Parent as CMlFrame);
                declare backgroundFrame <=> (parentFrame.GetFirstChild("player_row_bg") as CMlFrame);
                RowMouseOut(backgroundFrame);
                continue;
            }
        ***
        -->
    </script>
</component>
