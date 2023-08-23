<!--
declare Integer[Text] RadioButtonGroups;
declare Text[][] RadioButtonGroupIds;

Boolean SetRadioButtonState(Text idName, Boolean value) {
    declare control <=> Page.MainFrame.GetFirstChild(idName) as CMlFrame;
    declare group = control.DataAttributeGet("group");
    
    declare icon <=> control.Controls[0] as CMlLabel;
    declare entry <=> control.Controls[2] as CMlEntry;
    
    declare IsChecked for control = value;
    
    if (IsChecked != value) {
        IsChecked = value;
    } else {
        return False;
    }
    
    if (IsChecked) {
        icon.SetText("◉");
        entry.SetText("True", False);
        
        foreach (otherBtnId in RadioButtonGroupIds[RadioButtonGroups[group]]) {
            if (otherBtnId == idName) {
                continue;
            }
            
            SetRadioButtonState(otherBtnId, False);
        }
    } else {
        icon.SetText("○");
        entry.SetText("False", False);
    }
    
    return True;
}

*** OnInitialization ***
***
{
    Page.GetClassChildren("evosc-radiobutton-frame", Page.MainFrame, True);
    
    foreach (ctrl in Page.GetClassChildren_Result) {
        declare initValue = ctrl.DataAttributeGet("initvalue");
        declare group = ctrl.DataAttributeGet("group");
        declare IsChecked for ctrl = False;
        
        if (initValue == "True") {
            IsChecked = True;
        }
        
        if (!RadioButtonGroups.existskey(group)) {
            RadioButtonGroupIds.add([ctrl.ControlId]);
            RadioButtonGroups[group] = RadioButtonGroupIds.count-1;
        } else {
            RadioButtonGroupIds[RadioButtonGroups[group]].add(ctrl.ControlId);
        }
    }
}
***

*** OnMouseClick ***
***
if (Event.Control.HasClass("evosc-radiobutton")) {
    declare ControlId = Event.Control.DataAttributeGet("id");
    declare Control <=> Page.MainFrame.GetFirstChild(ControlId);
    declare IsChecked for Control = False;
    
    if (SetRadioButtonState(ControlId, !IsChecked)) {
        +++ OnRadioButtonToggle +++
    }
}
***

-->