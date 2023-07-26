<!--
Void CloseWindow(Text windowId) {
    declare Window = Page.MainFrame.GetFirstChild(windowId) as CMlFrame;
    Window.Hide();
    
    +++ OnWindowClosed +++
}

Void HideWindow(Text windowId) {
    declare Window = Page.MainFrame.GetFirstChild(windowId) as CMlFrame;
    Window.Visible = False;
    
    +++ OnWindowHidden +++
}

Void ShowWindow(Text windowId) {
    declare Window = Page.MainFrame.GetFirstChild(windowId) as CMlFrame;
    Window.Visible = True;
    
    +++ OnWindowShown +++
}

*** OnInitialization ***
***
declare Real EvoSC_Window_Z_Index for This = 100.;

declare WindowId = "{{ id }}";
declare WindowInstance <=> Page.MainFrame.GetFirstChild(WindowId) as CMlFrame;

{
    declare Boolean IsFocused for WindowInstance = False;
    declare Vec2 DragStartPosition for WindowInstance = WindowInstance.RelativePosition_V3;
    EvoSC_Window_Z_Index += 1;
    WindowInstance.ZIndex = EvoSC_Window_Z_Index;
}
***

*** OnMouseOver ***
***
if (Event.Control.HasClass("evosc-window-ctrlbtn")) {
    declare CloseBtn <=> Event.Control as CMlLabel;
    CloseBtn.TextColor = <0.84, 0.84, 0.87>;
}
***

*** OnMouseOut ***
***
if (Event.Control.HasClass("evosc-window-ctrlbtn")) {
    declare CloseBtn <=> Event.Control as CMlLabel;
    CloseBtn.TextColor = <1., 1., 1.>;
}
***

*** OnMouseClick ***
***
if (Event.Control.HasClass("evosc-window-closebtn")) {
    declare windowId = Event.Control.DataAttributeGet("id");
    HideWindow(windowId);
}
***

*** OnMouseDown ***
***
if ({{ canMove }}) {
    declare IsFocused for WindowInstance = False;
    declare DragStartPosition for WindowInstance = WindowInstance.RelativePosition_V3;
    
    DragStartPosition = WindowInstance.RelativePosition_V3;
    
    if (MouseX >= DragStartPosition[0] && MouseX <= DragStartPosition[0] + WindowInstance.Size[0]
        && MouseY <= DragStartPosition[1] && MouseY >= DragStartPosition[1] - 5) {
        IsFocused = True;
    }
    
    if (MouseX >= DragStartPosition[0]
     && MouseX <= DragStartPosition[0] + WindowInstance.Size[0]
     && MouseY <= DragStartPosition[1]
     && MouseY >= DragStartPosition[1] - WindowInstance.Size[1]) {
        declare Real EvoSC_Window_Z_Index for This = 100.;
        EvoSC_Window_Z_Index += 1;
        WindowInstance.ZIndex = EvoSC_Window_Z_Index;
    }
}
***

*** OnMouseUp ***
***
{
    declare IsFocused for WindowInstance = False;
    IsFocused = False;
}
***

*** OnMouseDrag ***
***
if ({{ canMove }}) {
    declare IsFocused for WindowInstance = False;
    
    if (IsFocused) {
        declare DragStartPosition for WindowInstance = WindowInstance.RelativePosition_V3;
        
        declare NewX = DragStartPosition[0] + (MouseX - MouseDragStartX);
        declare NewY = DragStartPosition[1] + (MouseY - MouseDragStartY);
        
        WindowInstance.RelativePosition_V3 = <NewX, NewY>;
    }
}
***
-->