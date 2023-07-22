<!--
Void SetSwitchState(Text idName, Boolean value) {
    declare bg = Page.MainFrame.GetFirstChild(idName^"-bg") as CMlQuad;
    declare head = Page.MainFrame.GetFirstChild(idName^"-head") as CMlQuad;
    declare icon = Page.MainFrame.GetFirstChild(idName^"-icon") as CMlLabel;
    declare entry = Page.MainFrame.GetFirstChild(idName^"-entry") as CMlEntry;
    declare onColor = <1., 0., 0.3450980392>; // #ff0058
    declare offColor = <0.2784313725, 0.2862745098, 0.3529411765>; // #47495A
    
    if (value) {
        bg.BgColor = onColor;
        head.RelativePosition_V3 = <5., 0.>;
        icon.RelativePosition_V3 = <7.5, -2.1>;
        icon.SetText("");
        icon.TextColor = onColor;
        entry.SetText("True", False);
    } else {
        bg.BgColor = offColor;
        head.RelativePosition_V3 = <0., 0.>;
        icon.RelativePosition_V3 = <2.5, -2.1>;
        icon.SetText("");
        icon.TextColor = offColor;
        entry.SetText("False", False);
    }
}
-->