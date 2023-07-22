*** OnInitialization ***
***
    declare switch1Value = False;
***

*** OnMouseClick ***
***
    if (Event.Control.ControlId == "switch1"
     || Event.Control.ControlId == "switch1-bg"
     || Event.Control.ControlId == "switch1-head"
     || Event.Control.ControlId == "switch1-icon") {
        log("yes");
        log(switch1Value);
        switch1Value = !switch1Value;
        SetSwitchState("switch1", switch1Value);
    }
***
