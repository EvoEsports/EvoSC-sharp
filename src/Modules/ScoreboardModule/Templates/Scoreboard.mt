<component>
    <using namespace="EvoSC.Modules.Official.ScoreboardModule.Config"/>
    <using namespace="System.Linq"/>

    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="ScoreboardModule.Components.ScoreboardHeader" as="Header"/>
    <import component="ScoreboardModule.Components.ScoreboardBody" as="Body"/>
    <import component="ScoreboardModule.Components.ScoreboardBg" as="ScoreboardBg"/>
    <import component="ScoreboardModule.Components.Row.Framemodel" as="PlayerRowFramemodel"/>

    <property type="IScoreboardSettings" name="settings"/>
    <property type="int" name="MaxPlayers" default="0"/>
    <property type="int" name="PointsLimit" default="0"/>
    <property type="int" name="RoundsPerMap" default="0"/>

    <property type="double" name="backgroundBorderRadius" default="3.0"/>
    <property type="double" name="headerHeight" default="14.0"/>
    <property type="double" name="rowHeight" default="8.0"/>
    <property type="double" name="rowInnerHeight" default="5.0"/>
    <property type="double" name="rowSpacing" default="0.3"/>
    <property type="double" name="columnSpacing" default="4.0"/>
    <property type="double" name="pointsWidth" default="16.0"/>
    <property type="double" name="padding" default="2.0"/>
    <property type="double" name="innerSpacing" default="1.6"/>
    <property type="double" name="legendHeight" default="3.8"/>
    <property type="int" name="actionButtonCount" default="2"/>

    <template layer="ScoresTable">
        <!-- UI Styles -->
        <UIStyle/>

        <!-- Frame Models -->
        <PlayerRowFramemodel w="{{ settings.Width }}"
                             padding="{{ padding }}"
                             rowHeight="{{ rowHeight }}"
                             rowSpacing="{{ rowSpacing }}"
                             columnSpacing="{{ columnSpacing }}"
                             innerSpacing="{{ innerSpacing }}"
                             rowInnerHeight="{{ rowInnerHeight }}"
                             pointsWidth="{{ pointsWidth }}"
                             actionButtonCount="{{ actionButtonCount }}"
                             settings="{{ settings }}"
        />

        <!-- Scoreboard Content -->
        <frame pos="{{ settings.Width / -2.0 }} {{ settings.Height / 2.0 }}">
            <!-- Background -->
            <ScoreboardBg width="{{ settings.Width }}"
                          height="{{ settings.Height }}"
            />

            <!-- Header -->
            <Header width="{{ settings.Width }}"
                    height="{{ headerHeight }}"
                    maxPlayers="{{ MaxPlayers }}"
                    pointsLimit="{{ PointsLimit }}"
                    roundsPerMap="{{ RoundsPerMap }}"
            />

            <!-- Body -->
            <Body y="{{ -headerHeight }}"
                  width="{{ settings.Width }}"
                  height="{{ settings.Height - headerHeight }}"
                  legendHeight="{{ legendHeight }}"
                  rowSpacing="{{ rowSpacing }}"
                  columnSpacing="{{ columnSpacing }}"
                  flagWidth="{{ rowInnerHeight * 1.5 }}"
                  clubTagWidth="{{ rowInnerHeight * 1.5 }}"
            />
            <frame id="rows_wrapper"
                   pos="0 {{ -headerHeight-legendHeight }}"
                   size="{{ settings.Width }} {{ settings.Height-headerHeight }}"
            >
                <frame id="rows_inner">
                    <frame id="frame_scroll"
                           size="{{ settings.Width }} {{ settings.Height-headerHeight-legendHeight - 0.1 }}"
                    >
                        <frameinstance modelid="player_row"
                                       foreach="int rowId in Enumerable.Range(0, MaxPlayers * 2).ToList()"
                                       pos="0 {{ rowId * -rowHeight + (rowId+1) * -rowSpacing }}"
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
        #Include "Libs/Nadeo/TMGame/Modes/MV_Utils.Script.txt" as MV_Utils
        #Include "Libs/Nadeo/TMGame/Modes/Base/UIModules/ScoresTable_Common.Script.txt" as UIModules_ScoresTable
        #Include "Libs/Nadeo/TMGame/Modes/Base/UIModules/Helpers_Client.Script.txt" as RaceHelpers
    
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
        declare Integer PlayerRowsVisible;
        declare Integer PlayerRowsFilled;
        declare Integer ScrollIndex;
        declare Integer MaxPlayers;
        declare CMlFrame RowsFrame;
        declare Text[Integer] PositionColors;
        
        Boolean ShouldShowPointsBox() {
            return CurrentScoreMode == C_Mode_LapTime
                || CurrentScoreMode == C_Mode_Laps
                || CurrentScoreMode == C_Mode_Points;
        }
        
        Void UpdateScoreboardLayout() {
            UpdateLegend(ShouldShowPointsBox());
        }
        
        Text StripLeadingZeroes(Text timeString) {
            return TL::RegexReplace("^[0.:]+", timeString, "", "");
        }
        
        Text StyleTime(Text timeString) {
            declare mutedTextColor = "{{ Color.ToTextColor(Theme.UI_TextMuted) }}";
            declare primaryTextColor = "{{ Color.ToTextColor(Theme.ScoreboardModule_Text_Color) }}";
        
            if(timeString == "0:00.000") {
                return mutedTextColor ^ "0" ^ timeString;
            }
        
            declare endPart = StripLeadingZeroes(timeString);
            declare startPart = TL::Replace(timeString, endPart, "");
            
            return mutedTextColor ^ startPart ^ primaryTextColor ^ endPart;
        }
        
        Text StylePoints(Text points, Integer padding) {
            declare mutedTextColor = "{{ Color.ToTextColor(Theme.UI_TextMuted) }}";
            declare primaryTextColor = "{{ Color.ToTextColor(Theme.ScoreboardModule_Text_Color) }}";
            declare out = mutedTextColor;
            
            for(i, 1, padding - TL::Length(points)){
                out ^= "0";
            }
            
            if(points == "0"){
                return out ^ points;
            }
            
            return out ^ primaryTextColor ^ points;
        }
        
        Text GetPlayerBestTimeStyled(CSmScore score) {
            if(score.BestRaceTimes.count == 0){
                return StyleTime("0:00.000");
            }
        
            declare bestTime = score.BestRaceTimes[score.BestRaceTimes.count - 1];
            return StyleTime(TL::TimeToText(bestTime, True, True));
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
        
        Void SetCountryFlag(CMlQuad flagQuad, Text login){
            if(login != "" && !TL::StartsWith("*fakeplayer", login)){
                flagQuad.ImageUrl = "file://ZoneFlags/Login/" ^ login ^ "/country";
                flagQuad.Opacity = 1.0;
            }else{
                flagQuad.ImageUrl = "file://ZoneFlags/World";
                flagQuad.Opacity = 1.0;
            }
        }
        
        Void UpdateScoreAndPoints(CSmScore Score, CMlFrame playerRow, Integer position){
            if(Score == Null || Score.User == Null || playerRow == Null){
                return;
            }
            
            declare netread Text[][Text] Net_TMGame_ScoresTable_CustomPoints for Teams[0];
            declare netread Integer[Text] Net_TMxSM_ScoresTable_CustomTimes for Teams[0];
            declare Boolean CustomPointsEnabled = Net_TMGame_ScoresTable_CustomPoints.existskey(Score.User.WebServicesUserId);
            declare Boolean Race_ScoresTable_IsSpectator for Score = False;
            declare ScoresTable_PlayerLastUpdate for Score = -1;
            declare Boolean PlayerIsConnected = ScoresTable_PlayerLastUpdate == Now;
            declare Boolean colorizePosition = False;
            declare CSmScore playerScore for playerRow;
            playerScore <=> Score;
                
            declare scoreLabel = (playerRow.GetFirstChild("score") as CMlLabel);
            declare bestTimeLabel = (playerRow.GetFirstChild("best_time") as CMlLabel);
            declare specDisconnectedLabel = (playerRow.GetFirstChild("spec_disconnected_label") as CMlLabel);
            declare roundPointsLabel = (playerRow.GetFirstChild("round_points") as CMlLabel);
            declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
            
            scoreLabel.Value = "";
            roundPointsLabel.Value = "";
            
            if (!(CustomPointsEnabled && CurrentScoreMode != C_Mode_Trophy)) {
                HideCustomLabel(playerRow);
            }
            
            if (CustomPointsEnabled && CurrentScoreMode != C_Mode_Trophy) {
                declare CustomLabelColorHex = "000";
                declare CustomLabelValue = "";
                
                if (Net_TMGame_ScoresTable_CustomPoints[Score.User.WebServicesUserId].existskey(C_CustomPoints_Text)) {
                    CustomLabelValue = Net_TMGame_ScoresTable_CustomPoints[Score.User.WebServicesUserId][C_CustomPoints_Text];
                }else{
                    CustomLabelValue = "";
                }
                if (Net_TMGame_ScoresTable_CustomPoints[Score.User.WebServicesUserId].existskey(C_CustomPoints_Color)) {
                    CustomLabelColorHex = Net_TMGame_ScoresTable_CustomPoints[Score.User.WebServicesUserId][C_CustomPoints_Color];
                }
            
                if(CustomLabelValue != ""){
                    SetCustomLabel(playerRow, CustomLabelValue, CustomLabelColorHex);
                }else{
                    HideCustomLabel(playerRow);
                }
            } else if (CurrentScoreMode == C_Mode_Points) {
                customLabel.Value = "";
                scoreLabel.Value = TL::ToText(Score.Points);
                colorizePosition = Score.Points > 0;
                
                if(Score.BestRaceTimes.count > 0 && Score.BestRaceTimes[Score.BestRaceTimes.count - 1] > 0){
                    bestTimeLabel.Value = TL::TimeToText(Score.BestRaceTimes[Score.BestRaceTimes.count - 1], True, True);
                }else{
                    declare CSmPlayer::ESpawnStatus Race_ScoresTable_SpawnStatus for Score = CSmPlayer::ESpawnStatus::NotSpawned;
                    if(Race_ScoresTable_SpawnStatus == CSmPlayer::ESpawnStatus::NotSpawned && PlayerIsConnected && !Race_ScoresTable_IsSpectator){
                        roundPointsLabel.Value = "DNF";
                    }else{
                        roundPointsLabel.Value = "";
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
                    colorizePosition = True;
                } else {
                    scoreLabel.Value = "nope";
                }
            } else if (CurrentScoreMode == C_Mode_BestTime && Score.BestRaceTimes.count > 0) {
                customLabel.Value = "";
                declare bestRaceTime = Score.BestRaceTimes[Score.BestRaceTimes.count - 1];
                scoreLabel.Value = TL::TimeToText(bestRaceTime, True, True);
                colorizePosition = bestRaceTime > 0;
            } else if (CurrentScoreMode == C_Mode_PrevTime && Score.PrevRaceTimes.count > 0) {
                customLabel.Value = "";
                declare bestPrevTime = Score.PrevRaceTimes[Score.PrevRaceTimes.count - 1];
                scoreLabel.Value = TL::TimeToText(bestPrevTime, True, True);
                colorizePosition = bestPrevTime > 0;
            } else if (CurrentScoreMode == C_Mode_LapTime && Score.BestLapTimes.count > 0) {
                customLabel.Value = "";
                scoreLabel.Value = TL::TimeToText(Score.BestLapTimes[Score.BestLapTimes.count - 1], True, True);
            } else if (CurrentScoreMode == C_Mode_Laps && Score.BestRaceTimes.count > 0) {
                customLabel.Value = "";
                scoreLabel.Value = TL::TimeToText(Score.BestRaceTimes[Score.BestRaceTimes.count - 1], True, True);
                scoreLabel.Value = ""^Score.BestRaceTimes.count;
            } else if (CurrentScoreMode == C_Mode_RaceProgression) {
                customLabel.Value = "";
                declare netread Int2 Net_TMxSM_ScoresTable_RaceProgression for Score;
                scoreLabel.Value = ""^Net_TMxSM_ScoresTable_RaceProgression.X;
                if (Net_TMxSM_ScoresTable_RaceProgression.Y > 0) {
                    scoreLabel.Value = TL::TimeToText(Net_TMxSM_ScoresTable_RaceProgression.Y, True, True);
                    colorizePosition = True;
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
            
            if(ShouldShowPointsBox()){
                scoreLabel.Value = StylePoints(scoreLabel.Value, 3);
                bestTimeLabel.Value = GetPlayerBestTimeStyled(Score);
            }else{
                scoreLabel.Value = StyleTime(scoreLabel.Value);
                bestTimeLabel.Value = "";
            }
            
            declare positionBox = (playerRow.GetFirstChild("position_box") as CMlFrame);
            declare playerRowBg = (playerRow.GetFirstChild("player_row_bg") as CMlFrame);
            if({{ Theme.ScoreboardModule_PositionBox_ShowAccent }}){
                if(PositionColors.existskey(position) && colorizePosition){
                    declare positionColor = PositionColors[position];
                    SetPositionBoxColor(positionBox, CL::HexToRgb(positionColor));
                }else{
                    SetPositionBoxColor(positionBox, CL::HexToRgb("{{ Theme.UI_AccentPrimary }}"));
                }
            }
            
            if (PlayerIsConnected) {
                //connected
                if(Race_ScoresTable_IsSpectator){
                    specDisconnectedLabel.Value = "{{ Icons.VideoCamera }}";
                }else{
                    specDisconnectedLabel.Value = "";
                }
            }else{
                //disconnected
                specDisconnectedLabel.Value = "{{ Icons.UserTimes }}";
            }
            
            //align items
            declare offset = 0.0;
            declare x = scoreLabel.RelativePosition_V3.X;
            
            if(scoreLabel.Value != ""){
                offset += scoreLabel.ComputeWidth(scoreLabel.Value) + {{ columnSpacing / 2.0 }};
            }
            customLabel.RelativePosition_V3.X = x - offset;
            if(customLabel.Value != ""){
                offset += customLabel.ComputeWidth(customLabel.Value) + {{ columnSpacing / 2.0 }};
            }
            roundPointsLabel.RelativePosition_V3.X = x - offset;
            if(roundPointsLabel.Value != ""){
                offset += roundPointsLabel.ComputeWidth(roundPointsLabel.Value) + {{ columnSpacing / 2.0 }};
            }
            specDisconnectedLabel.RelativePosition_V3.X = x - offset;
        }
        
        Text GetRecordText() {
            declare Integer SB_PointsLimit for UI = -2;
            
            if(SB_PointsLimit >= 0){
                if(SB_PointsLimit == 0){
                    return "POINTS LIMIT: UNLIMITED";
                }
                
                return "POINTS LIMIT: " ^ SB_PointsLimit;
            }
        
            return "AUTHOR TIME | " ^ TL::TimeToText(Map.TMObjective_AuthorTime, True, True);
        }
        
        Void UpdateScrollSize(Integer playerRowsFilled) {
            declare filledHeight = playerRowsFilled * {{ rowHeight + rowSpacing }};
            declare contentHeight = {{ settings.Height - headerHeight - legendHeight }};
            
            if(filledHeight > contentHeight) {
                RowsFrame.ScrollMax.Y = (filledHeight - contentHeight) * 1.0;
            }else{
                RowsFrame.ScrollMax.Y = 0.0;
            }
        
            PlayerRowsFilled = playerRowsFilled;
        }
        
        Text GetNickname(CUser user) {
            declare Text[Text] EvoSC_Player_Nicknames for UI = [];
            if(EvoSC_Player_Nicknames.existskey(user.Login)){
                return EvoSC_Player_Nicknames[user.Login];
            }
            
            return user.Name;
        }
        
        Void UpdateScoreTable() {
            foreach (PlayerIndex => Player in Players) {
                if (Player.Score == Null) continue;
                
                declare ScoresTable_PlayerLastUpdate for Player.Score = -1;
                ScoresTable_PlayerLastUpdate = Now;
                
                declare Boolean Race_ScoresTable_IsSpectator for Player.Score = False;
                Race_ScoresTable_IsSpectator = Player.RequestsSpectate;
                
                declare CSmPlayer::ESpawnStatus Race_ScoresTable_SpawnStatus for Player.Score = CSmPlayer::ESpawnStatus::NotSpawned;
                Race_ScoresTable_SpawnStatus = Player.SpawnStatus;
            }
            
            declare cursor = 0;
           
            foreach(Score => Weight in GetSortedScores()){
                if(!RowsFrame.Controls.existskey(cursor)){
                    continue;
                }
                
                declare persistent Boolean SB_Setting_ShowSpectators for LocalUser = True;
                declare persistent Boolean SB_Setting_ShowDisconnected for LocalUser = True;
                
                if(!SB_Setting_ShowSpectators){
                    declare Boolean Race_ScoresTable_IsSpectator for Score = False;
                    if(Race_ScoresTable_IsSpectator){
                        continue;
                    }
                }
                if(!SB_Setting_ShowDisconnected){
                    declare ScoresTable_PlayerLastUpdate for Score = -1;
                    declare Boolean PlayerIsConnected = ScoresTable_PlayerLastUpdate == Now;
                    if(!PlayerIsConnected){
                        continue;
                    }
                }
                
                declare playerRow = (RowsFrame.Controls[cursor] as CMlFrame);
                declare clubLabel = (playerRow.GetFirstChild("club") as CMlLabel);
                declare nameLabel = (playerRow.GetFirstChild("name") as CMlLabel);
                declare flagQuad = (playerRow.GetFirstChild("flag") as CMlQuad);
                declare positionBoxFrame = (playerRow.GetFirstChild("position_box") as CMlFrame);
                
                SetPlayerRank(positionBoxFrame, cursor + 1);
                nameLabel.Value = GetNickname(Score.User);
                clubLabel.Value = Score.User.ClubTag;
                if(clubLabel.Value == ""){
                    clubLabel.Value = "-";
                }
                
                declare Boolean CustomLabelVisible for playerRow = False;
                declare Boolean RowIsLocked for playerRow = False;
                
                if(!RowIsLocked){
                    UpdateScoreAndPoints(Score, playerRow, cursor + 1);
                    SetCountryFlag(flagQuad, Score.User.Login);
                }
                
                playerRow.Show();
                
                cursor += 1;
            }
            
            //Hide remaining rows
            for(i, cursor, {{ MaxPlayers - 1 }}){
                if(!RowsFrame.Controls.existskey(i)){
                    continue;
                }
            
                declare playerRow = (RowsFrame.Controls[i] as CMlFrame);
                playerRow.Hide();
            }
            UpdateScrollSize(cursor);
        }
        -->
    </script>

    <script>
        <!--
        *** OnInitialization ***
        ***
            declare netread Integer Net_TMGame_ScoresTable_ScoreMode for Teams[0];
            declare Integer scoreboardUpdateInterval = 333;
            declare Integer lastScoreboardUpdate = 0;
            RowsFrame <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
            
            RowsFrame.ScrollActive = True;
            RowsFrame.DisablePreload = True;
            RowsFrame.ScrollGridSnap = True;
            RowsFrame.ScrollMin = <0.0, 0.0>;
            RowsFrame.ScrollGrid = <0.0, {{ rowHeight + rowSpacing }} * 1.0>;
            
            MaxPlayers = {{ MaxPlayers }};
            PlayerRowsVisible = 0;
            PlayerRowsFilled = -1;
            CurrentScoreMode = -1;
            
            PositionColors = [
                1 => "{{ Theme.Gold }}",
                2 => "{{ Theme.Silver }}",
                3 => "{{ Theme.Bronze }}"
            ];
        ***
        
        *** OnLoop *** 
        ***
            if(lastScoreboardUpdate + scoreboardUpdateInterval < Now){
                ScrollIndex = ML::NearestInteger(RowsFrame.ScrollOffset.Y / {{ rowHeight + rowSpacing }});
                UpdateScoreTable();
                lastScoreboardUpdate = Now;
            }
            
            if(CurrentScoreMode != Net_TMGame_ScoresTable_ScoreMode){
                CurrentScoreMode = Net_TMGame_ScoresTable_ScoreMode;
                UpdateScoreboardLayout();
                log("[EvoSC#] Update scoreboard layout.");
            }
        ***
        
        *** OnScriptExecutionFinished *** 
        ***
            sleep(5000);
            TriggerPageAction("ScoreboardManialinkController/ResendScoreboard");
        ***
        -->
    </script>

    <script resource="EvoSC.Scripts.UIScripts"/>
</component>
