<component>
    <using namespace="EvoSC.Modules.Official.SpectatorTargetInfoModule.Config"/>

    <import component="EvoSC.Advanced.ClubTag" as="ClubTag"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="string" name="backgroundColor"/>
    <property type="string" name="headerColor"/>
    <property type="string" name="primaryColor"/>

    <property type="ISpectatorTargetInfoSettings" name="settings"/>
    <property type="string" name="bgDark" default="1c1d24"/>
    <property type="double" name="h" default="7.0"/>
    <property type="double" name="centerBoxWidth" default="48.0"/>

    <template>
        <UIStyle/>
        <frame id="main_frame" pos="{{ (h*4.4+centerBoxWidth) / -2.0 }} {{ settings.Y }}" hidden="1">
            <Rectangle width="{{ h*1.4 }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_AccentSecondary }}"
                       cornerRadius="0.5"
                       corners='TopLeft'
            />
            <quad pos="{{ h * 1.4 }}"
                  size="{{ h * 3.0 }} {{ h }}"
                  class="bg-primary"
                  opacity="1.0"
            />
            <Rectangle x="{{ h*4.4  }}"
                       width="{{ centerBoxWidth }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_BgPrimary }}ee"
                       cornerRadius="0.5"
                       corners='TopRight'
            />

            <label id="position_label"
                   class="text-primary"
                   pos="{{ (h*1.4) / 2.0 }} {{ h / -2.0 }}"
                   size="{{ h * 0.8 }} {{ h * 0.8 }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textcolor="{{ Theme.Black }}"
                   text="-1"
                   halign="center"
                   valign="center2"
            />
            <label id="diff_label"
                   pos="{{ h*2.9 }} {{ h / -2.0 }}"
                   size="{{ (h*3.4)*0.8 }} {{ h }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textfont="{{ Font.Regular }}"
                   text="-1"
                   halign="center"
                   valign="center2"
            />

            <frame id="name_box" pos="{{ h*4.4 + 2.0 }} {{ h / -2.0 }}">
                <ClubTag h="{{ h / 2.0 }}"
                         hidden="{{ true }}"
                />
                <label id="name_label"
                       class="text-primary"
                       pos="8.0 0"
                       size="{{ centerBoxWidth - 10.0 }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       valign="center2"
                />
            </frame>

            <Rectangle y="{{ -h }}"
                       width="{{ h*4.4+centerBoxWidth }}"
                       height="0.75"
                       bgColor="{{ Theme.UI_AccentPrimary }}99"
                       cornerRadius="0.5"
                       corners='BottomRight,BottomLeft'
            />
        </frame>
    </template>

    <script><!--
    #Include "MathLib" as ML
    #Include "TextLib" as TL
    
    Text StripLeadingZeroes(Text input) {
        return TL::RegexReplace("^[0:]+", input, "", "");
    }
    
    CSmPlayer GetFastestPlayer(Integer cpIndex){
        declare CSmPlayer fastestPlayerAtWaypoint;
        declare Integer fastestWaypointTime = -1;
        
        if(cpIndex == -1){
            return fastestPlayerAtWaypoint;
        }
        
        foreach(player in Players){
            if(player.LapWaypointTimes.count <= cpIndex) continue;
            declare playerWaypointTime = player.LapWaypointTimes[cpIndex];
            if(fastestWaypointTime == -1 || playerWaypointTime < fastestWaypointTime){
                fastestWaypointTime = playerWaypointTime;
                fastestPlayerAtWaypoint <=> player;
            }
        }
        
        return fastestPlayerAtWaypoint;
    }
    
    Integer GetPlayerCheckpointIndex(CSmPlayer player){
        return player.LapWaypointTimes.count - 1;
    }
    
    Integer GetPlayerWaypointTime(CSmPlayer player, Integer cpIndex){
        if(cpIndex == -1 || player.LapWaypointTimes.count <= cpIndex){
            return 0;
        }
        
        return player.LapWaypointTimes[cpIndex];
    }
    
    Integer GetTimeDifference(CSmPlayer spectatorTarget, CSmPlayer leadingPlayer){
        if(spectatorTarget == leadingPlayer){
            return 0;
        }
        
        declare cpIndexTarget = GetPlayerCheckpointIndex(spectatorTarget);
        declare spectatorTargetTime = GetPlayerWaypointTime(spectatorTarget, cpIndexTarget);
        declare leadingPlayerTime = GetPlayerWaypointTime(leadingPlayer, cpIndexTarget);
        
        return leadingPlayerTime - spectatorTargetTime;
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
        
        declare Integer EvoCheckpointTimesUpdate for UI;
        declare Boolean EvoCheckpointTimesReset for UI = False;
        declare Integer lastCheckpointUpdateCheck = 0;
        declare Integer[Text] ranksByAccountId;
        declare Integer[Text] diffs;
        
        while(True) {
            yield;
            
            if(EvoCheckpointTimesReset){
                ranksByAccountId = Integer[Text];
                EvoCheckpointTimesReset = False;
            }
            
            if(GUIPlayer == Null){
                sleep(100);
                if(mainFrame.Visible){
                    mainFrame.Hide();
                }
                continue;
            }
            
            declare CSmPlayer spectatorTarget <=> GUIPlayer;
            declare cpIndexTarget = GetPlayerCheckpointIndex(spectatorTarget);
            declare CSmPlayer leadingPlayer <=> GetFastestPlayer(cpIndexTarget);
            declare Integer timeDifference = GetTimeDifference(spectatorTarget, leadingPlayer);
            
            if(leadingPlayer != Null){
                log("Speccing: " ^ spectatorTarget.User.Name);
                log("Leading: " ^ leadingPlayer.User.Name);
                log("[CP#] " ^ (cpIndexTarget+1) ^ " [DIFF] " ^ timeDifference);
            }else{
                log("Nobody is leading.");
                continue;
            }
            
            if(!mainFrame.Visible){
                mainFrame.Show();
            }
            
            nameLabel.Value = spectatorTarget.User.Name;
            diffLabel.Value = "+" ^ StripLeadingZeroes(TL::TimeToText(timeDifference, True, True));
            positionLabel.Value = "-1";
            
            if(spectatorTarget.User.ClubTag != ""){
                clubTagLabel.Value = spectatorTarget.User.ClubTag;
                //nameLabel.RelativePosition_V3.X = clubTagBgQuad.Size.X + 1.0;
                //nameBoxFrame.RelativePosition_V3.X = (clubTagBgQuad.Size.X + 1.0 + nameLabel.ComputeWidth(nameLabel.Value)) / -2.0;
                clubTagBgQuad.Show();
                clubTagLabel.Show();
            }else{
                //nameLabel.RelativePosition_V3.X = 0.0;
                //nameBoxFrame.RelativePosition_V3.X = nameLabel.ComputeWidth(nameLabel.Value) / -2.0;
                clubTagBgQuad.Hide();
                clubTagLabel.Hide();
            }
            
            /*
            if(lastCheckpointUpdateCheck != EvoCheckpointTimesUpdate){
                lastCheckpointUpdateCheck = EvoCheckpointTimesUpdate;
                ranksByAccountId = GetRanksForAccountIds(EvoCheckpointTimes);
                diffs = CalculatePlayerDiffs(ranksByAccountId);
            }
            */
        }
    }
    --></script>
</component>