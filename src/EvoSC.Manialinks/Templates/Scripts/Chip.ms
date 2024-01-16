<!--
Void CloseChip(Text ChipId) {
    declare CMlFrame ChipControl <=> (Page.MainFrame.GetFirstChild(ChipId) as CMlFrame);
    ChipControl.Hide();
    
    +++ OnChipClosed +++
}

Void ShowChip(Text ChipId) {
    declare CMlFrame ChipControl <=> (Page.MainFrame.GetFirstChild(ChipId) as CMlFrame);
    ChipControl.Show();
    
    +++ OnChipShown +++
}

Void Private_OnClose(Text ChipId, CMlFrame ChipControl) {
    if (!ChipControl.DataAttributeExists("data")) {
        return;
    }
    
    declare Boolean IsClosable = TextLib::ToUpperCase(ChipControl.DataAttributeGet("data")) == "TRUE";
    
    if (IsClosable) {
        CloseChip(ChipId);
    }
}

*** OnMouseClick ***
***
    if (Event.Control.HasClass("chip-btnClose")) {
        declare Parent <=> Event.Control.Parent.Parent;
        declare Text ParentId = Parent.ControlId;
        Private_OnClose(ParentId, Parent);
    }
***
-->
