<!--
Void Private_HidePlaceholder(Text _Id) {
    log("ID: " ^ _Id);
    declare CMlFrame TextInputFrame <=> (Page.MainFrame.GetFirstChild(_Id) as CMlFrame);
    TextInputFrame.Controls[3].Hide();
}

Void Private_ShowPlaceholderIfEmpty(Text _Id) {
    log("ID: " ^ _Id);
    
    declare CMlFrame TextInputFrame <=> (Page.MainFrame.GetFirstChild(_Id) as CMlFrame);
    declare CMlEntry TextInputEntry <=> (TextInputFrame.Controls[2] as CMlEntry);
    
    if (TextInputEntry.Value == "") {
        TextInputFrame.Controls[3].Show();
    }
}

*** OnInitialization ***
***
declare Text FocusedTextInputId = "";
***

*** OnLoop ***
***
if (Page.FocusedControl != Null && Page.FocusedControl.HasClass("textinput-default")) {
    declare Text NewFocused = Page.FocusedControl.DataAttributeGet("id");
    
    if (NewFocused != FocusedTextInputId) {
        Private_ShowPlaceholderIfEmpty(NewFocused);
    }
    
    FocusedTextInputId = NewFocused;
    
    Private_HidePlaceholder(FocusedTextInputId);
} else if (FocusedTextInputId != "") {
    Private_ShowPlaceholderIfEmpty(FocusedTextInputId);
    FocusedTextInputId = "";
}
***
-->