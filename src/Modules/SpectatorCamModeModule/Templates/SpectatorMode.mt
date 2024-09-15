<component>
    <using namespace="EvoSC.Modules.Official.SpectatorCamModeModule.Config"/>
    <using namespace="EvoSC.Common.Util.Manialinks"/>
    
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="ISpectatorCamModeSettings" name="settings"/>
    <property type="double" name="width" default="20.0"/>
    <property type="double" name="height" default="5.5"/>
    <property type="double" name="opacityUnfocused" default="0.1"/>

    <template>
        <UIStyle/>

        <frame pos="{{ settings.X }} {{ settings.Y }}">
            <frame id="cam_mode_frame">
                <quad id="cam_mode_bg"
                      size="{{ width }} {{ height }}"
                      bgcolor="{{ Theme.UI_SurfaceBgPrimary }}"
                      opacity="{{ opacityUnfocused }}"
                      scriptevents="1"
                />
                <label id="cam_mode_label"
                       class="text-primary"
                       pos="2 {{ height / -2.0 - 0.25 }}"
                       textsize="{{ Theme.UI_FontSize*1.5 }}"
                       textfont="{{ Font.Bold }}"
                       text="CAM MODE"
                       valign="center2"
                />
                <quad id="cam_mode_icon"
                      size="3.5 3.5"
                      pos="{{ width - 6.0 }} {{ height / -2.0 }}"
                      valign="center2"
                      colorize="{{ Theme.UI_TextPrimary }}"
                />
            </frame>
        </frame>
    </template>

    <script><!--
    #Include "TextLib" as TextLib
    #Include "ColorLib" as ColorLib
    #Include "Libs/Nadeo/CMGame/Utils/Icons.Script.txt" as Icons
    
    #Const C_CamModes_Replay 0
    #Const C_CamModes_Follow 1
    #Const C_CamModes_FollowAll 2
    #Const C_CamModes_Free 3
    
    declare Boolean G_SelectedFollowAll;
    
    Void SetCamModeNameAndIcon(Integer _CamMode, CMlLabel camModeLabel, CMlQuad camModeQuad) {
        declare camModeFrame <=> (Page.MainFrame.GetFirstChild("cam_mode_frame") as CMlFrame);
        declare camModeBg <=> (Page.MainFrame.GetFirstChild("cam_mode_bg") as CMlQuad);
        declare Text Text_CamMode = "";
        declare Text Icon_CamMode = "";
        
        switch (_CamMode) {
            case C_CamModes_Follow: {
                //L16N Used when spectating a single target, seeing its point of view. In opposition with spectating all players from above "All".
                Text_CamMode = _("|Camera|Follow");
                Icon_CamMode = Icons::C_Icon_128x128_Cam_Follow;
            }
            case C_CamModes_FollowAll: {
                //L16N Used when spectating all players, seeing them from above. In opposition with seeing a single player point of view "Single".
                Text_CamMode = _("|Camera|Follow all");
                Icon_CamMode = Icons::C_Icon_128x128_Cam_All;
            }
            case C_CamModes_Free: {
                Text_CamMode = _("|Camera|Free");
                Icon_CamMode = Icons::C_Icon_128x128_Cam_Free;
            }
            case C_CamModes_Replay: {
                Text_CamMode = _("|Camera|Replay");
                Icon_CamMode = Icons::C_Icon_128x128_Cam_Replay;
            }
        }
        
        declare Real spacing = 1.0;
        declare Real offset = 0.0;
        declare Real width = 0.0;
        if (camModeLabel != Null){
            camModeLabel.Value = TextLib::ToUpperCase(Text_CamMode);
            offset = spacing * 2.0 + camModeLabel.ComputeWidth(camModeLabel.Value);
            width = spacing * 2.0 + camModeLabel.ComputeWidth(camModeLabel.Value);
        }
        if (camModeQuad != Null){
            camModeQuad.ImageUrl = Icon_CamMode;
            camModeQuad.RelativePosition_V3.X = offset + spacing;
            width = width + spacing + camModeQuad.Size.X;
        }
        width = width + spacing * 2.0;
        camModeBg.Size.X = width;
        
        if({{ settings.Alignment == WidgetPosition.Right ? "True" : "False" }}){
            camModeFrame.RelativePosition_V3.X = width * -1.0;
        }else if({{ settings.Alignment == WidgetPosition.Center ? "True" : "False" }}){
            camModeFrame.RelativePosition_V3.X = width / -2.0;
        }
    }

    Integer SetCamMode(Integer _CamMode, CMlLabel _Label_CamMode, CMlQuad _Quad_CamModeImage) {
        /* NB ClientUI.SpectatorForceCameraType:
         * 1: Script has 2 states
         *			Forced -> will act as ForcedTarget specifies
         *			Default -> input can focus a player or allplayers
         * 2: Free camera, player cannot use inputs
         * 15: same as 1 but player can escape in free camera (script totaly loses control)
    
         * After checking C++ with FlorentT it appears that
         * 0 is Replay
         * 1 is Follow
         * 2 is Free
         * 14 is FollowForced
         * 15 is DontChange
    
         * EDIT New API available, see Playground.SetWantedSpectatorCameraType(...)
         */
        declare Integer CamMode = _CamMode;
        ClientUI.Spectator_SetForcedTarget_Clear();
        G_SelectedFollowAll = False;
        if (CamMode == C_CamModes_FollowAll) {
            Playground.SetWantedSpectatorCameraType(CPlaygroundClient::ESpectatorCameraType::Follow); // SetWantedSpectatorCameraType changes "desired" camera, which means player can still change it with inputs (instead of "forced")
            ClientUI.Spectator_SetForcedTarget_AllPlayers();
            G_SelectedFollowAll = True;
        } else if (CamMode == C_CamModes_Follow || CamMode == C_CamModes_Replay) {
            // Check if there are players to follow
            declare Boolean CanFollowPlayer = False;
            foreach (Player in Players) {
                if (Player.SpawnStatus != CSmPlayer::ESpawnStatus::NotSpawned) {
                    CanFollowPlayer = True;
                    break;
                }
            }
            // If there are no players to follow switch to follow all
            if (!CanFollowPlayer) {
                CamMode = SetCamMode(C_CamModes_FollowAll, _Label_CamMode, _Quad_CamModeImage);
            } else {
                if (CamMode == C_CamModes_Follow) {
                    Playground.SetWantedSpectatorCameraType(CPlaygroundClient::ESpectatorCameraType::Follow);
                } else if (CamMode == C_CamModes_Replay) {
                    Playground.SetWantedSpectatorCameraType(CPlaygroundClient::ESpectatorCameraType::Replay);
                }
            }
        } else if (CamMode == C_CamModes_Free) {
            Playground.SetWantedSpectatorCameraType(CPlaygroundClient::ESpectatorCameraType::Free);
        }
        
        SetCamModeNameAndIcon(CamMode, _Label_CamMode, _Quad_CamModeImage);
    
        return CamMode;
    }

    Void AnimateOpacity(CMlQuad quad, Real targetOpacity){
        AnimMgr.Flush(quad);
        AnimMgr.Add(quad, "<anim opacity=\"" ^ targetOpacity ^ "\"/>", 200, CAnimManager::EAnimManagerEasing::ExpOut);
    }

    Void AnimateColorize(CMlQuad icon, Text targetColorHex){
        AnimMgr.Flush(icon);
        AnimMgr.Add(icon, "<anim colorize=\"" ^ targetColorHex ^ "\"/>", 200, CAnimManager::EAnimManagerEasing::ExpOut);
    }
    
    Integer GetAssumedCamMode(Integer camMode){
        declare Integer AssumedCamMode = camMode;
			
        if (Playground.GetSpectatorCameraType() == CPlaygroundClient::ESpectatorCameraType::Free) {
            AssumedCamMode = C_CamModes_Free;
        } else if (
            Playground.GetSpectatorCameraType() == CPlaygroundClient::ESpectatorCameraType::Follow ||
            Playground.GetSpectatorCameraType() == CPlaygroundClient::ESpectatorCameraType::FollowForced
        ) {
            switch (Playground.GetSpectatorTargetType()) {
                case CPlaygroundClient::ESpectatorTargetType::None: {
                    AssumedCamMode = C_CamModes_FollowAll;
                }
                case CPlaygroundClient::ESpectatorTargetType::Single: {
                    AssumedCamMode = C_CamModes_Follow;
                }
                case CPlaygroundClient::ESpectatorTargetType::AllPlayers: {
                    AssumedCamMode = C_CamModes_Follow;
                }
            }
        } else if (Playground.GetSpectatorCameraType() == CPlaygroundClient::ESpectatorCameraType::Replay) {
            AssumedCamMode = C_CamModes_Replay;
            switch (Playground.GetSpectatorTargetType()) {
                case CPlaygroundClient::ESpectatorTargetType::AllPlayers: {
                    AssumedCamMode = C_CamModes_FollowAll;
                }
            }
        }
        
        return AssumedCamMode;
    }
    
    main(){
        declare camModeFrame <=> (Page.MainFrame.GetFirstChild("cam_mode_frame") as CMlFrame);
        declare camModeBg <=> (Page.MainFrame.GetFirstChild("cam_mode_bg") as CMlQuad);
        declare camModeLabel <=> (Page.MainFrame.GetFirstChild("cam_mode_label") as CMlLabel);
        declare camModeIcon <=> (Page.MainFrame.GetFirstChild("cam_mode_icon") as CMlQuad);
    
        declare netread Integer Net_Race_SpectatorBase_CamMode for UI;
        declare netread Integer Net_Race_SpectatorBase_CamUpdate for UI;
        
        declare lastCamUpdate = Net_Race_SpectatorBase_CamUpdate;
        declare camMode = SetCamMode(Net_Race_SpectatorBase_CamMode, camModeLabel, camModeIcon);
        
        while(True){
            yield;
			
			declare shouldBeDisplayed = InputPlayer != Null && InputPlayer.SpawnStatus == CSmPlayer::ESpawnStatus::NotSpawned;
			
			if(!shouldBeDisplayed){
			    if(camModeFrame.Visible){
			        camModeFrame.Hide();
			    }
			    sleep(500);
			    continue;
			}
			
            if(!camModeFrame.Visible){
                camModeFrame.Show();
            }
			
			declare Integer AssumedCamMode = GetAssumedCamMode(camMode);
			
			if (camMode != AssumedCamMode) {
				camMode = AssumedCamMode;
				SetCamModeNameAndIcon(camMode, camModeLabel, camModeIcon);
			}
			
			if(lastCamUpdate != Net_Race_SpectatorBase_CamUpdate){
                camMode = SetCamMode(Net_Race_SpectatorBase_CamMode, camModeLabel, camModeIcon);
			    lastCamUpdate = Net_Race_SpectatorBase_CamUpdate;
			}
            
			foreach(Event in PendingEvents){
			    if(Event.Control != camModeBg) continue;
			    if(Event.Type == CMlScriptEvent::Type::MouseClick){
                    camMode = SetCamMode((camMode + 1) % 4, camModeLabel, camModeIcon);
                    continue;
			    }
			    if(Event.Type == CMlScriptEvent::Type::MouseOver){
			        AnimateOpacity(Event.Control as CMlQuad, 0.8);
			        AnimateColorize(camModeIcon, "{{ Theme.UI_AccentPrimary }}");
			        continue;
			    }
			    if(Event.Type == CMlScriptEvent::Type::MouseOut){
			        AnimateOpacity(Event.Control as CMlQuad, {{ opacityUnfocused }});
			        AnimateColorize(camModeIcon, "ffffff");
			        continue;
			    }
			}
        }
    }
    --></script>
</component>
