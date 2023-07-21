Void __DoNothing() {}

main() {
    +++ OnInitialization +++
    
    while (True) {
        yield;
        
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
        
        +++ OnLoop +++
    }
}