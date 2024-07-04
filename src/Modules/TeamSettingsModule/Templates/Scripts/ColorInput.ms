<!--

Void ShowColorPicker(CMlFrame colorInputFrame, CMlFrame colorPickerFrame) {
    declare entryFrame <=> (colorInputFrame.Controls[3] as CMlFrame);
    declare colorEntry <=> (entryFrame.Controls[1] as CMlEntry);
    declare colorPicker <=> (colorPickerFrame.Controls[1] as CMlColorPicker);
    
    colorPicker.Color = ColorLib::HexToRgb(colorEntry.Value);
    
    colorPickerFrame.Size.Y = 0.0;
    colorPickerFrame.Show();
    AnimMgr.Add(colorPickerFrame, "<frame size='"^(colorPickerFrame.Size.X)^" 22.0' />", 250, CAnimManager::EAnimManagerEasing::ExpOut);
}

*** OnMouseClick ***
***
if (Event.Control.ControlId == "labelPipette") {
    declare pipetteLabel = (Event.Control as CMlLabel);
    declare colorInputFrame <=> (Event.Control.Parent as CMlFrame);
    declare colorPickerFrame <=> (colorInputFrame.GetFirstChild("frameColorPicker") as CMlFrame);
    
    if(colorPickerFrame.Visible){
        colorPickerFrame.Hide();
        pipetteLabel.Opacity = 1.0;
    }else{
        ShowColorPicker(colorInputFrame, colorPickerFrame);
        pipetteLabel.Opacity = 0.75;
    }
}

if (Event.Control.ControlId == "picker") {
    declare picker <=> (Event.Control as CMlColorPicker);
    declare colorInputFrame <=> (picker.Parent.Parent as CMlFrame);
    declare entryFrame <=> (colorInputFrame.Controls[3] as CMlFrame);
    declare colorEntry <=> (entryFrame.Controls[1] as CMlEntry);
    
    colorEntry.Value = ColorLib::RgbToHex(picker.Color);
}
***

-->