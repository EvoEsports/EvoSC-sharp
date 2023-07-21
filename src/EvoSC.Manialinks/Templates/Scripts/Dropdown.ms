Void ShowDropdownPanel(Text idName, Boolean visible) {
    declare panel = Page.MainFrame.GetFirstChild(idName^"-panel") as CMlFrame;
    panel.Visible = visible;
}

*** OnInitialization ***
***
    declare mybtnVisible = False;
    ShowDropdownPanel("mybtn", mybtnVisible);
***

*** OnMouseClick ***
***
    if (Event.Control.ControlId == "mybtn-btn") {
        log("YEP 2");
        
        mybtnVisible = !mybtnVisible;
        ShowDropdownPanel("mybtn", mybtnVisible);
    } else {
        mybtnVisible = False;
        ShowDropdownPanel("mybtn", mybtnVisible);
    }
***
