<component>
    <using namespace="System.Linq"/>

    <import component="Scoreboard.Components.BackgroundBox" as="Background"/>

    <property type="int" name="MaxPlayers" default="0"/>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="71.2"/>
    <property type="double" name="backgroundBorderRadius" default="3.0"/>
    <property type="double" name="headerHeight" default="18.0"/>
    <property type="double" name="rowHeight" default="8.0"/>
    <property type="double" name="pointsWidth" default="16.0"/>
    <property type="double" name="rowSpacing" default="0.6"/>
    <property type="double" name="padding" default="3.0"/>
    <property type="double" name="innerSpacing" default="1.6"/>
    <property type="string" name="positionColor" default="1b3598" />

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
            <quad class="set" pos="1 0" size="{{ pointsWidth - 2.0 }} 1" bgcolor="fff" /> <!-- top bar -->
            <quad class="set" pos="1 {{ 1.0 - rowHeight }}" size="{{ pointsWidth - 2.0 }} 1" bgcolor="fff" /> <!-- bottom bar -->
            <quad class="set" pos="0 -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="fff" /> <!-- left bar -->
            <quad class="set" pos="{{ pointsWidth - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="fff" /> <!-- right bar -->
            <quad class="set" pos="1 -1" size="{{ pointsWidth - 2.0 }} {{ rowHeight - 2.0 }}" bgcolor="fff" /> <!-- center quad -->
        </framemodel>
        
        <framemodel id="PositionBackground">
            <frame size="1 1">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="{{ positionColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ rowHeight * 1.2 }} {{ -rowHeight }}" rot="180">
                <!-- bottom right corner -->
                <quad size="2 2"
                      modulatecolor="{{ positionColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ rowHeight * 1.2 }} 0" rot="90">
                <!-- top right corner -->
                <quad size="2 2"
                      modulatecolor="{{ positionColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="0 {{ -rowHeight }}" rot="-90">
                <!-- bottom left -->
                <quad size="2 2"
                      modulatecolor="{{ positionColor }}"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <quad pos="1 0" size="{{ rowHeight * 1.2 - 2.0 }} 1" bgcolor="{{ positionColor }}" /> <!-- top bar -->
            <quad pos="1 {{ 1.0 - rowHeight }}" size="{{ rowHeight * 1.2 - 2.0 }} 1" bgcolor="{{ positionColor }}" /> <!-- bottom bar -->
            <quad pos="0 -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="{{ positionColor }}" /> <!-- left bar -->
            <quad pos="{{ rowHeight * 1.2 - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="{{ positionColor }}" /> <!-- right bar -->
            <quad pos="1 -1" size="{{ rowHeight * 1.2 - 2.0 }} {{ rowHeight - 2.0 }}" bgcolor="{{ positionColor }}" /> <!-- center quad -->
        </framemodel>
        
        <framemodel id="player_row">
            <!-- Scroll activation -->
            <quad size="{{ w }} {{ rowHeight + rowSpacing }}" ScriptEvents="1"/>
            
            <!-- Background -->
            <quad pos="{{ padding + 1.0 }} 0" size="{{ w - padding * 2.0 - 2.0 }} {{ rowHeight }}" bgcolor="0c0f31" opacity="0.93"/>
            <quad pos="{{ w - padding - 1.0 }} -1" size="1 {{ rowHeight - 2.0 }}" bgcolor="0c0f31" opacity="0.93" z-index="10"/>
            <frame size="1 1" pos="{{ w - padding }}" rot="90">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="0c0f31"
                      opacity="0.93"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>
            <frame size="1 1" pos="{{ w - padding }} {{ -rowHeight }}" rot="180">
                <!-- top left corner -->
                <quad size="2 2"
                      modulatecolor="0c0f31"
                      opacity="0.93"
                      image="file:///Media/Painter/Stencils/01-EllipseRound/Brush.tga"/>
            </frame>

            <!-- Position Box -->
            <frameinstance id="position_box" modelid="PositionBackground" pos="{{ padding }} 0" />
            <label id="position" pos="{{ padding + (rowHeight * 0.6) }} {{ rowHeight / -2.0 + 0.25 }}" valign="center" halign="center" textsize="2.6" textfont="GameFontBlack"/>

            <frame pos="{{ padding + (rowHeight * 1.2) + innerSpacing }} 0">
                <!-- Flag -->
                <quad id="flag" size="{{ rowHeight * 0.8 }} {{ rowHeight - innerSpacing * 2.0 }}" pos="{{ innerSpacing }} {{ -innerSpacing }}" bgcolor="00000033"/>
                
                <frame pos="{{ rowHeight * 0.8 + innerSpacing * 2.0 }} 0">
                    <label id="club" pos="0 {{ rowHeight / -2.0 + 0.25 }}" valign="center" textsize="2.6" opacity="0.75" textprefix="$i" textfont="GameFontSemiBold"/>
                    <label id="name" pos="0 {{ rowHeight / -2.0 + 0.25 }}" valign="center" textsize="2.6" textfont="GameFontSemiBold"/>
                </frame>
            </frame>
            
            <!-- Points Box -->
            <label id="score" pos="{{ padding + w - 9.0 }} {{ rowHeight / -2.0 + 0.25 }}" valign="center" halign="right" textsize="2.4" textfont="GameFontRegular"/>
            <frameinstance id="points_box_outer" modelid="PointsBackground" pos="{{ w - padding - pointsWidth * 0.75 - rowHeight * 0.25 * 0.5 }} {{ rowHeight * 0.25 * -0.5 }}" size="{{ pointsWidth }} {{ rowHeight }}" scale="0.75" />
            <frameinstance id="points_box" modelid="PointsBackground" pos="{{ w - padding - pointsWidth * 0.65 - rowHeight * 0.4 * 0.5 -0.05 }} {{ rowHeight * 0.33 * -0.5 }}" size="{{ pointsWidth }} {{ rowHeight }}" scale="0.65" />
            <label id="points" pos="{{ w - padding - (pointsWidth * 0.75) / 2.0 - rowHeight * 0.25 * 0.5 }} {{ rowHeight / -2.0 + 0.25 }}" text="x" valign="center" halign="center" textsize="2.6" textcolor="333" textfont="GameFontRegular"/>
        </framemodel>

        <frame pos="{{ w / -2.0 }} {{ h / 2.0 + 10.0 }}">
            <Background w="{{ w }}" h="{{ h }}" radius="{{ backgroundBorderRadius }}" headerHeight="{{ headerHeight }}"/>

            <frame pos="{{ w }} -5.5">
                <quad size="6 80"
                      rot="-90"
                      pos="-40.0 3.0"
                      modulatecolor="c22477"
                      halign="right"
                      valign="center"
                      image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                      opacity="0.6"/>
                <label id="round_label" text="ROUND 1/?" pos="-3 0.25" valign="center" halign="right" textsize="2" textfont="GameFontBlack"/>
            </frame>

            <label id="map_name" pos="5 -6" text="MAP NAME" valign="center" textsize="3" textfont="GameFontSemiBold"/>
            <label id="author_name" pos="5 -11" text="AUTHOR NAME" valign="center" textsize="1.4" textfont="GameFontRegular"/>

            <label id="sub_text" pos="{{ w - 3.0 }} -11" text="" valign="center" halign="right" textsize="1" opacity="0.9" textfont="GameFontRegular"/>

            <frame id="frame_scroll" pos="0 {{ -headerHeight - padding }}" size="{{ w }} {{ h - padding }}">
                <frameinstance modelid="player_row"
                               foreach="int rowId in Enumerable.Range(0, MaxPlayers * 2).ToList()"
                               pos="0 {{ -rowId * (rowHeight + rowSpacing) }}"
                />
            </frame>
        </frame>
    </template>

    <script>
        <!--
        #Include "MathLib" as ML
        #Include "TextLib" as TL
        #Include "ManiaApps/Nadeo/TMxSM/Race/UIModules/ScoresTable_Common.Script.txt" as UIModules_ScoresTable
    
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
        
        Void setOuterPointsColor(CMlFrame pointsBoxOuterFrame, Vec3 color) {
            Page.GetClassChildren("modulate", pointsBoxOuterFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).ModulateColor = color;
            }
            Page.GetClassChildren("set", pointsBoxOuterFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).BgColor = color;
            }
        }
        
        Void updateScoreTable() {
            declare rowsFrame <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            declare cursor = 0;
            declare isPointsBased = True;
            
            foreach(Score => Weight in GetSortedScores()){
                declare CUser User <=> Score.User;
                declare playerRow = (rowsFrame.Controls[cursor] as CMlFrame);
                declare positionLabel = (playerRow.GetFirstChild("position") as CMlLabel);
                declare clubLabel = (playerRow.GetFirstChild("club") as CMlLabel);
                declare nameLabel = (playerRow.GetFirstChild("name") as CMlLabel);
                declare flagQuad = (playerRow.GetFirstChild("flag") as CMlQuad);
                declare pointsBox = (playerRow.GetFirstChild("points_box") as CMlQuad);
                declare pointsLabel = (playerRow.GetFirstChild("points") as CMlLabel);
                declare scoreLabel = (playerRow.GetFirstChild("score") as CMlLabel);
                declare pointsBoxOuterFrame = (playerRow.GetFirstChild("points_box_outer") as CMlFrame);
                declare pointsBoxFrame = (playerRow.GetFirstChild("points_box") as CMlFrame);
                
                positionLabel.Value = (cursor + 1) ^ "";
                clubLabel.Value = TL::StripFormatting(User.ClubTag);
                nameLabel.Value = User.Name;
                nameLabel.RelativePosition_V3.X = clubLabel.RelativePosition_V3.X + clubLabel.ComputeWidth(clubLabel.Value) + {{ innerSpacing * 0.5 }};
                
                flagQuad.ImageUrl = "file://ZoneFlags/Login/" ^ User.Login ^ "/country";
                
                if(Score.BestLapTimes.count > 0 && Score.BestLapTimes[Score.BestLapTimes.count - 1] > 0){
                    scoreLabel.Value = TL::TimeToText(Score.BestLapTimes[Score.BestLapTimes.count - 1], True, True);
                    scoreLabel.Opacity = 1.0;
                }else{
                    scoreLabel.Value = "-"; //no finish
                    scoreLabel.Opacity = 0.5;
                }
                
                if(isPointsBased){
                    pointsLabel.Value = Score.Points ^ "";
                    scoreLabel.RelativePosition_V3.X = pointsBox.RelativePosition_V3.X - {{ innerSpacing * 2.0 }};
                    pointsBoxOuterFrame.Show();
                    pointsBoxFrame.Show();
                }else{
                    pointsLabel.Value = "";
                    scoreLabel.RelativePosition_V3.X = {{ w - padding - innerSpacing * 2.0 }};
                    pointsBoxOuterFrame.Hide();
                    pointsBoxFrame.Hide();
                }
                
                playerRow.Show();
                
                if(cursor == 0){
                    //for finalist mode
                    setOuterPointsColor(pointsBoxOuterFrame, <0.3, 0.8, 0.3>);
                }
                
                cursor += 1;
            }
            
            for(i, cursor, {{ MaxPlayers - 1 }}){
                declare playerRow = (rowsFrame.Controls[i] as CMlFrame);
                //declare positionLabel = (playerRow.GetFirstChild("position") as CMlLabel);
                //declare nameLabel = (playerRow.GetFirstChild("name") as CMlLabel);
                //positionLabel.Value = (i + 1) ^ "";
                //nameLabel.Value = "empty_" ^ i;
                playerRow.Hide();
            }
            
            declare mapNameLabel <=> (Page.MainFrame.GetFirstChild("map_name") as CMlLabel);
            declare authorName <=> (Page.MainFrame.GetFirstChild("author_name") as CMlLabel);
            declare subTextLabel <=> (Page.MainFrame.GetFirstChild("sub_text") as CMlLabel);
            declare roundLabel <=> (Page.MainFrame.GetFirstChild("round_label") as CMlLabel);
            
            mapNameLabel.Value = Map.MapName;
            authorName.Value = Map.AuthorNickName;
            subTextLabel.Value = "Author time: " ^ TL::TimeToText(Map.TMObjective_AuthorTime, True, True);
            
            if(isPointsBased){
                roundLabel.Value = "ROUND 0/0";
            }else{
                roundLabel.Value = CurrentServerModeName;
            }
            
            //TODO: update scroll size
            declare scrollFrame <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            declare scrollN = 0;
            if(cursor >= 8){
                scrollN = cursor - 8;
            }
            scrollFrame.ScrollMax = <0.0, {{ rowHeight + rowSpacing }} * scrollN * 1.0>;
        }
        -->
    </script>

    <script>
        <!--
        *** OnInitialization ***
        ***
            declare Integer scoreboardUpdateInterval = 500;
            declare Integer lastScoreboardUpdate = 0;
            declare scrollFrame <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            
            scrollFrame.ScrollActive = True;
            scrollFrame.ScrollGridSnap = True;
            scrollFrame.ScrollMin = <0.0, 0.0>;
            scrollFrame.ScrollMax = <0.0, {{ MaxPlayers * (rowHeight + rowSpacing) - h }} * 1.0>;
            scrollFrame.ScrollGrid = <0.0, {{ rowHeight + rowSpacing }} * 1.0>;
        ***
        
        *** OnLoop *** 
        ***
            if(lastScoreboardUpdate + scoreboardUpdateInterval < Now){
                updateScoreTable();
                lastScoreboardUpdate = Now;
            }
        ***
        -->
    </script>

    <script resource="EvoSC.Scripts.UIScripts"/>
</component>
