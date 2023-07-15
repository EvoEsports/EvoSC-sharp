<component>
    <import component="MotdModule.MotdWindow" as="MotdWindow" />

    <property type="bool" name="isChecked" />
    <property type="string" name="text" />

    <property type="string" name="primaryColor" default="ff0058"/>
    <property type="string" name="backgroundColor" default="47495A"/>

    <property type="double" name="titleBarHeight" default="6.0"/>
    
    <property type="double" name="buttonBarHeight" default="7.0"/>
    <property type="string" name="buttonText" default="Close"/>

    <property type="string" name="checkboxUncheckedIcon" default="" />
    <property type="string" name="checkboxCheckedIcon" default="" />

    <property type="int" name="zIndex" default="1" />
    <property type="double" name="w" default="160" />
    <property type="double" name="h" default="90" />
    
    <template>
        <MotdWindow h="{{ h }}" w="{{ w }}" x="0" y="0" isChecked="{{ isChecked }}" zIndex="1000" title="Message of the Day" titleBarHeight="{{ titleBarHeight }}">
            <label autonewline="1" text="{{ text }}" z-index="{{ zIndex + 1 }}" pos="0 0" size="{{ w }} {{ h - (buttonBarHeight + titleBarHeight*2) }}" />
            <!-- ButtonBar -->
            <frame pos="0 {{ -h + buttonBarHeight*2 + 2 }}" size="{{ w }} {{ buttonBarHeight }}" z-index="{{ zIndex + 1 }}">
                <!-- Dont show again Checkbox -->
                <label id="checkbox" scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" pos="0 -.5" size="{{ buttonBarHeight/2 }} {{ buttonBarHeight/2 }}" z-index="{{ zIndex + 2}}" text="{{ (isChecked) ? checkboxCheckedIcon : checkboxUncheckedIcon }}" textcolor="{{ primaryColor }}"/>
                <label id="checkboxText" scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" pos="6 -2" size="{{ w/4 }} {{ buttonBarHeight/2 }}" textsize=".7" z-index="{{ zIndex + 2}}" text="Dont annoy me again!" />

                <!-- Close Button -->
                <label scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" id="button_close" style="CardButtonMediumWide" hidden="0" text="{{ buttonText }}" z-index="20" pos="{{ w/2 }} 0" halign="center" size="0 {{ buttonBarHeight/2 }}" />
            </frame>
        </MotdWindow>
        <script>
            <!--
            *** OnMouseClick ***
            ***
            if(Event.Control.ControlId == "checkbox" || Event.Control.ControlId == "checkboxText") {
                if(checkboxChecked) {
                    (Page.GetFirstChild("checkbox") as CMlLabel).Value = "";
                }
                else {
                    (Page.GetFirstChild("checkbox") as CMlLabel).Value = "";
                }
                checkboxChecked = !checkboxChecked;
            } else if(Event.Control.ControlId == "button_close") {
                TriggerPageAction("MotdManialinkController/Close/"^checkboxChecked);
            }
            ***
            
            main() {
                declare Boolean checkboxChecked = False;
                declare Text checkBoxText = (Page.GetFirstChild("checkbox") as CMlLabel).Value;
                if(checkBoxText == "")
                    checkboxChecked = False;
                else
                    checkboxChecked = True;
                    
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