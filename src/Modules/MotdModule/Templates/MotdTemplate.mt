<component>
    <import component="EvoSC.Window" as="Window" />
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
    <import component="EvoSC.Controls.Button" as="Button" />

    <property type="bool" name="isChecked" />
    <property type="string" name="text" />

    <property type="string" name="primaryColor" default="ff0058"/>
    <property type="string" name="backgroundColor" default="47495A"/>

    <property type="double" name="titleBarHeight" default="6.0"/>
    
    <property type="double" name="buttonBarHeight" default="8.0"/>
    <property type="string" name="buttonText" default="Close"/>

    <property type="string" name="checkboxUncheckedIcon" default="" />
    <property type="string" name="checkboxCheckedIcon" default="" />

    <property type="double" name="w" default="160" />
    <property type="double" name="h" default="90" />
    
    <template>
        <Theme />
        <Window height="{{ h }}" width="{{ w }}" x="{{ -w/2 }}" y="{{ h/2 }}" title="Message of the Day">
            <label class="text" autonewline="1" text="{{ text }}" pos="0 0" size="{{ w }} {{ h - (buttonBarHeight + titleBarHeight*2) }}" />
            <!-- ButtonBar -->
            <frame pos="0 {{ -h + buttonBarHeight*2 + 2 }}" size="{{ w }} {{ buttonBarHeight }}">
                <Checkbox id="chkDontShowAgain" text="Dont annoy me again!" x="0" y="-.5" isChecked="{{ isChecked }}" />
                
                <!-- Close Button -->
                <Button text="{{ buttonText }}" x="{{ (w-60)/2 }}" width="60" id="closeBtn" />
            </frame>
        </Window>
    </template>

    <script>
        <!--
            *** OnMouseClick ***
            ***
                log(Event.Control.ControlId);
                if (Event.Control.ControlId == "closeBtn" || Event.Control.HasClass("evosc-window-closebtn")) {
                    CloseWindow("evosc-window");
            
                    declare Checkbox <=> Page.MainFrame.GetFirstChild("chkDontShowAgain") as CMlFrame;
                    declare IsChecked for Checkbox = False;
                    TriggerPageAction("MotdManialinkController/Close/" ^ IsChecked);
                }
            ***
        -->
    </script>
    <script resource="EvoSC.Scripts.UIScripts" main="true" />
</component>