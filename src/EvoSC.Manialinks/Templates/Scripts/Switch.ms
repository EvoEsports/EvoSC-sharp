<!--
Void SetSwitchState(Text idName, Boolean value) {
    declare switchFrame <=> Page.MainFrame.GetFirstChild(idName) as CMlFrame;
    declare SwitchValue for switchFrame = value;
    declare bg <=> switchFrame.Controls[0] as CMlQuad;
    declare head <=> switchFrame.Controls[1] as CMlQuad;
    declare icon <=> switchFrame.Controls[2] as CMlLabel;
    declare entry <=> switchFrame.Controls[3] as CMlEntry;
    declare onColor = TextLib::ToColor("ff0058");
    declare offColor = TextLib::ToColor("47495A" );
    
    SwitchValue = value;
    
    if (value) {
        bg.BgColor = onColor;
        icon.SetText("");
        AnimMgr.Add(head, """<quad pos='5 0' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        AnimMgr.Add(icon, """<label pos='7.5 -2.1' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        icon.TextColor = onColor;
        entry.SetText("True", False);
    } else {
        bg.BgColor = offColor;
        AnimMgr.Add(head, """<quad pos='0 0' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        AnimMgr.Add(icon, """<label pos='2.5 -2.1' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        icon.SetText("");
        icon.TextColor = offColor;
        entry.SetText("False", False);
    }
}

*** OnInitialization ***
***
{
    Page.GetClassChildren("evosc-toggleswitch-frame", Page.MainFrame, True);
    
    foreach (switchCtrl in Page.GetClassChildren_Result) {
        declare valueText = TextLib::ToLowerCase(switchCtrl.DataAttributeGet("value"));
        SetSwitchState(switchCtrl.ControlId, valueText == "true");
    }
}
***

*** OnMouseClick ***
***
    if (Event.Control.HasClass("evosc-toggleswitch")) {
        declare ControlId = Event.Control.DataAttributeGet("id");
        declare Control <=> Page.MainFrame.GetFirstChild(ControlId);
        declare SwitchValue for Control = False;
        
        SetSwitchState(ControlId, !SwitchValue);
        
        {
            +++ OnSwitchToggle +++
        }
    }
***
-->