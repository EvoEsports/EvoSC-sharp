<component>
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
    <import component="EvoSC.Controls.Button" as="Button" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
    <import component="EvoSC.Controls.TextInput" as="TextInput" />
    <import component="EvoSC.Controls.Switch" as="Switch" />
    <import component="EvoSC.Controls.Checkbox" as="Checkbox" />
    <import component="EvoSC.Controls.RadioButton" as="RadioButton" />
    <import component="EvoSC.Controls.Alert" as="Alert" />
    <import component="EvoSC.Window" as="Window" />
    
    <template>
        <Theme />
        <!-- <Dropdown text="Dropdown" id="myDropdown" x="10" y="20">
            <Button text="Normal" id="myAction1" y="0" />
            <Button text="Secondary" id="myAction2" y="-5" type="secondary" />
            <Button text="Disabled" id="myAction3" y="-10" disabled="true" />
            <IconButton icon="" text="Icon" id="myAction4" y="-15" />
        </Dropdown> -->
        
        <!-- <TextInput name="myinput" value="something" /> -->
        
        <!-- <Switch value="false" id="switch1" />
        <Switch value="false" y="-6" id="switch2" /> -->
        
        <!-- <Checkbox id="mycheck" text="Check this!" />
        <Checkbox id="mycheck2" text="Check this!" isChecked="true" y="-4" /> -->
        
        
        <!-- <RadioButton id="radio1" text="Group 1 Btn 1" group="group1" />
        <RadioButton id="radio2" text="Group 1 Btn 2" group="group1" y="-4" />
        <RadioButton id="radio3" text="Group 2 Btn 1" group="group2" y="-18" />
        <RadioButton id="radio4" text="Group 2 Btn 2" group="group2" y="-22" />
        <RadioButton id="radio5" text="Group 2 Btn 3" group="group2" y="-26" /> -->
        
        <!-- <Window style="secondary" hasTitlebar="false">
            <label text="test" />
        </Window> -->
        
        <Alert id="myAlert" />
        <Button id="showAlert" text="Toggle Alert" width="20" y="-10" />
    </template>
    
    <script>
        *** OnInitialization ***
        ***
            declare alertShown = False;
        ***
        
        *** OnMouseClick ***
        ***
            if (Event.Control.ControlId == "showAlert") {
                if (alertShown) {
                    HideAlert("myAlert");
                } else {
                    ShowAlert("myAlert");
                }
            
                alertShown = !alertShown;
            }
        ***
    </script>
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
