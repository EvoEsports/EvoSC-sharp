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
