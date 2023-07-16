<component>
    <import component="MotdModule.MotdWindow" as="MotdWindow" />
    
    <property type="string" name="text" />

    <property type="string" name="primaryColor" default="ff0058"/>
    <property type="string" name="backgroundColor" default="47495A"/>

    <property type="double" name="titleBarHeight" default="6.0"/>
    
    <property type="double" name="buttonBarHeight" default="7.0"/>

    <property type="string" name="checkboxUncheckedIcon" default="" />
    <property type="string" name="checkboxCheckedIcon" default="" />

    <property type="int" name="zIndex" default="1" />
    <property type="double" name="w" default="160" />
    <property type="double" name="h" default="90" />
    
    <template>
        <MotdWindow id="window" h="{{ h }}" w="{{ w }}" x="0" y="0" zIndex="1000" title="Message of the Day" titleBarHeight="{{ titleBarHeight }}">
            <!--
            <label autonewline="1" text="{{ text }}" z-index="{{ zIndex + 1 }}" pos="0 0" size="{{ w }} {{ h - (buttonBarHeight + titleBarHeight*2) }}" />
            -->
            <textedit id="textedit_text" text="{{ text }}" autonewline="1" z-index="{{ zIndex + 1 }}" pos="0 0" size="{{ w }} {{ h - (buttonBarHeight + titleBarHeight*2) }}" />
            
            <!-- ButtonBar -->
            <frame pos="0 {{ -h + buttonBarHeight*2 + 2 }}" size="{{ w }} {{ buttonBarHeight+2 }}" z-index="{{ zIndex + 1 }}">
                <!-- Close Button -->
                <label id="button_save" scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" 
                       style="CardButtonMediumWide" hidden="0" text="Save" z-index="20" pos="{{ w/2 }} 0" halign="left" size="0 {{ buttonBarHeight/2 }}" />
                <label id="button_close" scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" 
                       style="CardButtonMediumWide" hidden="0" text="Close" z-index="20" pos="{{ w/2 }} 0" halign="right" size="0 {{ buttonBarHeight/2 }}" />
            </frame>
        </MotdWindow>
        <script>
            <!--
            *** OnMouseClick ***
            ***
            if(Event.Control.ControlId == "button_close") {
                Page.GetFirstChild("window").Hide();
            } else if(Event.Control.ControlId == "button_save") {
                TriggerPageAction("MotdEditManialinkController/Save/"^(Page.GetFirstChild("textedit_text") as CMlTextEdit).Value);
            }
            ***
            
            main() {
              +++OnInit+++
            
              while(True) {
                yield;
                if (!PageIsVisible || InputPlayer == Null) {
                          continue;
                  }
            
                foreach (Event in PendingEvents) {
                        switch (Event.Type) {
                            case CMlScriptEvent::Type::EntrySubmit: {
                                +++EntrySubmit+++
                            }
                            case CMlScriptEvent::Type::KeyPress: {
                                +++OnKeyPress+++
                            }
                            case CMlScriptEvent::Type::MouseClick: {
                                +++OnMouseClick+++
                            }
                            case CMlScriptEvent::Type::MouseOut: {
                                +++OnMouseOut+++
                            }
                            case CMlScriptEvent::Type::MouseOver: {
                                +++OnMouseOver+++
                            }
                        }
                    }
            
                    +++Loop+++
              }
            }
            -->
        </script>
    </template>
</component>