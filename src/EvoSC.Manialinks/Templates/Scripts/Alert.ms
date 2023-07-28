<!--
Void ShowAlert(Text alertId) {
    declare frame <=> Page.MainFrame.GetFirstChild(alertId) as CMlFrame;
    declare frameInner <=> Page.MainFrame.GetFirstChild(alertId^"-inner-frame") as CMlFrame;
    declare width = TextLib::ToInteger(frame.DataAttributeGet("width"));
    declare startX = frame.DataAttributeGet("startX");
    declare startY = frame.DataAttributeGet("startY");
    
    AnimMgr.Add(frameInner, "<frame pos='"^(width-6)^" 0' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
    sleep(250);
    AnimMgr.Add(frame, "<frame pos='"^startX^" "^startY^"' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
    AnimMgr.Add(frameInner, "<frame pos='0 0' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
}

Void HideAlert(Text alertId) {
    declare frame <=> Page.MainFrame.GetFirstChild(alertId) as CMlFrame;
    declare frameInner <=> Page.MainFrame.GetFirstChild(alertId^"-inner-frame") as CMlFrame;
    declare width = TextLib::ToInteger(frame.DataAttributeGet("width"));
    declare startX = TextLib::ToInteger(frame.DataAttributeGet("startX"));
    declare startY = TextLib::ToInteger(frame.DataAttributeGet("startY"));

    log("hide");
    AnimMgr.Add(frame, "<frame pos='"^(startX - width+6)^" "^startY^"' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);    
    AnimMgr.Add(frameInner, "<frame pos='"^(width-6)^" 0' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);    
    sleep(250);
    AnimMgr.Add(frameInner, "<frame pos='"^(width-6)^" "^(startY+7)^"' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
}

*** OnInitialization ***
***
    Page.GetClassChildren("evosc-alert-frame", Page.MainFrame, True);
        
        foreach (alertFrame in Page.GetClassChildren_Result) {
            declare frame <=> alertFrame as CMlFrame;
            declare frameInner <=> frame.Controls[0] as CMlFrame;
            
            declare width = TextLib::ToInteger(frame.DataAttributeGet("width"));
            declare startX = TextLib::ToInteger(frame.DataAttributeGet("startX"));
            declare startY = TextLib::ToReal(frame.DataAttributeGet("startY"));
            
            frame.RelativePosition_V3 = <startX - width+6., startY>;
            frameInner.RelativePosition_V3 = <width - 6., 7.>;
        }
***
-->
