<!--
Void SetSwitchState(Text idName, Boolean value) {
    declare switchFrame <=> Page.MainFrame.GetFirstChild(idName) as CMlFrame;
    declare SwitchValue for switchFrame = value;
    declare bg <=> switchFrame.Controls[0] as CMlQuad;
    declare head <=> switchFrame.Controls[1] as CMlQuad;
    declare icon <=> switchFrame.Controls[2] as CMlLabel;
    declare entry <=> switchFrame.Controls[3] as CMlEntry;
    declare onColor = <1., 0., 0.3450980392>; // #ff0058
    declare offColor = <0.2784313725, 0.2862745098, 0.3529411765>; // #47495A
    
    SwitchValue = value;
    
    if (value) {
        bg.BgColor = onColor;
        head.RelativePosition_V3 = <5., 0.>;
        icon.RelativePosition_V3 = <7.5, -2.1>;
        icon.SetText("");
        icon.TextColor = onColor;
        entry.SetText("True", False);
    } else {
        bg.BgColor = offColor;
        head.RelativePosition_V3 = <0., 0.>;
        icon.RelativePosition_V3 = <2.5, -2.1>;
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
        SetSwitchState(switchCtrl.ControlId, False);
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
        
        +++ OnSwitchToggle +++
    }
***
-->