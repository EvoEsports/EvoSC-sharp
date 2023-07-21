<component>
    <import component="EvoSC.Dropdown" as="Dropdown" />
    <import component="EvoSC.Button" as="Button" />
    <import component="EvoSC.IconButton" as="IconButton" />
    
    <template>
        <quad bgcolor="0000ff" size="1 1" halign="center" valign="center" />
        
        <Dropdown text="Dropdown" id="mybtn" x="10" y="20">
            <Button text="Normal" id="myAction1" y="0" />
            <Button text="Secondary" id="myAction2" y="-5" type="secondary" />
            <Button text="Disabled" id="myAction3" y="-10" disabled="true" />
            <IconButton icon="" text="Icon" id="myAction4" y="-15" />
        </Dropdown>
    </template>

    <script resource="EvoSC.Scripts.UIScripts" />
</component>
