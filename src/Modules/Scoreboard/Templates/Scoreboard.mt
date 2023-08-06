<component>
    <using namespace="System.Linq"/>

    <import component="Scoreboard.Components.BackgroundBox" as="Background"/>

    <property type="int" name="MaxPlayers" default="0"/>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="70"/>
    <property type="double" name="backgroundBorderRadius" default="3.0"/>
    <property type="double" name="rowHeight" default="8.0"/>
    <property type="double" name="rowSpacing" default="0.5"/>

    <template>
        <framemodel id="player_row">
            <quad size="{{ w }} {{ rowHeight + rowSpacing }}" ScriptEvents="1"/>
            <quad size="{{ w - 6.0 }} {{ rowHeight }}" pos="3.0 0" bgcolor="0c0f31" opacity="0.93"/>
            <quad size="{{ rowHeight }} {{ rowHeight }}" pos="3.0 0" bgcolor="2a37a9"/>
            <frame pos="3.0 {{ rowHeight / -2.0 }}">
                <label id="position" pos="{{ rowHeight / 2.0 }} 0" valign="center" halign="center" textsize="2.6" textfont="GameFontBlack"/>
                <label id="club" pos="10 0" valign="center" textsize="2.6" opacity="0.75" textfont="GameFontSemiBold"/>
                <label id="name" pos="10 0" valign="center" textsize="2.6" textfont="GameFontSemiBold"/>
                <label id="points" pos="{{ w - 9.0 }} 0" valign="center" halign="right" textsize="2.4" textfont="GameFontRegular"/>
                <label id="score" pos="{{ w - 9.0 }} 0" valign="center" halign="right" textsize="2.4" textfont="GameFontRegular"/>
            </frame>
        </framemodel>

        <frame pos="{{ w / -2.0 }} {{ h / 2.0 + 10.0 }}">
            <Background w="{{ w }}" h="{{ h }}" radius="{{ backgroundBorderRadius }}" headerHeight="16.0"/>

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

            <label id="map_name" pos="5 -6" text="MAP NAME" valign="center" textsize="2.6" textfont="GameFontSemiBold"/>
            <label id="author_name" pos="5 -10" text="AUTHOR NAME" valign="center" textsize="1.1" textfont="GameFontRegular"/>

            <label id="sub_text" pos="{{ w - 3.0 }} -11" text="" valign="center" halign="right" textsize="0.8" opacity="0.8" textfont="GameFontRegular"/>

            <frame id="frame_scroll" pos="0 {{ -19.0 }}" size="{{ w }} {{ h - backgroundBorderRadius }}">
                <frameinstance modelid="player_row"
                               foreach="int rowId in Enumerable.Range(0, MaxPlayers).ToList()"
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
        
        Void updateScoreTable() {
            declare rowsFrame <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            declare cursor = 0;
            
            foreach(Score => Weight in GetSortedScores()){
                declare CUser User <=> Score.User;
                declare playerRow = (rowsFrame.Controls[cursor] as CMlFrame);
                declare positionLabel = (playerRow.GetFirstChild("position") as CMlLabel);
                declare clubLabel = (playerRow.GetFirstChild("club") as CMlLabel);
                declare nameLabel = (playerRow.GetFirstChild("name") as CMlLabel);
                declare pointsLabel = (playerRow.GetFirstChild("points") as CMlLabel);
                declare scoreLabel = (playerRow.GetFirstChild("score") as CMlLabel);
                
                positionLabel.Value = (cursor + 1) ^ "";
                clubLabel.Value = "$i$<" ^ User.ClubTag ^ "$>";
                nameLabel.Value = User.Name;
                nameLabel.RelativePosition_V3.X = clubLabel.RelativePosition_V3.X + clubLabel.ComputeWidth(clubLabel.Value) + 1.0;
                //pointsLabel.Value = Score.Points ^ " Points";
                
                if(Score.BestLapTimes.count > 0 && Score.BestLapTimes[Score.BestLapTimes.count - 1] > 0){
                    scoreLabel.Value = TL::TimeToText(Score.BestLapTimes[Score.BestLapTimes.count - 1], True, True);
                }else{
                    scoreLabel.Value = "-";
                }
                
                playerRow.Show();
                
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
            
            mapNameLabel.Value = Map.MapName;
            authorName.Value = Map.AuthorNickName;
            subTextLabel.Value = "Author time: " ^ TL::TimeToText(Map.TMObjective_AuthorTime, True, True);
            
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
