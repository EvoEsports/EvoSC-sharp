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
        
        <Window title="UI Demo" icon="" width="120" height="60" x="-60" y="30">
            <label text="Buttons:" class="text" />
            <frame pos="0 -4">
                <Button id="btnDefault" text="Default" />
                <Button id="btnDefaultDisabled" text="Default Disabled" disabled="true" width="25" y="-6" />
                <Button id="btnSecondary" text="Secondary" type="secondary" y="-12" />
                <Button id="btnSecondaryDisabled" text="Secondary Disabled" type="secondary" disabled="true" width="25" y="-18" />
                <IconButton id="btnIconLeft" text="Icon Button" width="25" y="-24" icon="" />
                <IconButton id="btnIconRight" text="Icon Button" width="25" y="-30" icon="" iconPos="right" />
            </frame>
            
            <label text="Dropdown:" class="text" pos="30 -1" />
            <frame pos="55 0" z-index="10">
                <Dropdown text="Chose Something" id="dropdownDemo" width="30">
                    <Button text="Action 1" id="dropdownBtn1" />
                    <Button text="Action 2" id="dropdownBtn1" y="-5" />
                    <Button text="Action 3" id="dropdownBtn1" y="-10" />
                </Dropdown>
            </frame>
            
            <frame pos="30 -6">
                <label text="Text Input:" class="text" pos="0 0" />
                <TextInput name="txtInputDemo" x="25" />
                <label text="Password Input:" class="text" pos="0 -7" />
                <TextInput name="txtInputDemoPassword" isPassword="true" y="-6" x="25" />
            </frame>

            <label text="Switch:" class="text" pos="0 -41" />
            <frame pos="15 -40">
                <Switch id="switchDemo" value="true" />
            </frame>

            <label text="Checkbox:" class="text" pos="30 -20" />
            <frame pos="30 -24">
                <Checkbox id="checkbox1" text="one" isChecked="true" />
                <Checkbox id="checkbox2" text="two" isChecked="true" y="-5" />
                <Checkbox id="checkbox3" text="three" y="-10" />
            </frame>

            <label text="Radio Buttons:" class="text" pos="50 -20" />
            <frame pos="50 -24">
                <RadioButton id="radioButton1" text="one" isChecked="true" />
                <RadioButton id="radioButton2" text="two" y="-5" />
                <RadioButton id="radioButton3" text="three" y="-10" />
            </frame>
            
            <Button id="btnShowAlert" text="Show Alert" x="88" />
            <Button id="btnHideAlert" text="Hide Alert" x="88" y="-6" />
        </Window>
        
        <Alert text="Hello there!" id="myAlert" y="70" />
    </template>
    
    <script>
        *** OnMouseClick ***
        ***
            log(Event.Control.ControlId);
            if (Event.Control.ControlId == "btnShowAlert") {
                ShowAlert("myAlert");
            } else if (Event.Control.ControlId == "btnHideAlert") {
                HideAlert("myAlert");
            }
        ***
    </script>
    
    <script resource="EvoSC.Scripts.UIScripts" />
</component>
