<!--
Void SetCheckboxState(Text idName, Boolean value) {
    declare checkboxFrame <=> Page.MainFrame.GetFirstChild(idName) as CMlFrame;
    declare IsChecked for checkboxFrame = value;
    
    declare bg <=> checkboxFrame.Controls[4] as CMlQuad;
    declare icon <=> checkboxFrame.Controls[6] as CMlLabel;
    
    IsChecked = value;
    
    if (value) {
        bg.Opacity = 1.;
        icon.SetText("");
    } else {
        bg.Opacity = 0.;
        icon.SetText("");
    }
}

*** OnInitialization ***
***
    {
        Page.GetClassChildren("evosc-checkbox-frame", Page.MainFrame, True);
            
        foreach (ctrl in Page.GetClassChildren_Result) {
            SetCheckboxState(ctrl.ControlId, False);
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
