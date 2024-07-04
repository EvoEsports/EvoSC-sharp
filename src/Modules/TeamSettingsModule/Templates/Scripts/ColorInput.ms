<!--

Void ShowColorPicker(CMlFrame colorInputFrame, CMlFrame colorPickerDropdownFrame) {
    declare entryFrame <=> (colorInputFrame.Controls[3] as CMlFrame);
    declare colorEntry <=> (entryFrame.Controls[1] as CMlEntry);
    declare colorPicker <=> (colorPickerDropdownFrame.Controls[1] as CMlColorPicker);
    
    colorPicker.Color = ColorLib::HexToRgb(colorEntry.Value);
    
    colorPickerDropdownFrame.Size.Y = 0.0;
    colorPickerDropdownFrame.Show();
    AnimMgr.Add(colorPickerDropdownFrame, "<frame size='"^(colorPickerDropdownFrame.Size.X)^" 22.0' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
}

*** OnMouseClick ***
***
if (Event.Control.ControlId == "labelPipette") {
    declare pipetteLabel = (Event.Control as CMlLabel);
    declare colorInputFrame <=> (Event.Control.Parent as CMlFrame);
    declare colorPickerDropdownFrame <=> (colorInputFrame.GetFirstChild("frameColorPicker") as CMlFrame);
    
    if(colorPickerDropdownFrame.Visible){
        colorPickerDropdownFrame.Hide();
        pipetteLabel.Opacity = 1.0;
    }else{
        ShowColorPicker(colorInputFrame, colorPickerDropdownFrame);
        pipetteLabel.Opacity = 0.75;
    }
}

if (Event.Control.ControlId == "picker") {
    declare picker <=> (Event.Control as CMlColorPicker);
    declare colorInputFrame <=> (picker.Parent.Parent as CMlFrame);
    declare previewLabel <=> (colorInputFrame.GetFirstChild("rectangleColorPreview") as CMlQuad);
    declare entryFrame <=> (colorInputFrame.Controls[3] as CMlFrame);
    declare colorEntry <=> (entryFrame.Controls[1] as CMlEntry);
    
    colorEntry.Value = ColorLib::RgbToHex(picker.Color);
    previewLabel.ModulateColor = picker.Color;
}
***

*** EntrySubmit ***
***
if (Event.Control.HasClass("colorInput")) {
    declare colorEntry <=> (Event.Control as CMlEntry);
    log("color:" ^ colorEntry.Value);
}
***

-->