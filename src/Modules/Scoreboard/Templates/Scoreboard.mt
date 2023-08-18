<component>
    <using namespace="System.Linq"/>

    <import component="Scoreboard.Components.BackgroundBox" as="Background"/>
    <import component="Scoreboard.Components.Settings" as="Settings"/>

    <property type="int" name="MaxPlayers" default="0"/>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="71.2"/>
    <property type="double" name="backgroundBorderRadius" default="3.0"/>
    <property type="double" name="headerHeight" default="19.0"/>
    <property type="double" name="rowHeight" default="8.0"/>
    <property type="double" name="rowInnerHeight" default="5.0"/>
    <property type="double" name="pointsWidth" default="16.0"/>
    <property type="double" name="rowSpacing" default="0.6"/>
    <property type="double" name="padding" default="3.0"/>
    <property type="double" name="innerSpacing" default="1.6"/>

<!--    <property type="string" name="headerColor" default="0c0f31"/>-->
<!--    <property type="string" name="color" default="041138"/>-->
<!--    <property type="string" name="primaryColor" default="1b3598"/>-->

    <property type="string" name="headerColor" default="111111"/>
    <property type="string" name="backgroundColor" default="222222"/>
    <property type="string" name="primaryColor" default="bb0755"/>

    <template layer="ScoresTable">
        <framemodel id="PointsBackground">
            <frame size="1 1">
                <!-- top left corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="fff"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ pointsWidth }} {{ -rowHeight }}" rot="180">
                <!-- bottom right corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="fff"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ pointsWidth }} 0" rot="90">
                <!-- top right corner -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="fff"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="0 {{ -rowHeight }}" rot="-90">
                <!-- bottom left -->
                <quad size="2 2"
                      class="modulate"
                      modulatecolor="fff"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <quad class="set" pos="1 0" size="{{ pointsWidth - 2.0 }} 1" bgcolor="fff"/> <!-- top bar -->
            <quad class="set" pos="1 {{ 1.0 - rowHeight }}" size="{{ pointsWidth - 2.0 }} 1"
                  bgcolor="fff"/> <!-- bottom bar -->
            <quad class="set" pos="0 -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="fff"/> <!-- left bar -->
            <quad class="set" pos="{{ pointsWidth - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}"
                  bgcolor="fff"/> <!-- right bar -->
            <quad class="set" pos="1 -1" size="{{ pointsWidth - 2.0 }} {{ rowHeight - 2.0 }}"
                  bgcolor="fff"/> <!-- center quad -->
        </framemodel>

        <framemodel id="PositionBackground">
            <frame size="1 1">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ rowHeight * 1.2 }} {{ -rowHeight }}" rot="180">
                <!-- bottom right corner -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ rowHeight * 1.2 }} 0" rot="90">
                <!-- top right corner -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="0 {{ -rowHeight }}" rot="-90">
                <!-- bottom left -->
                <quad size="2 2"
                      modulatecolor="{{ primaryColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <quad pos="1 0" size="{{ rowHeight * 1.2 - 2.0 }} 1" bgcolor="{{ primaryColor }}"/> <!-- top bar -->
            <quad pos="1 {{ 1.0 - rowHeight }}" size="{{ rowHeight * 1.2 - 2.0 }} 1"
                  bgcolor="{{ primaryColor }}"/> <!-- bottom bar -->
            <quad pos="0 -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="{{ primaryColor }}"/> <!-- left bar -->
            <quad pos="{{ rowHeight * 1.2 - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}"
                  bgcolor="{{ primaryColor }}"/> <!-- right bar -->
            <quad pos="1 -1" size="{{ rowHeight * 1.2 - 2.0 }} {{ rowHeight - 2.0 }}"
                  bgcolor="{{ primaryColor }}"/> <!-- center quad -->
        </framemodel>

        <framemodel id="player_row">
            <!-- Scroll activation -->
            <quad size="{{ w }} {{ rowHeight + rowSpacing }}" ScriptEvents="1"/>

            <!-- Background -->
            <quad pos="{{ padding + 1.0 }} 0" 
                  size="{{ w - padding * 2.0 - 2.0 }} {{ rowHeight }}"
                  bgcolor="{{ headerColor }}"
                  opacity="0.6"/>
            <quad pos="{{ w - padding - 1.0 }} -1"
                  size="1 {{ rowHeight - 2.0 }}"
                  bgcolor="{{ headerColor }}" 
                  opacity="0.6"
                  z-index="10"/>
            <frame size="1 1" pos="{{ w - padding }}" rot="90">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="{{ headerColor }}"
                      opacity="0.6"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ w - padding }} {{ -rowHeight }}" rot="180">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="{{ headerColor }}"
                      opacity="0.6"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>

            <!-- Position Box -->
            <frameinstance id="position_box" modelid="PositionBackground" pos="{{ padding }} 0" z-index="5"/>
            <label id="position" pos="{{ padding + (rowHeight * 0.6) }} {{ rowHeight / -2.0 + 0.25 }}" valign="center"
                   halign="center" textsize="2.6" textfont="GameFontBlack" z-index="5"/>

            <frame pos="{{ padding + (rowHeight * 1.2) + innerSpacing * 2.0 }} {{ rowHeight / -2.0 }}">
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

                <!-- Club Tag -->
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
            <frameinstance id="points_box_outer" modelid="PointsBackground"
                           pos="{{ w - padding - pointsWidth * 0.75 - rowHeight * 0.25 * 0.5 }} {{ rowHeight * 0.25 * -0.5 }}"
                           size="{{ pointsWidth }} {{ rowHeight }}" scale="0.75" hidden="1"/>
            <frameinstance id="points_box" modelid="PointsBackground"
                           pos="{{ w - padding - pointsWidth * 0.65 - rowHeight * 0.4 * 0.5 -0.05 }} {{ rowHeight * 0.33 * -0.5 }}"
                           size="{{ pointsWidth }} {{ rowHeight }}" scale="0.65" hidden="1"/>
            <label id="points"
                   pos="{{ w - padding - (pointsWidth * 0.75) / 2.0 - rowHeight * 0.25 * 0.5 }} {{ rowHeight / -2.0 + 0.4 }}"
                   text="x" 
                   valign="center" 
                   halign="center" 
                   textsize="2" 
                   textcolor="333" 
                   textfont="GameFontSemiBold"
            />
        </framemodel>

        <frame pos="{{ w / -2.0 }} {{ h / 2.0 + 10.0 }}">
            <Background w="{{ w }}" h="{{ h }}" 
                        radius="{{ backgroundBorderRadius }}"
                        headerHeight="{{ headerHeight }}"
                        headerColor="{{ headerColor }}"
                        color="{{ backgroundColor }}"
                        gradientColor="{{ primaryColor }}"
            />

            <label id="map_name" pos="5 -7.5" text="MAP NAME" valign="center" textsize="3.3" textfont="GameFontBlack"/>
            <label id="author_name" pos="5 -12.5" textprefix="by " text="AUTHOR NAME" valign="center" textsize="1.6"
                   textfont="GameFontRegular"/>

            <frame pos="{{ w - 0.25 }} -6.5">
                <frame size="1 1" pos="-2.6 3" rot="90">
                    <!-- top right corner -->
                    <quad size="2 2"
                          modulatecolor="{{ primaryColor }}"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
                </frame>
                <quad pos="-3.6 2" size="1 6.3" bgcolor="{{ primaryColor }}"/> <!-- right bar -->
                <frame size="1 1" pos="-2.6 -5.3" rot="180">
                    <!-- bottom right corner -->
                    <quad size="2 2"
                          modulatecolor="{{ primaryColor }}"
                          image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
                </frame>
                
                <quad size="8.3 90"
                      rot="-90"
                      pos="-48.6 3.0"
                      modulatecolor="{{ primaryColor }}"
                      halign="right"
                      valign="center"
                      image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"/>
                <label id="round_label" 
                       text="ROUND 1/?" 
                       pos="-4 -2.2"
                       valign="center"
                       halign="right"
                       textsize="1.9"
                       textfont="GameFontBlack"/>
                <label id="gradient_label_small" 
                       text="MODE" 
                       pos="-4 1.1" 
                       valign="center"
                       halign="right"
                       textsize="0.7"
                       opacity="0.75"
                       textfont="GameFontSemiBold"
                />
            </frame>
            
            <!-- Sub Text (Below highlighted box) -->
            <label id="sub_text"
                   pos="{{ w - 7 }} -14.5" 
                   textcolor="fff" 
                   valign="center" 
                   halign="right"
                   textsize="1"
                   textfont="GameFontRegular"
            />

            <!-- Settings Icon -->
            <label id="settings_icon"
                   pos="{{ w - 4.5 }} -14.7" 
                   size="5 5"
                   textcolor="fff" 
                   valign="center" 
                   halign="center"
                   textsize="1"
                   text=""
                   textfont="GameFontRegular"
                   ScriptEvents="1"
                   focusareacolor1="0000"
                   focusareacolor2="0000"
            />

            <frame id="rows_wrapper" pos="0 {{ -headerHeight - padding }}" size="{{ w }} {{ h - padding }}">
                <frame id="rows_inner">
                    <frame pos="{{ padding }} {{ h + padding }}">
                        <frame pos="{{ padding }} {{ -padding }}">
                            <Settings w="{{ w - padding * 2.0 }}" h="{{ h - padding * 2.0 }}" />
                        </frame>
                    </frame>
                    <frame id="frame_scroll" size="{{ w }} {{ h - padding }}">
                        <frameinstance modelid="player_row"
                                       foreach="int rowId in Enumerable.Range(0, MaxPlayers * 2).ToList()"
                                       pos="0 {{ -rowId * (rowHeight + rowSpacing) }}"
                        />
                    </frame>
                </frame>
            </frame>
        </frame>
    </template>

    <script>
        <!--
        #Include "MathLib" as ML
        #Include "TextLib" as TL
        #Include "ColorLib" as CL
        #Include "Libs/Nadeo/ModeLibs/TrackMania/MV_Utils.Script.txt" as MV_Utils
        #Include "ManiaApps/Nadeo/TMxSM/Race/UIModules/ScoresTable_Common.Script.txt" as UIModules_ScoresTable
        #Include "ManiaApps/Nadeo/TMxSM/Race/UIModules/Helpers_Client.Script.txt" as RaceHelpers
    
        #Const C_Status_Disconnected	0
        #Const C_Status_Spawned			1
        #Const C_Status_NotSpawned		2
        #Const C_Status_Spectating		3

        #Const UIModules_ScoresTable::C_Mode_BestTime as C_Mode_BestTime
        #Const UIModules_ScoresTable::C_Mode_PrevTime as C_Mode_PrevTime
        #Const UIModules_ScoresTable::C_Mode_LapTime as C_Mode_LapTime
        #Const UIModules_ScoresTable::C_Mode_Points as C_Mode_Points
        #Const UIModules_ScoresTable::C_Mode_Laps as C_Mode_Laps
        #Const UIModules_ScoresTable::C_Mode_Trophy as C_Mode_Trophy
        #Const UIModules_ScoresTable::C_Mode_RaceProgression as C_Mode_RaceProgression

        #Const C_CustomPoints_Text 0
        #Const C_CustomPoints_Color 1
        
        declare Integer CurrentScoreMode;
        declare CMlFrame RowsFrame;
        declare Boolean SettingsVisible;
        
        Boolean ShouldShowPointsBox() {
            return CurrentScoreMode == C_Mode_LapTime
                || CurrentScoreMode == C_Mode_Laps
                || CurrentScoreMode == C_Mode_Points;
        }
        
        Void UpdateScoreboardLayout() {
            declare shouldShowPoints = ShouldShowPointsBox();
            
            foreach(Control in RowsFrame.Controls){
                declare playerRow = (Control as CMlFrame);
                declare pointsBoxOuterFrame = (playerRow.GetFirstChild("points_box_outer") as CMlFrame);
                declare pointsBoxFrame = (playerRow.GetFirstChild("points_box") as CMlFrame);
                declare pointsLabel = (playerRow.GetFirstChild("points") as CMlLabel);
                
                pointsBoxOuterFrame.Visible = shouldShowPoints;
                pointsBoxFrame.Visible = shouldShowPoints;
                pointsLabel.Visible = shouldShowPoints;
            }
        }
        
        Text StripLeadingZeroes(Text input) {
            return TL::RegexReplace("^[0.:]+", input, "", "");
        }

        Integer[CSmScore] GetSortedScores() {
            declare persistent Boolean[Text] LibScoresTable2_Settings for This;
            declare SortedScores = Integer[CSmScore];
            declare c = 0;
    
            foreach (Score in Scores) {
                declare LibST_Status for Score.User = C_Status_Disconnected;
                declare Weight = c;
                if (
                    LibScoresTable2_Settings.existskey("SortSpec") &&
                    LibScoresTable2_Settings["SortSpec"] &&
                    LibST_Status == C_Status_Spectating
                ) Weight += Scores.count;
                if (
                    LibScoresTable2_Settings.existskey("SortDisco") &&
                    LibScoresTable2_Settings["SortDisco"] &&
                    LibST_Status == C_Status_Disconnected
                ) Weight += 2 * Scores.count;
                SortedScores[Score] = Weight;
                c += 1;
            }
    
            return SortedScores.sort();
        }
        
        Void SetOuterPointsColor(CMlFrame pointsBoxOuterFrame, Vec3 color) {
            Page.GetClassChildren("modulate", pointsBoxOuterFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).ModulateColor = color;
            }
            Page.GetClassChildren("set", pointsBoxOuterFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).BgColor = color;
            }
        }
        
        Void SetCountryFlag(CMlQuad flagQuad, Text login){
            if(login != "" && !TL::StartsWith("*fakeplayer", login)){
                flagQuad.ImageUrl = "file://ZoneFlags/Login/" ^ login ^ "/country";
                flagQuad.ModulateColor = <1.0, 1.0, 1.0>;
                flagQuad.Opacity = 1.0;
            }else{
                flagQuad.ImageUrl = "file://Media/Manialinks/Nadeo/TMNext/Menus/Common/Common_Flag_Mask.dds";
                flagQuad.ModulateColor = CL::HexToRgb("{{ headerColor }}");
                flagQuad.Opacity = 0.5;
            }
        }
        
        Void UpdateScoreAndPoints(CSmScore Score, CMlLabel scoreLabel, CMlLabel scoreTwoLabel, CMlLabel pointsLabel, CMlLabel customLabel, CMlLabel roundPointsLabel){
            declare netread Text[][Text] Net_TMxSM_ScoresTable_CustomPoints for Teams[0];
            declare netread Integer[Text] Net_TMxSM_ScoresTable_CustomTimes for Teams[0];
            declare Boolean CustomPointsEnabled = Net_TMxSM_ScoresTable_CustomPoints.existskey(Score.User.WebServicesUserId);
            
            if (CustomPointsEnabled && CurrentScoreMode != C_Mode_Trophy) {
                if (Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId].existskey(C_CustomPoints_Text)) {
                    customLabel.Value = Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId][C_CustomPoints_Text];
                }else{
                    customLabel.Value = "";
                }
                if (Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId].existskey(C_CustomPoints_Color)) {
                    customLabel.TextColor = CL::HexToRgb(Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId][C_CustomPoints_Color]);
                    customLabel.Opacity = 1.0;
                }else{
                    customLabel.TextColor = <1.0, 1.0, 1.0>;
                    customLabel.Opacity = 0.5;
                }
            } else if (CurrentScoreMode == C_Mode_Points) {
                //SetRacePoints(Frame_RoundPoints, Score.RoundPoints, IsLocalPlayer);
                customLabel.Value = "";
                pointsLabel.Value = TL::ToText(Score.Points);
                
                
                if(Score.PrevRaceTimes.count > 0 && Score.PrevRaceTimes[Score.PrevRaceTimes.count - 1] > 0){
                    scoreLabel.Value = TL::TimeToText(Score.PrevRaceTimes[Score.PrevRaceTimes.count - 1], True, True);
                }else{
                    declare CSmPlayer Driver for Score;
                    if(Driver.SpawnStatus == CSmPlayer::ESpawnStatus::NotSpawned){
                        scoreLabel.Value = "DNF";
                    }else{
                        scoreLabel.Value = "";
                    }
                }
                
                if(Score.RoundPoints != 0){
                    declare sign = "+";
                    if(Score.RoundPoints < 0){
                        sign = "-";
                    }
                    roundPointsLabel.Value = sign ^ Score.RoundPoints;
                }else{
                    roundPointsLabel.Value = "";
                }
            } else if (Net_TMxSM_ScoresTable_CustomTimes.existskey(Score.User.WebServicesUserId)) {
                customLabel.Value = "";
                if (Net_TMxSM_ScoresTable_CustomTimes[Score.User.WebServicesUserId] > 0){
                    scoreLabel.Value = TL::TimeToText(Net_TMxSM_ScoresTable_CustomTimes[Score.User.WebServicesUserId], True, True);
                } else {
                    scoreLabel.Value = "nope";
                }
            } else if (CurrentScoreMode == C_Mode_BestTime && Score.BestRaceTimes.count > 0) {
                customLabel.Value = "";
                scoreLabel.Value = TL::TimeToText(Score.BestRaceTimes[Score.BestRaceTimes.count - 1], True, True);
                log(TL::TimeToText(Score.BestRaceTimes[Score.BestRaceTimes.count - 1], True, True));
            } else if (CurrentScoreMode == C_Mode_PrevTime && Score.PrevRaceTimes.count > 0) {
                customLabel.Value = "";
                scoreLabel.Value = TL::TimeToText(Score.PrevRaceTimes[Score.PrevRaceTimes.count - 1], True, True);
            } else if (CurrentScoreMode == C_Mode_LapTime && Score.BestLapTimes.count > 0) {
                customLabel.Value = "";
                scoreLabel.Value = TL::TimeToText(Score.BestLapTimes[Score.BestLapTimes.count - 1], True, True);
            } else if (CurrentScoreMode == C_Mode_Laps && Score.BestRaceTimes.count > 0) {
                customLabel.Value = "";
                scoreLabel.Value = TL::TimeToText(Score.BestRaceTimes[Score.BestRaceTimes.count - 1], True, True);
                pointsLabel.Value = ""^Score.BestRaceTimes.count;
            } else if (CurrentScoreMode == C_Mode_RaceProgression) {
                customLabel.Value = "";
                declare netread Int2 Net_TMxSM_ScoresTable_RaceProgression for Score;
                pointsLabel.Value = ""^Net_TMxSM_ScoresTable_RaceProgression.X;
                if (Net_TMxSM_ScoresTable_RaceProgression.Y > 0) {
                    scoreLabel.Value = TL::TimeToText(Net_TMxSM_ScoresTable_RaceProgression.Y, True, True);
                } else {
                    scoreLabel.Value = "0:00.000";
                }
            } else if (CurrentScoreMode == C_Mode_Trophy) {
                customLabel.Value = "";
                scoreLabel.Value = "0:00.000"; //todo: implement trophy mode
            } else {
                customLabel.Value = "";
                scoreLabel.Value = "0:00.000";
            }
            
            // scoreTwoLabel.Value = scoreLabel.Value;
            scoreTwoLabel.Value = "";
            scoreLabel.Value = StripLeadingZeroes(scoreLabel.Value);
            
            if(customLabel.Value != "" || roundPointsLabel.Value != ""){
                if(scoreLabel.Value == ""){
                    customLabel.RelativePosition_V3.X = scoreLabel.RelativePosition_V3.X;
                }else{
                    customLabel.RelativePosition_V3.X = scoreLabel.RelativePosition_V3.X - scoreLabel.ComputeWidth(scoreLabel.Value) - {{ innerSpacing }};
                }
            }
            if(roundPointsLabel.Value != ""){
                roundPointsLabel.RelativePosition_V3.X = customLabel.RelativePosition_V3.X - customLabel.ComputeWidth(customLabel.Value);
            }
        }
        
        Void SetMapAndAuthorName() {
            declare mapNameLabel <=> (Page.MainFrame.GetFirstChild("map_name") as CMlLabel);
            declare authorName <=> (Page.MainFrame.GetFirstChild("author_name") as CMlLabel);
        
            mapNameLabel.Value = Map.MapName;
            authorName.Value = Map.AuthorNickName;
        }
        
        Text GetRecordText() {
            return "$999AUTHOR TIME | $fff" ^ TL::TimeToText(Map.TMObjective_AuthorTime, True, True) ^ " | " ^ Map.AuthorNickName;
        }
        
        Void UpdateHeaderInfo() {
            declare subTextLabel <=> (Page.MainFrame.GetFirstChild("sub_text") as CMlLabel);
            declare gradientSmallLabel <=> (Page.MainFrame.GetFirstChild("gradient_label_small") as CMlLabel);
            declare roundLabel <=> (Page.MainFrame.GetFirstChild("round_label") as CMlLabel);
            
            subTextLabel.Value = GetRecordText();
            
            declare Owner <=> MV_Utils::GetOwner(This);
            
            if (CurrentScoreMode == C_Mode_BestTime || CurrentScoreMode == C_Mode_PrevTime){
                declare timeLimit = RaceHelpers::GetTimeLimit(Teams[0]);
                roundLabel.Value = "TIME LIMIT | ";
                if(timeLimit <= 0){
                    roundLabel.Value ^= "UNLIMITED";
                }else{                   
                    roundLabel.Value ^= TL::TimeToText(timeLimit);
                }
            }else if (CurrentScoreMode == C_Mode_LapTime || CurrentScoreMode == C_Mode_Laps){
				declare Integer LapCurrent = -1;
				if(Owner != Null){
                    declare Integer LapCurrent = RaceHelpers::GetPlayerLap(Owner);
				}
				declare LapsTotal = RaceHelpers::GetLapsNb(Teams[0]);
                roundLabel.Value = TL::Compose("%1 | %2/%3", _("|Race|Lap"), TL::ToText(LapCurrent), TL::ToText(LapsTotal));
            }else if (CurrentScoreMode == C_Mode_Points) {
                roundLabel.Value = TL::Compose("ROUND | %1/%2", TL::ToText(-1), TL::ToText(-1));
            }else{
                roundLabel.Value = "";
            }
            
            gradientSmallLabel.Value = CurrentServerModeName;
            SetMapAndAuthorName();
        }
        
        Void UpdateScrollSize(Integer playerRowsFilled) {
            declare scrollN = 0;
            if(playerRowsFilled >= 8){
                scrollN = playerRowsFilled - 8;
            }
            RowsFrame.ScrollMax = <0.0, {{ rowHeight + rowSpacing }} * scrollN * 1.0>;
        }
        
        Void UpdateScoreTable() {
            if (CurrentScoreMode == C_Mode_Points) {
                foreach (PlayerIndex => Player in Players) {
                    if (Player.Score == Null) continue;
                    
                    declare CSmPlayer Driver for Player.Score;
                    Driver <=> Player;
                }
            }
        
            declare cursor = 0;
           
            foreach(Score => Weight in GetSortedScores()){
                declare CUser User <=> Score.User;
                declare playerRow = (RowsFrame.Controls[cursor] as CMlFrame);
                declare positionLabel = (playerRow.GetFirstChild("position") as CMlLabel);
                declare clubBg = (playerRow.GetFirstChild("club_bg") as CMlQuad);
                declare clubLabel = (playerRow.GetFirstChild("club") as CMlLabel);
                declare nameLabel = (playerRow.GetFirstChild("name") as CMlLabel);
                declare flagQuad = (playerRow.GetFirstChild("flag") as CMlQuad);
                declare pointsLabel = (playerRow.GetFirstChild("points") as CMlLabel);
                declare scoreLabel = (playerRow.GetFirstChild("score") as CMlLabel);
                declare scoreTwoLabel = (playerRow.GetFirstChild("score_two") as CMlLabel);
                declare roundPointsLabel = (playerRow.GetFirstChild("round_points") as CMlLabel);
                declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
                declare pointsBoxOuterFrame = (playerRow.GetFirstChild("points_box_outer") as CMlFrame);
                declare pointsBoxFrame = (playerRow.GetFirstChild("points_box") as CMlFrame);
                
                positionLabel.Value = (cursor + 1) ^ "";
                clubLabel.Value = User.ClubTag;
                nameLabel.Value = User.Name;
                
                if(clubLabel.Value != ""){
                    clubBg.Opacity = 1.0;
                }else{
                    clubBg.Opacity = 0.5;
                }
                
                UpdateScoreAndPoints(Score, scoreLabel, scoreTwoLabel, pointsLabel, customLabel, roundPointsLabel);
                SetCountryFlag(flagQuad, User.Login);
                
                if(ShouldShowPointsBox()){
                    scoreLabel.RelativePosition_V3.X = pointsBoxOuterFrame.RelativePosition_V3.X - {{ innerSpacing * 2.0 }};
                    pointsBoxOuterFrame.Show();
                    pointsBoxFrame.Show();
                }else{
                    scoreLabel.RelativePosition_V3.X = {{ w - padding - innerSpacing * 2.0 }};
                    pointsBoxOuterFrame.Hide();
                    pointsBoxFrame.Hide();
                }
                scoreTwoLabel.RelativePosition_V3.X = scoreLabel.RelativePosition_V3.X;
                
                playerRow.Show();
                
                cursor += 1;
            }
            
            //Hide remaining rows
            for(i, cursor, {{ MaxPlayers - 1 }}){
                declare playerRow = (RowsFrame.Controls[i] as CMlFrame);
                playerRow.Hide();
            }
            
            UpdateHeaderInfo();
            UpdateScrollSize(cursor);
        }
        
        Void ToggleShowSettings() {
            declare wrapperInnerFrame <=> (Page.MainFrame.GetFirstChild("rows_inner") as CMlFrame);
            declare y = 0.0;
            SettingsVisible = !SettingsVisible;
        
            if(SettingsVisible) {
                y = {{ h + padding }} * -1.0;
            }
            
            declare targetState = "<frame pos='0 " ^ y ^ "' />";
            AnimMgr.Add(wrapperInnerFrame, targetState, 320, CAnimManager::EAnimManagerEasing::ExpInOut);
        }
        -->
    </script>

    <script>
        <!--
        *** OnInitialization ***
        ***
            declare netread Integer Net_TMxSM_ScoresTable_ScoreMode for Teams[0];
            declare Integer scoreboardUpdateInterval = 200;
            declare Integer lastScoreboardUpdate = 0;
            RowsFrame <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            
            RowsFrame.ScrollActive = True;
            RowsFrame.ScrollGridSnap = True;
            RowsFrame.ScrollMin = <0.0, 0.0>;
            RowsFrame.ScrollMax = <0.0, {{ MaxPlayers * (rowHeight + rowSpacing) - h }} * 1.0>;
            RowsFrame.ScrollGrid = <0.0, {{ rowHeight + rowSpacing }} * 1.0>;
            
            CurrentScoreMode = -1;
            SettingsVisible = False;
        ***
        
        *** OnLoop *** 
        ***
            if(lastScoreboardUpdate + scoreboardUpdateInterval < Now){
                UpdateScoreTable();
                lastScoreboardUpdate = Now;
            }
            
            if(CurrentScoreMode != Net_TMxSM_ScoresTable_ScoreMode){
                CurrentScoreMode = Net_TMxSM_ScoresTable_ScoreMode;
                UpdateScoreboardLayout();
                log("[EvoSC#] Update scoreboard layout.");
            }
        ***
        
        *** OnMouseClick *** 
        ***
            if (Event.Control.ControlId == "settings_icon") {
                ToggleShowSettings();
            }
        ***
        -->
    </script>

    <script resource="EvoSC.Scripts.UIScripts"/>
</component>
