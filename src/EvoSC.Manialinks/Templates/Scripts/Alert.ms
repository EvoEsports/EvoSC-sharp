<!--
Void ShowAlert(Text alertId) {
    declare frame <=> Page.MainFrame.GetFirstChild(alertId) as CMlFrame;
    declare frameInner <=> Page.MainFrame.GetFirstChild(alertId^"-inner-frame") as CMlFrame;
    
    declare startX = frame.DataAttributeGet("startX");
    declare startY = frame.DataAttributeGet("startY");
    
    AnimMgr.Add(frameInner, "<frame pos='"^(frame.Size.X-7)^" 0' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
    sleep(250);
    AnimMgr.Add(frame, "<frame pos='"^startX^" "^startY^"' />", 200, CAnimManager::EAnimManagerEasing::ExpOut);
    AnimMgr.Add(frameInner, "<frame pos='0 0' />", 200, CAnimManager::EAnimManagerEasing::ExpOut);
}

Void HideAlert(Text alertId) {
    declare frame <=> Page.MainFrame.GetFirstChild(alertId) as CMlFrame;
    declare frameInner <=> Page.MainFrame.GetFirstChild(alertId^"-inner-frame") as CMlFrame;
    
    declare width = frame.Size.X;
    declare startX = TextLib::ToInteger(frame.DataAttributeGet("startX"));
    declare startY = TextLib::ToInteger(frame.DataAttributeGet("startY"));

    AnimMgr.Add(frame, "<frame pos='"^(startX - width+7)^" "^startY^"' />", 200, CAnimManager::EAnimManagerEasing::ExpOut);    
    AnimMgr.Add(frameInner, "<frame pos='"^(width-7)^" 0' />", 200, CAnimManager::EAnimManagerEasing::ExpOut);    
    sleep(250);
    AnimMgr.Add(frameInner, "<frame pos='"^(width-7)^" 8' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
}

*** OnInitialization ***
***
    Page.GetClassChildren("evosc-alert-frame", Page.MainFrame, True);
        
    foreach (alertFrame in Page.GetClassChildren_Result) {
        declare frame <=> alertFrame as CMlFrame;
        declare frameInner <=> frame.Controls[0] as CMlFrame;
        
        declare startX = TextLib::ToInteger(frame.DataAttributeGet("startX"));
        declare startY = TextLib::ToReal(frame.DataAttributeGet("startY"));
        
        frame.RelativePosition_V3 = <startX - frame.Size.X + 7., startY>;
        frameInner.RelativePosition_V3 = <frame.Size.X - 7., 7.>;
    }
***
-->
