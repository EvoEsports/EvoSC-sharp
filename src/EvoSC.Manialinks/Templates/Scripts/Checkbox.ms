<!--
Void SetCheckboxState(Text idName, Boolean value) {
    declare checkboxFrame <=> Page.MainFrame.GetFirstChild(idName) as CMlFrame;
    declare IsChecked for checkboxFrame = value;
    
    declare bg <=> checkboxFrame.Controls[4] as CMlQuad;
    declare icon <=> checkboxFrame.Controls[6] as CMlLabel;
    
    IsChecked = value;
    
    if (value) {
        AnimMgr.Add(bg, """<quad opacity='1' />""", 320, CAnimManager::EAnimManagerEasing::ExpOut);
        icon.SetText("");
    } else {
        AnimMgr.Add(bg, """<quad opacity='0' />""", 320, CAnimManager::EAnimManagerEasing::ExpOut);
        icon.SetText("");
    }
}

*** OnInitialization ***
***
    {
        Page.GetClassChildren("evosc-checkbox-frame", Page.MainFrame, True);
            
        foreach (ctrl in Page.GetClassChildren_Result) {
            declare valueText = TextLib::ToLowerCase(ctrl.DataAttributeGet("value"));
            SetCheckboxState(ctrl.ControlId, valueText == "true");
        }
    }
***

*** OnMouseClick ***
***
    if (Event.Control.HasClass("evosc-checkbox")) {
        declare ControlId = Event.Control.DataAttributeGet("id");
        declare Control <=> Page.MainFrame.GetFirstChild(ControlId);
        declare IsChecked for Control = False;
        
        SetCheckboxState(ControlId, !IsChecked);
        
        {
            +++ OnCheckboxToggle +++
        }
    }
***
-->
