<component>
    <import component="EvoSC.Controls.Dropdown" as="Dropdown" />
    <import component="EvoSC.Controls.Button" as="Button" />
    <import component="EvoSC.Controls.IconButton" as="IconButton" />
    <import component="EvoSC.Controls.TextInput" as="TextInput" />
    
    <template>
        <!-- <Dropdown text="Dropdown" id="mybtn" x="10" y="20">
            <Button text="Normal" id="myAction1" y="0" />
            <Button text="Secondary" id="myAction2" y="-5" type="secondary" />
            <Button text="Disabled" id="myAction3" y="-10" disabled="true" />
            <IconButton icon="" text="Icon" id="myAction4" y="-15" />
        </Dropdown> -->
        
        <TextInput name="myinput" value="something" />
    </template>

    <script resource="EvoSC.Scripts.UIScripts" />
</component>
