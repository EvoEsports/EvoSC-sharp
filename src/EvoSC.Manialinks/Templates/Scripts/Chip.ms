<!--
Void CloseChip(Text ChipId) {
    declare CMlFrame ChipControl <=> (Page.MainFrame.GetFirstChild(ChipId) as CMlFrame);
    ChipControl.Hide();
    
    log("hidden");
    
    +++ OnChipClosed +++
}

Void ShowChip(Text ChipId) {
    declare CMlFrame ChipControl <=> (Page.MainFrame.GetFirstChild(ChipId) as CMlFrame);
    ChipControl.Show();
    
    +++ OnChipShown +++
}

Void Private_OnClose(CMlFrame ChipControl) {
    log(ChipControl.DataAttributeExists("data"));
    if (!ChipControl.DataAttributeExists("data")) {
        return;
    }
    
    declare Boolean IsClosable = TextLib::ToUpperCase(ChipControl.DataAttributeGet("data")) == "TRUE";
    
    log("Closable: " ^ ChipControl.DataAttributeGet("data"));
    log("Closable: " ^ IsClosable);
    
    if (IsClosable) {
        CloseChip(ChipControl.ControlId);
    }
}

*** OnMouseClick ***
***
    if (Event.Control.HasClass("chip-btnClose")) {
        declare Parent <=> Event.Control.Parent.Parent;
        Private_OnClose(Parent);
    }
***
-->
