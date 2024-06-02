<component>
    <import component="EvoSC.Containers.Window" as="Window" />
    <import component="EvoSC.Style.UIStyle" as="UIStyle" />
    <import component="EvoSC.Controls.Button" as="Button" />
    
    <property type="string" name="text" />
  
    <property type="double" name="titleBarHeight" default="6.0"/>
    
    <property type="double" name="buttonBarHeight" default="7.0"/>

    <property type="int" name="zIndex" default="1" />
    <property type="double" name="w" default="160" />
    <property type="double" name="h" default="90" />
    
    <template>
        <UIStyle />
        <Window width="{{ w }}" height="{{ h }}" x="{{ -w/2 }}" y="{{ h/2 }}" title="Edit Message of the Day">
            <textedit class="text" name="Text" default="{{ text }}" autonewline="1" z-index="{{ zIndex + 1 }}" pos="0 0" size="{{ w }} {{ h - (buttonBarHeight + titleBarHeight*2) }}" />

            <frame pos="0 {{ -h + buttonBarHeight*2 + 2 }}" size="{{ w }} {{ buttonBarHeight+2 }}" z-index="{{ zIndex + 1 }}">
                <Button text="Save" id="btnSave" action="MotdEditManialinkController/Save" x="{{ w/2-19 }}" width="18" />
                <Button text="Close" id="btnClose" width="18" x="{{ w/2+1 }}" type="secondary" />
            </frame>
        </Window>
    </template>

    <script>
        <!--
        *** OnMouseClick ***
        ***
        if (Event.Control.ControlId == "button_save") {
            TriggerPageAction("MotdEditManialinkController/Save/"^(Page.GetFirstChild("textedit_text") as CMlTextEdit).Value);
            CloseWindow("evosc-window");
        } else if (Event.Control.ControlId == "btnClose") {
            CloseWindow("evosc-window");
        }
        ***
        -->
    </script>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>