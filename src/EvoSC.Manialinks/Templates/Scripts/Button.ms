<!--

Void HighlightButton(Text _Id, Boolean _Highlight, Boolean _IsSecondary) {
    log(_IsSecondary);
    Page.GetClassChildren(_Id ^ "-surface", Page.MainFrame, True);
    
    foreach (buttonPart in Page.GetClassChildren_Result) {
        declare part <=> buttonPart as CMlQuad;
        
        declare ColorFocus = {{ Color.ToMlColor(Theme.UI_Button_Default_BgFocus) }};
        declare ColorNormal = {{ Color.ToMlColor(Theme.UI_Button_Default_Bg) }};
        
        if (_IsSecondary) {
            ColorFocus = {{ Color.ToMlColor(Theme.UI_Button_Secondary_BgFocus) }};
            ColorNormal = {{ Color.ToMlColor(Theme.UI_Button_Secondary_Bg) }};
        }
        
        if (_Highlight) {
            part.BgColor = ColorFocus;
            part.ModulateColor = ColorFocus;
        } else {
            part.BgColor = ColorNormal;
            part.ModulateColor = ColorNormal;
        }
    }
}

*** OnMouseOver ***
***
if (Event.Control.HasClass("btn-btn")) {
    declare Text Id = Event.Control.DataAttributeGet("id");
    declare Frame = (Page.MainFrame.GetFirstChild(Id) as CMlFrame);
    HighlightButton(Id, True, Frame.DataAttributeGet("type") == "secondary");
}
***

*** OnMouseOut ***
***
if (Event.Control.HasClass("btn-btn")) {
    declare Text Id = Event.Control.DataAttributeGet("id");
    declare Frame = (Page.MainFrame.GetFirstChild(Id) as CMlFrame);
    HighlightButton(Id, False, Frame.DataAttributeGet("type") == "secondary");
}
***

-->
