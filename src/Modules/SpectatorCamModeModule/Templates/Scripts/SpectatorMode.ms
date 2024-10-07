<!--
#Include "TextLib" as TextLib
#Include "ColorLib" as ColorLib
#Include "Libs/Nadeo/CMGame/Utils/Icons.Script.txt" as Icons

#Const C_CamModes_Replay 0
#Const C_CamModes_Follow 1
#Const C_CamModes_FollowAll 2
#Const C_CamModes_Free 3

declare Boolean G_SelectedFollowAll;

Void SetCamModeNameAndIcon(Integer _CamMode, CMlFrame camModeFrame) {
    declare camModeLabel <=> (camModeFrame.GetFirstChild("cam_mode_label") as CMlLabel);
    declare camModeBg <=> (camModeFrame.GetFirstChild("cam_mode_bg") as CMlQuad);
    declare camModeQuad <=> (camModeFrame.GetFirstChild("cam_mode_icon") as CMlQuad);
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
    if (camModeQuad != Null){
        camModeQuad.ImageUrl = Icon_CamMode;
        camModeQuad.RelativePosition_V3.X = {{ width }} * 1.0 - spacing - camModeQuad.Size.X;
    }
    if (camModeLabel != Null){
        camModeLabel.Value = TextLib::ToUpperCase(Text_CamMode);
        camModeLabel.RelativePosition_V3.X = camModeQuad.RelativePosition_V3.X - spacing;
    }
    camModeFrame.Size.X = 32.0;
    camModeBg.Size.X = 32.0;
    
    declare Integer targetCamMode for camModeFrame;
    targetCamMode = _CamMode;
    
    if({{ settings.Alignment == WidgetPosition.Right ? "True" : "False" }}){
        camModeFrame.RelativePosition_V3.X = camModeFrame.Size.X * -1.0;
    }else if({{ settings.Alignment == WidgetPosition.Center ? "True" : "False" }}){
        camModeFrame.RelativePosition_V3.X = camModeFrame.Size.X / -2.0;
    }
}

/**
 * Copied from "Libs/Nadeo/TMGame/Modes/Base/UIModules/SpectatorBase_Client.Script.txt"
 */
Integer SetCamMode(Integer _CamMode, CMlFrame camModeFrame) {
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
            CamMode = SetCamMode(C_CamModes_FollowAll, camModeFrame);
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
    
    SetCamModeNameAndIcon(CamMode, camModeFrame);

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

Void AnimateExpandWrapper(CMlFrame modesWrapper){
    modesWrapper.Show();
}

Void AnimateCloseWrapper(CMlFrame modesWrapper){
    modesWrapper.Hide();
}

Void AnimateButtonMouseOver(CMlQuad bgTrigger){
    declare camModeIcon <=> (bgTrigger.Parent.GetFirstChild("cam_mode_icon") as CMlQuad);
    AnimateOpacity(bgTrigger, 0.8);
    AnimateColorize(camModeIcon, "{{ Theme.UI_AccentPrimary }}");
}

Void AnimateButtonMouseOut(CMlQuad bgTrigger){
    declare camModeIcon <=> (bgTrigger.Parent.GetFirstChild("cam_mode_icon") as CMlQuad);
    AnimateOpacity(bgTrigger, {{ opacityUnfocused }});
    AnimateColorize(camModeIcon, "{{ Theme.UI_TextPrimary }}");
}

Void SetupModesDropUp(CMlFrame wrapperFrame){
    for(i, 0, 3){
        declare modeButtonFrame = (wrapperFrame.Controls[i] as CMlFrame);
        SetCamModeNameAndIcon(i, modeButtonFrame);
        modeButtonFrame.RelativePosition_V3.Y = {{ -height }} * i;
    
        if({{ settings.Alignment == WidgetPosition.Right ? "True" : "False" }}){
            modeButtonFrame.RelativePosition_V3.X = {{ width }} - modeButtonFrame.Size.X;
        }else if({{ settings.Alignment == WidgetPosition.Center ? "True" : "False" }}){
            modeButtonFrame.RelativePosition_V3.X = {{ width }} - modeButtonFrame.Size.X / 2.0;
        }
    }
    
    if({{ settings.Alignment == WidgetPosition.Right ? "True" : "False" }}){
        wrapperFrame.RelativePosition_V3.X = {{ width }} * -1.0;
    }else if({{ settings.Alignment == WidgetPosition.Center ? "True" : "False" }}){
        wrapperFrame.RelativePosition_V3.X = {{ width }} / -2.0;
    }
}

Void HideCurrentlySelectedModeAndRearrange(CMlFrame wrapperFrame, Integer selectedCamMode){
    declare offset = 0;
    for(i, 0, 3){
        declare modeButtonFrame = (wrapperFrame.Controls[i] as CMlFrame);
        
        if(selectedCamMode == i){
            modeButtonFrame.Hide();
            continue;
        }
        
        modeButtonFrame.RelativePosition_V3.Y = {{ -height }} * offset;
        modeButtonFrame.Show();
        offset += 1;
    }
}

main(){
    declare mainFrame <=> (Page.MainFrame.GetFirstChild("main_frame") as CMlFrame);
    declare camModeFrame <=> (Page.MainFrame.GetFirstChild("cam_mode_frame") as CMlFrame);
    declare modesWrapperFrame <=> (Page.MainFrame.GetFirstChild("modes_wrapper") as CMlFrame);

    declare netread Integer Net_Race_SpectatorBase_CamMode for UI;
    declare netread Integer Net_Race_SpectatorBase_CamUpdate for UI;
    
    declare lastCamUpdate = Net_Race_SpectatorBase_CamUpdate;
    declare camMode = SetCamMode(Net_Race_SpectatorBase_CamMode, camModeFrame);
    declare isExpanded = False;
    
    SetupModesDropUp(modesWrapperFrame);
    HideCurrentlySelectedModeAndRearrange(modesWrapperFrame, camMode);
    
    while(True){
        yield;
        
        declare shouldBeDisplayed = InputPlayer != Null 
                                    && InputPlayer.SpawnStatus == CSmPlayer::ESpawnStatus::NotSpawned
                                    && UI.UISequence == CUIConfig::EUISequence::Playing;
        
        if(!shouldBeDisplayed){
            if(mainFrame.Visible){
                mainFrame.Hide();
            }
            sleep(500);
            continue;
        }
        
        if(!mainFrame.Visible){
            mainFrame.Show();
        }
        
        declare Integer AssumedCamMode = GetAssumedCamMode(camMode);
        
        if (camMode != AssumedCamMode) {
            camMode = AssumedCamMode;
            SetCamModeNameAndIcon(camMode, camModeFrame);
        }
        
        if(lastCamUpdate != Net_Race_SpectatorBase_CamUpdate){
            camMode = SetCamMode(Net_Race_SpectatorBase_CamMode, camModeFrame);
            lastCamUpdate = Net_Race_SpectatorBase_CamUpdate;
        }
        
        foreach(Event in PendingEvents){
            if(Event.Type == CMlScriptEvent::Type::MouseClick){
                if(Event.Control.Parent.ControlId == "cam_mode_frame"){
                    camMode = SetCamMode((camMode + 1) % 4, camModeFrame);
                }else{
                    declare Integer targetCamMode for Event.Control.Parent = 0;
                    camMode = SetCamMode(targetCamMode, camModeFrame);
                }
                
                if(isExpanded){
                    AnimateCloseWrapper(modesWrapperFrame);
                    isExpanded = False;
                }
    
                continue;
            }
            if(Event.Type == CMlScriptEvent::Type::MouseRightClick && Event.Control.Parent.ControlId == "cam_mode_frame"){
                if(isExpanded){
                    AnimateCloseWrapper(modesWrapperFrame);
                }else{
                    HideCurrentlySelectedModeAndRearrange(modesWrapperFrame, camMode);
                    AnimateExpandWrapper(modesWrapperFrame);
                }
                isExpanded = !isExpanded;
                continue;
            }
            if(Event.Type == CMlScriptEvent::Type::MouseOver){
                AnimateButtonMouseOver(Event.Control as CMlQuad);
                continue;
            }
            if(Event.Type == CMlScriptEvent::Type::MouseOut){
                AnimateButtonMouseOut(Event.Control as CMlQuad);
                continue;
            }
        }
    }
}
-->