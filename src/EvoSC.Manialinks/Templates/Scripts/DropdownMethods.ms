Void ShowDropdownPanel(Text idName, Boolean visible) {
    declare panel = Page.MainFrame.GetFirstChild(idName^"-panel") as CMlFrame;
    panel.Visible = visible;
}