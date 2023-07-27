#Include "TextLib" as TextLib

Void __DoNothing() {}

main() {
    declare Boolean MouseIsDown = False;
    declare Real MouseLastX = MouseX;
    declare Real MouseLastY = MouseY;
    declare Real MouseDragStartX = 0.0;
    declare Real MouseDragStartY = 0.0;

    +++ OnInitialization +++
    
    while (True) {
        yield;
        
        if (!PageIsVisible) {
            continue;
        }
        
        foreach (Event in PendingEvents) {
            switch (Event.Type) {
                case CMlScriptEvent::Type::KeyPress: { +++ OnKeyPress +++ }
                case CMlScriptEvent::Type::MouseClick: { +++ OnMouseClick +++ }
                case CMlScriptEvent::Type::MouseRightClick: {  +++ OnMouseRightClick +++  }
                case CMlScriptEvent::Type::MouseOver: {  +++ OnMouseOver +++  }
                case CMlScriptEvent::Type::MouseOut: {  +++ OnMouseOut +++  }
                case CMlScriptEvent::Type::EntrySubmit: {  +++ OnEntrySubmit +++  }
                case CMlScriptEvent::Type::MenuNavigation: {  +++ OnMenuNavigation +++  }
                case CMlScriptEvent::Type::PluginCustomEvent: {  +++ OnPluginCustomEvent +++  }
            }
            
            +++ OnEvent +++
        }
        
        foreach (Event in Input.PendingEvents) {
            +++ OnInputEvent +++
        }
        
        foreach (Event in Http.PendingEvents) {
            +++ OnHttpEvent +++
        }
        
        if (MouseLeftButton && !MouseIsDown) {
            MouseIsDown = True;
            MouseDragStartX = MouseX;
            MouseDragStartY = MouseY;
            
            +++ OnMouseDown +++
        } else if (!MouseLeftButton && MouseIsDown) {
            MouseIsDown = False;
            
            +++ OnMouseUp +++
        }
        
        if (MouseX != MouseLastX || MouseY != MouseLastY) {
            +++ OnMouseMove +++
            
            if (MouseIsDown) {
                +++ OnMouseDrag +++
            }
            
            MouseLastX = MouseX;
            MouseLastY = MouseY;
        }
        
        +++ OnLoop +++
    }
}
