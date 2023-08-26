﻿<component>
    <using namespace="System.Linq"/>

    <import component="Scoreboard.Components.BackgroundBox" as="ScoreboardBackground"/>
    <import component="Scoreboard.Components.ScoreboardHeader" as="ScoreboardHeader"/>
    <import component="Scoreboard.Components.PlayerRow.Framemodel" as="PlayerRowFramemodel"/>

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

    <property type="string" name="headerColor" default="111111"/>
    <property type="string" name="backgroundColor" default="222222"/>
    <property type="string" name="primaryColor" default="bb0755"/>

    <template layer="ScoresTable">
        <PlayerRowFramemodel headerColor="{{ headerColor }}"
                             primaryColor="{{ primaryColor }}"
                             w="{{ w }}"
                             padding="{{ padding }}"
                             rowHeight="{{ rowHeight }}"
                             rowSpacing="{{ rowSpacing }}"
                             innerSpacing="{{ innerSpacing }}"
                             rowInnerHeight="{{ rowInnerHeight }}"
                             pointsWidth="{{ pointsWidth }}"
        />

        <frame pos="{{ w / -2.0 }} {{ h / 2.0 + 10.0 }}">
            <ScoreboardBackground w="{{ w }}" h="{{ h }}"
                                  radius="{{ backgroundBorderRadius }}"
                                  headerHeight="{{ headerHeight }}"
                                  headerColor="{{ headerColor }}"
                                  color="{{ backgroundColor }}"
                                  gradientColor="{{ primaryColor }}"
            />

            <ScoreboardHeader w="{{ w }}" primaryColor="{{ primaryColor }}"/>

            <!-- Player Rows -->
            <frame id="rows_wrapper" pos="0 {{ -headerHeight - padding }}" size="{{ w }} {{ h - padding }}">
                <frame id="rows_inner">
                    <SettingsWrapper h="{{ h }}" padding="{{ padding }}">
                        <SettingsForm w="{{ w - padding * 2.0 }}" h="{{ h - padding * 2.0 }}"/>
                    </SettingsWrapper>
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
            declare persistent Boolean TSB_ShowClubTags for LocalUser = True;
            declare persistent Boolean TSB_ShowFlags for LocalUser = True;
            declare shouldShowPoints = ShouldShowPointsBox();
            declare Real flagWidth = {{ rowInnerHeight }} * 2.0;
            declare Real innerSpacing = {{ innerSpacing }} * 1.0;
            
            foreach(Control in RowsFrame.Controls){
                declare Real offset = 0.0;
                declare playerRow = (Control as CMlFrame);
                declare pointsBoxFrame = (playerRow.GetFirstChild("points_box") as CMlFrame);
                declare flagQuad = (playerRow.GetFirstChild("flag") as CMlQuad);
                declare clubQuad = (playerRow.GetFirstChild("club_bg") as CMlQuad);
                declare clubLabel = (playerRow.GetFirstChild("club") as CMlLabel);
                declare nameLabel = (playerRow.GetFirstChild("name") as CMlLabel);
                declare pointsLabel = (playerRow.GetFirstChild("points") as CMlLabel);
                
                pointsBoxFrame.Visible = shouldShowPoints;
                pointsLabel.Visible = shouldShowPoints;
                
                if(TSB_ShowFlags){
                    flagQuad.RelativePosition_V3.X = offset;
                    flagQuad.Show();
                    offset += flagWidth;
                }else{
                    flagQuad.Hide();
                }
                
                if(TSB_ShowClubTags){
                    clubQuad.RelativePosition_V3.X = offset;
                    clubLabel.RelativePosition_V3.X = offset + (flagWidth / 2.0);
                    clubQuad.Show();
                    clubLabel.Show();
                    offset += flagWidth;
                }else{
                    clubQuad.Hide();
                    clubLabel.Hide();
                }
                
                nameLabel.RelativePosition_V3.X = offset + innerSpacing;
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
        
        Void SetCustomLabel(CMlFrame playerRow, Text value, Text hexColor){
            declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
            declare customGradientFrame = (playerRow.GetFirstChild("custom_gradient") as CMlFrame);
            
            customLabel.Value = value;
            customLabel.TextColor = CL::HexToRgb(hexColor);
            
            Page.GetClassChildren("modulate", customGradientFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).ModulateColor = customLabel.TextColor;
            }
            Page.GetClassChildren("set", customGradientFrame, True);
            foreach(Control in Page.GetClassChildren_Result){
                (Control as CMlQuad).BgColor = customLabel.TextColor;
            }
            
            customGradientFrame.Show();
        }
        
        Void HideCustomLabel(CMlFrame playerRow){
            declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
            declare customGradientFrame = (playerRow.GetFirstChild("custom_gradient") as CMlFrame);
            customLabel.Value = "";
            customGradientFrame.Hide();
        }
        
        Void UpdateScoreAndPoints(CSmScore Score, CMlFrame playerRow){
            declare netread Text[][Text] Net_TMxSM_ScoresTable_CustomPoints for Teams[0];
            declare netread Integer[Text] Net_TMxSM_ScoresTable_CustomTimes for Teams[0];
            declare Boolean CustomPointsEnabled = Net_TMxSM_ScoresTable_CustomPoints.existskey(Score.User.WebServicesUserId);
            declare CSmPlayer Driver for Score;
            declare Boolean Race_ScoresTable_IsSpectator for Score = False;
            declare ScoresTable_PlayerLastUpdate for Score = -1;
            declare Boolean PlayerIsConnected = ScoresTable_PlayerLastUpdate == Now;
                
            declare scoreLabel = (playerRow.GetFirstChild("score") as CMlLabel);
            declare specDisconnectedLabel = (playerRow.GetFirstChild("spec_disconnected_label") as CMlLabel);
            declare pointsLabel = (playerRow.GetFirstChild("points") as CMlLabel);
            declare roundPointsLabel = (playerRow.GetFirstChild("round_points") as CMlLabel);
            declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
            
            if (!(CustomPointsEnabled && CurrentScoreMode != C_Mode_Trophy)) {
                HideCustomLabel(playerRow);
            }
            
            if (CustomPointsEnabled && CurrentScoreMode != C_Mode_Trophy) {
                declare CustomLabelColorHex = "000";
                declare CustomLabelValue = "";
                
                if (Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId].existskey(C_CustomPoints_Text)) {
                    CustomLabelValue = Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId][C_CustomPoints_Text];
                }else{
                    CustomLabelValue = "";
                }
                if (Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId].existskey(C_CustomPoints_Color)) {
                    CustomLabelColorHex = Net_TMxSM_ScoresTable_CustomPoints[Score.User.WebServicesUserId][C_CustomPoints_Color];
                }
            
                if(CustomLabelValue != ""){
                    SetCustomLabel(playerRow, CustomLabelValue, CustomLabelColorHex);
                }else{
                    HideCustomLabel(playerRow);
                }
            } else if (CurrentScoreMode == C_Mode_Points) {
                //SetRacePoints(Frame_RoundPoints, Score.RoundPoints, IsLocalPlayer);
                customLabel.Value = "";
                pointsLabel.Value = TL::ToText(Score.Points);
                
                if(Score.PrevRaceTimes.count > 0 && Score.PrevRaceTimes[Score.PrevRaceTimes.count - 1] > 0){
                    scoreLabel.Value = TL::TimeToText(Score.PrevRaceTimes[Score.PrevRaceTimes.count - 1], True, True);
                }else{
                    if(Driver.SpawnStatus == CSmPlayer::ESpawnStatus::NotSpawned && PlayerIsConnected && !Race_ScoresTable_IsSpectator){
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
            
            scoreLabel.Value = StripLeadingZeroes(scoreLabel.Value);
            
            if (PlayerIsConnected) {
                //connected
                if(Race_ScoresTable_IsSpectator){
                    specDisconnectedLabel.Value = "";
                }else{
                    specDisconnectedLabel.Value = "";
                }
            }else{
                //disconnected
                specDisconnectedLabel.Value = "";
            }
            
            //align items
            declare offset = 0.0;
            declare x = scoreLabel.RelativePosition_V3.X;
            
            if(scoreLabel.Value != ""){
                offset += scoreLabel.ComputeWidth(scoreLabel.Value) + {{ innerSpacing }};
            }
            customLabel.RelativePosition_V3.X = x - offset;
            if(customLabel.Value != ""){
                offset += customLabel.ComputeWidth(customLabel.Value) + {{ innerSpacing }};
            }
            roundPointsLabel.RelativePosition_V3.X = x - offset;
            if(roundPointsLabel.Value != ""){
                offset += roundPointsLabel.ComputeWidth(roundPointsLabel.Value) + {{ innerSpacing }};
            }
            specDisconnectedLabel.RelativePosition_V3.X = x - offset;
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
            foreach (PlayerIndex => Player in Players) {
                if (Player.Score == Null) continue;
                
                declare ScoresTable_PlayerLastUpdate for Player.Score = -1;
                ScoresTable_PlayerLastUpdate = Now;
                
                declare Boolean Race_ScoresTable_IsSpectator for Player.Score = False;
                Race_ScoresTable_IsSpectator = Player.RequestsSpectate;
                
                declare CSmPlayer Driver for Player.Score;
                Driver <=> Player;
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
                declare scoreLabel = (playerRow.GetFirstChild("score") as CMlLabel);
                declare scoreTwoLabel = (playerRow.GetFirstChild("score_two") as CMlLabel);                
                declare pointsBoxFrame = (playerRow.GetFirstChild("points_box") as CMlFrame);
                
                positionLabel.Value = (cursor + 1) ^ "";
                clubLabel.Value = User.ClubTag;
                nameLabel.Value = User.Name;
                
                if(clubLabel.Value != ""){
                    clubBg.Opacity = 1.0;
                }else{
                    clubBg.Opacity = 0.5;
                }
                
                declare Boolean CustomLabelVisible for playerRow = False;
                
                UpdateScoreAndPoints(Score, playerRow);
                SetCountryFlag(flagQuad, User.Login);
                
                if(ShouldShowPointsBox()){
                    scoreLabel.RelativePosition_V3.X = pointsBoxFrame.RelativePosition_V3.X - {{ innerSpacing * 2.0 }};
                    pointsBoxFrame.Show();
                }else{
                    scoreLabel.RelativePosition_V3.X = {{ w - padding - innerSpacing * 2.0 }};
                    pointsBoxFrame.Hide();
                }
                
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
            declare Integer scoreboardUpdateInterval = 333;
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
