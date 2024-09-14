<component>
    <using namespace="EvoSC.Modules.Official.SpectatorTargetInfoModule.Config"/>

    <import component="EvoSC.Advanced.ClubTag" as="ClubTag"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="ISpectatorTargetInfoSettings" name="settings"/>
    <property type="int" name="timeDifference" default="0"/>
    <property type="int" name="playerRank" default="0"/>
    <property type="int" name="playerTeam" default="-1"/>
    <property type="string" name="playerName" default="unknown"/>
    <property type="string" name="teamColorCode" default="000"/>

    <property type="double" name="centerBoxWidth" default="50.0"/>
    <property type="double" name="h" default="6.0"/>

    <template>
        <UIStyle/>
        <frame id="main_frame" pos="{{ (h*4.4+centerBoxWidth) / -2.0 }} {{ settings.Y }}">
            <Rectangle width="{{ h*1.6 }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_AccentSecondary }}cc"
                       cornerRadius="0.5"
                       corners='TopLeft'
            />
            <quad pos="{{ h * 1.6 }}"
                  size="{{ h * 3.0 }} {{ h }}"
                  bgcolor="{{ teamColorCode }}"
                  opacity="0.8"
            />
            <Rectangle x="{{ h*4.6  }}"
                       width="{{ centerBoxWidth }}"
                       height="{{ h }}"
                       bgColor="{{ Theme.UI_BgPrimary }}cc"
                       cornerRadius="0.5"
                       corners='TopRight'
            />

            <label id="position_label"
                   class="text-primary"
                   pos="{{ (h*1.6) / 2.0 }} {{ h / -2.0 }}"
                   size="{{ h * 0.8 }} {{ h * 0.8 }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textcolor="{{ Theme.Black }}"
                   text='{{ true ? Icons.Flag : playerRank }}'
                   halign="center"
                   valign="center2"
            />
            <label id="diff_label"
                   pos="{{ h*1.6+h*1.5 }} {{ h / -2.0 }}"
                   size="{{ (h*3.4)*0.8 }} {{ h }}"
                   textsize="{{ Theme.UI_FontSize*2 }}"
                   textfont="{{ Font.Regular }}"
                   text='{{ timeDifference > 0 ? RaceTime.FromMilliseconds(timeDifference) : "000" }}'
                   textprefix="+"
                   halign="center"
                   valign="center2"
            />

            <frame id="name_box" pos="{{ h*4.6 }} {{ h / -2.0 }}">
                <label id="left_button"
                       class="text-primary"
                       pos="4 -0.25"
                       size="{{ h }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ Icons.ArrowCircleLeft }}"
                       valign="center2"
                       halign="center"
                       opacity="0.5"
                       scriptevents="1"
                       focusareacolor1="0000"
                       focusareacolor2="0000"
                />
                <label id="name_label"
                       class="text-primary"
                       pos="{{ centerBoxWidth/2.0 }} 0"
                       size="{{ centerBoxWidth - 14.0 }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ playerName }}"
                       valign="center2"
                       halign="center"
                />
                <label id="right_button"
                       class="text-primary"
                       pos="{{ centerBoxWidth - 4 }} -0.25"
                       size="{{ h }} {{ h }}"
                       textsize="{{ Theme.UI_FontSize*2 }}"
                       text="{{ Icons.ArrowCircleRight }}"
                       valign="center2"
                       halign="center"
                       opacity="0.5"
                       scriptevents="1"
                       focusareacolor1="0000"
                       focusareacolor2="0000"
                />
            </frame>

            <Rectangle y="{{ -h }}"
                       width="{{ h*4.6+centerBoxWidth }}"
                       height="0.75"
                       bgColor="{{ teamColorCode }}"
                       cornerRadius="0.5"
                       corners='BottomRight,BottomLeft'
            />
        </frame>
    </template>

    <script><!--
    #Include "Libs/Nadeo/CMGame/Utils/Icons.Script.txt" as Icons
    #Include "TextLib" as TextLib

    CSmPlayer GetNextSpawnedPlayer() {
        if (GUIPlayer == Null) return Null;
    
        declare CSmPlayer TargetPlayer;
        declare Boolean CanSelectNextPlayer = False;
    
        foreach (Player in Players) {
            if (Player == GUIPlayer) {
                CanSelectNextPlayer = True;
            } else if (
                Player.SpawnStatus != CSmPlayer::ESpawnStatus::NotSpawned &&
                Player.User != Null
            ) {
                if (CanSelectNextPlayer) {
                    TargetPlayer <=> Player;
                    break;
                } else if (TargetPlayer == Null) {
                    TargetPlayer <=> Player;
                }
            }
        }
        return TargetPlayer;
    }
    
    CSmPlayer GetPrevSpawnedPlayer() {
        if (GUIPlayer == Null) return Null;
    
        declare CSmPlayer TargetPlayer;
        declare Boolean CanSelectPrevPlayer = False;
    
        // Read the Players array in the reverse order to fin the previous spawned player
        for (I, 0, Players.count-1, -1) {
            declare CSmPlayer Player <=> Players[I];
            if (Player == GUIPlayer) {
                CanSelectPrevPlayer = True;
            } else if (
                Player.SpawnStatus != CSmPlayer::ESpawnStatus::NotSpawned &&
                Player.User != Null
            ) {
                if (CanSelectPrevPlayer) {
                    TargetPlayer <=> Player;
                    break;
                } else if (TargetPlayer == Null) {
                    TargetPlayer <=> Player;
                }
            }
        }
    
        return TargetPlayer;
    }

    Void AnimateOpacity(CMlLabel button, Real targetOpacity){
        AnimMgr.Flush(button);
        AnimMgr.Add(button, "<anim opacity=\"" ^ targetOpacity ^ "\"/>", 200, CAnimManager::EAnimManagerEasing::ExpOut);
    }
    
    Void AnimatePop(CMlLabel button){
        AnimMgr.Flush(button);
        AnimMgr.Add(button, "<anim scale=\"1.2\" opacity=\"1.0\"/>", 200, CAnimManager::EAnimManagerEasing::ExpOut);
        AnimMgr.AddChain(button, "<anim scale=\"1.0\" opacity=\"0.5\"/>", 200, CAnimManager::EAnimManagerEasing::ExpOut);
    }

    Void FocusPlayer(CSmPlayer _Player) {
        if (_Player != Null && _Player.SpawnStatus != CSmPlayer::ESpawnStatus::NotSpawned && _Player.User != Null) {
            SetSpectateTarget(_Player.User.Login);
        }
    }
    
    Void SpecPrevious(CMlLabel button){
        AnimatePop(button);
        declare CSmPlayer target <=> GetPrevSpawnedPlayer();
        FocusPlayer(target);
    }
    
    Void SpecNext(CMlLabel button){
        AnimatePop(button);
        declare CSmPlayer target <=> GetNextSpawnedPlayer();
        FocusPlayer(target);
    }
    
    main() {
        declare previousButton <=> (Page.MainFrame.GetFirstChild("left_button") as CMlLabel);
        declare nextButton <=> (Page.MainFrame.GetFirstChild("right_button") as CMlLabel);
        
        while(True){
            yield;
            
            if(GUIPlayer == Null){
                continue;
            }
            
			foreach (InputEvent in Input.PendingEvents) {
			    if(InputEvent.Button == CInputEvent::EButton::Left){
			        SpecPrevious(previousButton);
			        continue;
			    }
			    if(InputEvent.Button == CInputEvent::EButton::Right){
			        SpecNext(nextButton);
			        continue;
			    }
			}
			
			foreach(Event in PendingEvents){
			    if(Event.Type == CMlScriptEvent::Type::MouseClick){
			        if(Event.Control == previousButton){
                        SpecPrevious(previousButton);
                        continue;
			        }else if(Event.Control == nextButton){
                        SpecNext(nextButton);
                        continue;
			        }
			    }
			    if(Event.Type == CMlScriptEvent::Type::MouseOver){
			        AnimateOpacity(Event.Control as CMlLabel, 1.0);
			        continue;
			    }
			    if(Event.Type == CMlScriptEvent::Type::MouseOut){
			        AnimateOpacity(Event.Control as CMlLabel, 0.5);
			        continue;
			    }
			}
        }
    }
    
    --></script>
</component>