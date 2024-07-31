<component>
    <import component="EvoSC.Advanced.ClubTag" as="ClubTag"/>
    
    <property type="string" name="backgroundColor"/>
    <property type="string" name="headerColor"/>
    <property type="string" name="primaryColor"/>

    <property type="string" name="bgDark" default="1c1d24"/>
    <property type="double" name="h" default="10.0"/>
    <property type="double" name="y" default="-76.0"/>
    <property type="double" name="centerBoxWidth" default="60.0"/>
    <property type="double" name="smallBoxWidth" default="19.0"/>

    <template>
        <frame id="main_frame" pos="0 {{ y }}" hidden="1">
            <frame id="rounded_box_left" pos="{{ centerBoxWidth / -2.0 - smallBoxWidth }}">
                <frame size="{{ smallBoxWidth }} {{ h }}">
                    <quad pos="0 0.2"
                          size="{{ smallBoxWidth + 2.0 }} {{ h + 0.4 }}"
                          colorize="{{ Theme.SpectatorTargetInfoModule_SpectatorTargetInfo_BgGrad2 }}"
                          style="UICommon64_1"
                          substyle="BgFrame2"
                          opacity="0.9"
                    />
                </frame>
                <!-- GRADIENT -->
                <quad pos="{{ smallBoxWidth }} {{ -h }}"
                      size="{{ smallBoxWidth / 2.0 }} {{ h }}"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="{{ Theme.SpectatorTargetInfoModule_SpectatorTargetInfo_BgGrad1 }}"
                      rot="180"
                      opacity="0.5"
                />
                <label id="position_label"
                       pos="{{ smallBoxWidth / 2.0 }} {{ h / -2.0 }}"
                       size="{{ smallBoxWidth - 6.0 }} {{ h }}"
                       textsize="2.8"
                       textfont="{{ Font.Regular }}"
                       text="-1"
                       halign="center"
                       valign="center2"
                />
            </frame>

            <frame id="rounded_box_right" pos="{{ centerBoxWidth / 2.0 }}">
                <frame size="{{ smallBoxWidth }} {{ h }}">
                    <quad pos="-2 0.2"
                          size="{{ smallBoxWidth + 2.0 }} {{ h + 0.4 }}"
                          colorize="{{ Theme.SpectatorTargetInfoModule_SpectatorTargetInfo_BgGrad2 }}"
                          style="UICommon64_1"
                          substyle="BgFrame2"
                          opacity="0.9"
                    />
                </frame>
                <!-- GRADIENT -->
                <quad pos="0 0"
                      size="{{ smallBoxWidth / 2.0 }} {{ h }}"
                      image="file://Media/Painter/Stencils/15-Stripes/_Stripe0Grad/Brush.tga"
                      modulatecolor="{{ Theme.SpectatorTargetInfoModule_SpectatorTargetInfo_BgGrad1 }}"
                      opacity="0.5"
                />
                <label id="diff_label"
                       pos="{{ smallBoxWidth / 2.0 }} {{ h / -2.0 }}"
                       size="{{ smallBoxWidth - 6.0 }} {{ h }}"
                       textsize="2"
                       textfont="{{ Font.Regular }}"
                       text="-1"
                       halign="center"
                       valign="center2"
                />
            </frame>

            <!-- BOX MIDDLE -->
            <quad size="{{ centerBoxWidth }} {{ h }}"
                  halign="center"
                  bgcolor="{{ Theme.SpectatorTargetInfoModule_SpectatorTargetInfo_Bg }}"
            />
            
            <frame id="name_box" pos="0 {{ h / -2.0 }}">
                <ClubTag h="{{ h / 2.0 }}" 
                         hidden="{{ true }}"
                />
                <label id="name_label"
                       size="{{ centerBoxWidth - 10.0 }} {{ h }}"
                       textsize="2.8"
                       textfont="{{ Font.Regular }}"
                       text="-1"
                       valign="center2"
                />
            </frame>
        </frame>
    </template>
    
    <script><!--
    #Include "MathLib" as ML
    #Include "TextLib" as TL
        
    #Const C_Status_Disconnected	0
    #Const C_Status_Spawned			1
    #Const C_Status_NotSpawned		2
    #Const C_Status_Spectating		3
    
    #Struct EvoSC_CheckpointTime {
        Text AccountId;
        Integer CpIndex;
        Integer Time;
    }
    
    declare Integer FirstPositionTime;
    declare Integer LocalPositionTime;
        
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
    
    Integer GetCurrentCheckpointTime(CSmPlayer player) {
        if(player != Null && player.CurrentLapWaypointTimes.count > 0){
            return player.CurrentLapWaypointTimes[player.CurrentLapWaypointTimes.count - 1];
        }
        
        return 0;
    }
    
    Integer GetPlayerRank(Integer[Text] ranksByAccountId, CSmPlayer player) {
        declare accountId = player.User.WebServicesUserId;
    
        if(ranksByAccountId.existskey(accountId)){
            return ranksByAccountId[accountId];
        }
        
        return -1;
    }
    
    Integer[Text] CalculatePlayerDiffs(Integer[Text] ranksByAccountId) {
        declare Integer[Text] scoresByAccountId;
        declare Integer[Text] diffsByAccountId;
        declare Integer bestTime = -1;
        
        foreach(player in Players){
            scoresByAccountId[player.User.WebServicesUserId] = GetCurrentCheckpointTime(player);
        }
    
        foreach(rank in ranksByAccountId){
            declare accountId = ranksByAccountId.keyof(rank);
            
            if(scoresByAccountId.existskey(accountId)){
                if(bestTime == -1){
                    bestTime = scoresByAccountId[accountId];
                }
                
                diffsByAccountId[accountId] = ML::Abs(scoresByAccountId[accountId] - bestTime);
            }
        }
        
        return diffsByAccountId;
    }
        
    Text StripLeadingZeroes(Text input) {
        return TL::RegexReplace("^[0:]+", input, "", "");
    }
    
    Integer[Text] GetRanksForAccountIds(EvoSC_CheckpointTime[Text] checkpointTimes){
        declare Integer[Text][Integer] timesByCheckpoints;
        
        foreach(cpTime in checkpointTimes){
            declare index = cpTime.CpIndex;
        
            if(!timesByCheckpoints.existskey(index)){
                timesByCheckpoints[index] = Integer[Text];
            }
        
            timesByCheckpoints[index][cpTime.AccountId] = cpTime.Time;
        }
        
        declare Integer[Text] sortedTimes;
        declare Integer rank = 1;
        
        foreach(checkpointsTimes in timesByCheckpoints.sortkeyreverse()){
            foreach(cpTime in checkpointsTimes.sort()){
                declare accountId = checkpointsTimes.keyof(cpTime);
                sortedTimes[accountId] = rank;
                rank += 1;
            }
        }
        
        return sortedTimes;
    }
    
    main() {
        declare mainFrame <=> (Page.MainFrame.GetFirstChild("main_frame") as CMlFrame);
        declare nameBoxFrame <=> (Page.MainFrame.GetFirstChild("name_box") as CMlFrame);
        declare positionLabel <=> (Page.MainFrame.GetFirstChild("position_label") as CMlLabel);
        declare diffLabel <=> (Page.MainFrame.GetFirstChild("diff_label") as CMlLabel);
        declare nameLabel <=> (Page.MainFrame.GetFirstChild("name_label") as CMlLabel);
        declare clubTagBgQuad <=> (Page.MainFrame.GetFirstChild("club_bg") as CMlQuad);
        declare clubTagLabel <=> (Page.MainFrame.GetFirstChild("club") as CMlLabel);
        declare lastAssignUpdate = 0;
        
        declare EvoSC_CheckpointTime[Text] EvoCheckpointTimes for UI;
        declare Integer EvoCheckpointTimesUpdate for UI;
        declare Boolean EvoCheckpointTimesReset for UI = False;
        declare Integer lastCheckpointUpdateCheck = 0;
        declare Integer[Text] ranksByAccountId;
        declare Integer[Text] diffs;
        
        FirstPositionTime = 0;
        LocalPositionTime = 0;
        
        while(True) {
            yield;
            
            if(EvoCheckpointTimesReset){
                ranksByAccountId = Integer[Text];
                EvoCheckpointTimesReset = False;
            }
            
            if(GUIPlayer == Null || GUIPlayer.User == LocalUser){
                sleep(100);
                if(mainFrame.Visible){
                    mainFrame.Hide();
                }
                continue;
            }
            
            if(lastCheckpointUpdateCheck != EvoCheckpointTimesUpdate){
                lastCheckpointUpdateCheck = EvoCheckpointTimesUpdate;
                ranksByAccountId = GetRanksForAccountIds(EvoCheckpointTimes);
                diffs = CalculatePlayerDiffs(ranksByAccountId);
            }
            
            if(!mainFrame.Visible){
                mainFrame.Show();
            }
            
            declare Integer timeDifference = 0;
            if(diffs.existskey(GUIPlayer.User.WebServicesUserId)){
                timeDifference = diffs[GUIPlayer.User.WebServicesUserId];
            }
            
            nameLabel.Value = GUIPlayer.User.Name;
            diffLabel.Value = "+" ^ StripLeadingZeroes(TL::TimeToText(timeDifference, True, True));
            positionLabel.Value = GetPlayerRank(ranksByAccountId, GUIPlayer) ^ ".";
            clubTagLabel.Value = GUIPlayer.User.ClubTag;
            
            if(GUIPlayer.User.ClubTag != ""){
                nameLabel.RelativePosition_V3.X = clubTagBgQuad.Size.X + 1.0;
                nameBoxFrame.RelativePosition_V3.X = (clubTagBgQuad.Size.X + 1.0 + nameLabel.ComputeWidth(nameLabel.Value)) / -2.0;
                clubTagBgQuad.Show();
                clubTagLabel.Show();
            }else{
                nameLabel.RelativePosition_V3.X = 0.0;
                nameBoxFrame.RelativePosition_V3.X = nameLabel.ComputeWidth(nameLabel.Value) / -2.0;
                clubTagBgQuad.Hide();
                clubTagLabel.Hide();
            }
        }
    }
    --></script>
</component>