<!--
#Include "TextLib" as TextLib

Void __DoNothing() {}

main() {
    declare Boolean MouseIsDown = False;
    declare Vec2 MouseLastPos = <MouseX, MouseY>;
    declare Vec2 MouseDragStart = <0., 0.>;

    // set to true before initialization if scripts wants to terminate immediately
    declare Boolean ScriptRunning for This = False;
    ScriptRunning = True;

    +++ OnInitialization +++
    
    while (ScriptRunning) {
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
            MouseDragStart = <MouseX, MouseY>;
            
            +++ OnMouseDown +++
        } else if (!MouseLeftButton && MouseIsDown) {
            MouseIsDown = False;
            
            +++ OnMouseUp +++
        }
        
        if (MouseX != MouseLastPos.X || MouseY != MouseLastPos.Y) {
            { +++ OnMouseMove +++ }
            
            if (MouseIsDown) {
                +++ OnMouseDrag +++
            }
            
            MouseLastPos = <MouseX, MouseY>;
        }
        
        +++ OnLoop +++
    }
    
    +++ OnScriptExecutionFinished +++
    
    log("Script execution finished.");
}
-->
