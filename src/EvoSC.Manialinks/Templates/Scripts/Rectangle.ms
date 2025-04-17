<!--
Void Rectangle_SetBgColor(Text _Id, Vec3 _Color) {
    (Page.MainFrame.GetFirstChild(_Id ^ "-rect-quad-left") as CMlQuad).BgColor = _Color;
    (Page.MainFrame.GetFirstChild(_Id ^ "-rect-quad-right") as CMlQuad).BgColor = _Color;
    (Page.MainFrame.GetFirstChild(_Id ^ "-rect-quad-bottom") as CMlQuad).BgColor = _Color;
    (Page.MainFrame.GetFirstChild(_Id ^ "-rect-quad-top") as CMlQuad).BgColor = _Color;
    (Page.MainFrame.GetFirstChild(_Id ^ "-rect-quad-middle") as CMlQuad).BgColor = _Color;
    
    declare QuarterTopLeft <=> Page.MainFrame.GetFirstChild(_Id ^ "-rect-quarter-topleft");
    if (QuarterTopLeft != Null) { (QuarterTopLeft as CMlQuad).ModulateColor = _Color; }
    
    declare QuarterTopRight <=> Page.MainFrame.GetFirstChild(_Id ^ "-rect-quarter-topright");
    if (QuarterTopRight != Null) { (QuarterTopRight as CMlQuad).ModulateColor = _Color; }
    
    declare QuarterBottomLeft <=> Page.MainFrame.GetFirstChild(_Id ^ "-rect-quarter-bottomleft");
    if (QuarterBottomLeft != Null) { (QuarterBottomLeft as CMlQuad).ModulateColor = _Color; }
    
    declare QuarterBottomRight <=> Page.MainFrame.GetFirstChild(_Id ^ "-rect-quarter-bottomright");
    if (QuarterBottomRight != Null) { (QuarterBottomRight as CMlQuad).ModulateColor = _Color; }
}
-->