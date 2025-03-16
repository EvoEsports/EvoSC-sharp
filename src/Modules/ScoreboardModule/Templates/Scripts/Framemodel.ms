<!--
Void ShowPlayerActions(CMlFrame playerRow) {
    declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
    declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
    declare backgroundWrapper = (playerRow.GetFirstChild("player_row_bg") as CMlFrame);
    playerActions.Show();
    detailsWrapper.Hide();
    backgroundWrapper.Size.X = ({{ (w - positionBoxWidth) - actionButtonCount*(rowHeight*1.2) - actionButtonCount*rowSpacing - 0.1 }}) * 1.0;
}

Void HidePlayerActions(CMlFrame playerRow) {
    declare playerActions = (playerRow.GetFirstChild("player_actions") as CMlFrame);
    declare detailsWrapper = (playerRow.GetFirstChild("details_wrapper") as CMlFrame);
    declare backgroundWrapper = (playerRow.GetFirstChild("player_row_bg") as CMlFrame);
    playerActions.Hide();
    detailsWrapper.Show();
    backgroundWrapper.Size.X = {{ (w - positionBoxWidth) }} * 1.0;
}

Void ResetPlayerActions() {
    declare playerRows <=> (Page.MainFrame.GetFirstChild("frame_scroll") as CMlFrame);
    foreach(playerRowControl in playerRows.Controls){
        declare playerRow = (playerRowControl as CMlFrame);
        HidePlayerActions(playerRow);
        declare Boolean RowIsLocked for playerRow = False;
        RowIsLocked = False;
    }
}

Void TogglePlayerActions(CMlFrame playerRow) {
    declare Boolean RowIsLocked for playerRow = False;
    
    if(RowIsLocked){
        RowIsLocked = False;
        HidePlayerActions(playerRow);
    }else{
        ResetPlayerActions();
        RowIsLocked = True;
        ShowPlayerActions(playerRow);
    }
}

Vec3 GetCustomLabelColor(Text value, Text defaultColorHex){
    declare customLabelColorHex = "";
    switch(value){
        case _("|Status|Winner"): customLabelColorHex = "{{ Theme.ScoreboardModule_WinnerColor }}";
        case _("|Status|Finalist"): customLabelColorHex = "{{ Theme.ScoreboardModule_FinalistColor }}";
    }
    
    if(customLabelColorHex == ""){
        customLabelColorHex = defaultColorHex;
    }
    
    return CL::HexToRgb(customLabelColorHex);
}

Void SetCustomLabel(CMlFrame playerRow, Text value, Text hexColor){
    declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
    customLabel.Value = value;
    customLabel.TextColor = GetCustomLabelColor(value, hexColor);
    SetPlayerHighlightColor(playerRow, customLabel.TextColor);
}

Void HideCustomLabel(CMlFrame playerRow){
    declare customLabel = (playerRow.GetFirstChild("custom_label") as CMlLabel);
    customLabel.Value = "";
    ResetPlayerHighlightColor(playerRow);
}

*** OnMouseClick ***
***
    if(Event.Control.ControlId == "player_row_trigger"){
        TogglePlayerActions(Event.Control.Parent as CMlFrame);
        continue;
    }
***

*** OnMouseOver ***
***
    if(Event.Control.ControlId == "player_row_trigger"){
        declare parentFrame = (Event.Control.Parent as CMlFrame);
        declare backgroundFrame <=> (parentFrame.GetFirstChild("player_row_bg") as CMlFrame);
        RowMouseOver(backgroundFrame);
        continue;
    }
***

*** OnMouseOut ***
***
    if(Event.Control.ControlId == "player_row_trigger"){
        declare parentFrame = (Event.Control.Parent as CMlFrame);
        declare backgroundFrame <=> (parentFrame.GetFirstChild("player_row_bg") as CMlFrame);
        RowMouseOut(backgroundFrame);
        continue;
    }
***
-->
