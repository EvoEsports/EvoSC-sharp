*** OnSwitchToggle ***
***
    declare persistent Boolean SB_Setting_ShowSpectators for LocalUser = True;
    declare persistent Boolean SB_Setting_ShowDisconnected for LocalUser = True;
    
    switch(ControlId){
        case "switchHideSpectators": { SB_Setting_ShowSpectators = !SB_Setting_ShowSpectators; }
        case "switchHideDisconnected": { SB_Setting_ShowDisconnected = !SB_Setting_ShowDisconnected; }
    }
***
