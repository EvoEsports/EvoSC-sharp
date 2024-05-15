<!--
Void SetSwitchState(Text idName, Boolean value) {
    declare switchFrame <=> Page.MainFrame.GetFirstChild(idName) as CMlFrame;
    declare SwitchValue for switchFrame = value;
    declare bg <=> switchFrame.Controls[0] as CMlQuad;
    
    /* declare head <=> switchFrame.Controls[1] as CMlQuad;
    declare icon <=> switchFrame.Controls[2] as CMlLabel; */
    
    declare head <=> (Page.MainFrame.GetFirstChild(idName ^ "-head") as CMlQuad);
    declare icon <=> (Page.MainFrame.GetFirstChild(idName ^ "-icon") as CMlLabel);
    declare entry <=> switchFrame.Controls[2] as CMlEntry;
    
    log(idName ^ "-head");
    log(head);
    log(idName ^ "-icon");
    log(icon);
    
    SwitchValue = value;
    
    if (value) {
        icon.SetText("{{ Icons.Check }}");
        AnimMgr.Add(head, """<quad pos='5 0' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        AnimMgr.Add(icon, """<label pos='7.5 -2.1' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        icon.TextColor = {{ Color.ToMlColor(Theme.Green) }};
        entry.SetText("True", False);
    } else {
        AnimMgr.Add(head, """<quad pos='0 0' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        AnimMgr.Add(icon, """<label pos='2.5 -2.1' />""", 250, CAnimManager::EAnimManagerEasing::ExpOut);
        icon.SetText("{{ Icons.Close }}");
        icon.TextColor = {{ Color.ToMlColor(Theme.Red) }};
        entry.SetText("False", False);
    }
}

*** OnInitialization ***
***
{
    Page.GetClassChildren("evosc-toggleswitch-frame", Page.MainFrame, True);
    
    foreach (switchCtrl in Page.GetClassChildren_Result) {
        declare valueText = TextLib::ToLowerCase(switchCtrl.DataAttributeGet("value"));
        log(switchCtrl.ControlId);
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