<component>
    <!--
    <script resource="MotdModule.Templates.Scripts.MotdWindow" />
    -->
    <property type="int" name="zIndex" default="0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="w"/>
    <property type="double" name="h"/>
    <property type="string" name="title" default="Test"/>
    <property type="double" name="titleBarHeight" default="6.0"/>
    <property type="double" name="buttonBarHeight" default="7.0"/>
    <property type="string" name="buttonText" default="Close"/>
    <property type="string" name="buttonAction" default="test"/>
    <property type="string" name="halign" default="center"/>
    <property type="string" name="valign" default="center"/>
    <property type="double" name="padding" default="2"/>

    <property type="string" name="primaryColor" default="ff0058"/>
    <property type="string" name="backgroundColor" default="47495A"/>
    
    <property type="bool" name="isChecked" default="false" /> 

    <property type="string" name="checkboxUncheckedIcon" default="" />
    <property type="string" name="checkboxCheckedIcon" default="" />

    <template>
        <frame id="window" pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}" valign="{{ valign }}" halign="{{ halign }}">
            <!-- Title Bar -->
            <quad pos="0 {{ h/2-titleBarHeight/2 }}" size="{{ w }} {{ titleBarHeight }}" valign="center" halign="center" bgcolor="{{ primaryColor }}" opacity="1" z-index="{{ zIndex }}"/>
            <!-- Title Bar Close Button -->
            <label id="window_close_button" scriptevents="1" pos="{{ w/2-2 }} {{ h/2-titleBarHeight/2 }}" textsize=".6" halign="right" valign="center2" text="❌" z-index="{{ zIndex + 1 }}"/>
            <!-- Title Bar Title -->
            <label pos="{{ (w*-1)/2+padding }} {{ h/2-padding }}" text="{{ title }}" textsize=".8" z-index="{{ zIndex + 1 }}" />
            <!-- Content Background -->
            <quad pos="0 0" size="{{ w }} {{ h }}" valign="center" halign="center" bgcolor="{{ backgroundColor }}" opacity="1" />
            <!-- Content -->
            <frame pos="{{ (w*-1)/2+padding }} {{ (h/2-titleBarHeight)-padding }}" size="{{ w - 4.0 }} {{ h - titleBarHeight - 4.0 }}" z-index="{{ zIndex + 1 }}">
                <slot/>
                
                <label text="Niggo pupst unter der Dusche!" z-index="{{ zIndex + 1 }}" pos="0 0" size="100 100" />
                
                <!-- Dont show again Checkbox -->
                <frame pos="0 {{ -h + buttonBarHeight*2 }}" size="{{ w }} {{ buttonBarHeight }}" z-index="{{ zIndex + 1 }}">
                    <label id="checkbox" action="MotdManialinkController/ToggleCheckbox" scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" pos="0 0.5" size="{{ buttonBarHeight/2 }} {{ buttonBarHeight/2 }}" z-index="{{ zIndex + 2}}" text="{{ (isChecked) ? checkboxCheckedIcon : checkboxUncheckedIcon }}" textcolor="{{ primaryColor }}"/>
                    <label id="checkboxText" action="MotdManialinkController/ToggleCheckbox" scriptevents="1" focusareacolor1="{{ backgroundColor }}" focusareacolor2="{{ backgroundColor }}" pos="6 -1" size="{{ w/4 }} {{ buttonBarHeight/2 }}" textsize=".7" z-index="{{ zIndex + 2}}" text="Dont annoy me again!" />
                </frame>
            </frame>
            <!-- ButtonBar -->
            <frame pos="{{ -(w/2) }} {{ (h/2)*-1+buttonBarHeight }}" size="{{ w }} {{ buttonBarHeight }}" z-index="{{ zIndex + 1 }}">
                <label style="CardButtonMediumWide" hidden="0" text="{{ buttonText }}" action="{{ buttonAction }}" z-index="20" pos="{{ w/2 }} 0" halign="center" size="0 {{ buttonBarHeight/2 }}" />
                
                <!--
                for later use
                <label style="CardButtonMedium" hidden="0" text="ActionButton" z-index="20" pos="{{ w }} 0" halign="right" size="10 {{ buttonBarHeight/2 }}" />
                -->
            </frame>
        </frame>
        <script>
            <!--
            *** OnMouseClick ***
            ***
            if(Event.Control.ControlId == "window_close_button"){
                Page.GetFirstChild("window").Hide();
                return;
            }
            else if(Event.Control.ControlId == "checkbox" || Event.Control.ControlId == "checkboxText") {
                log("test3");
                if(checkboxChecked) {
                    log("testchecked");
                    (Page.GetFirstChild("checkbox") as CMlLabel).Value = "";
                }
                else {
                    log("testunchecked");
                    (Page.GetFirstChild("checkbox") as CMlLabel).Value = "";
                }
                log("test4");
                checkboxChecked = !checkboxChecked;
            }
            ***
            Void _nothing() {
            }
            
            main() {
                declare Boolean checkboxChecked = False;
            
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
            }-->
        </script>
    </template>
</component>
