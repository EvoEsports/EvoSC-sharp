<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <import component="EvoSC.FormEntry" as="FormEntry" />
    
    <property type="FormValidationResult" name="Validation" />
    <property type="string" name="Username" />
    <property type="string" name="Password" />
    
    <template>
        <frame pos="0 0">
            <quad pos="0 0" size="30 26" bgcolor="000000" opacity="0.5" />
            <FormEntry
                    validationResult='{{ Validation?.GetResult("Username") }}'
                    value='{{ Username }}'
                    name="Username"
                    label="Username:"
                    w="30"
            />
            <FormEntry
                    validationResult='{{ Validation?.GetResult("Password") }}'
                    value='{{ Password }}'
                    name="Password"
                    label="Password:"
                    w="30"
                    y="-10"
            />
            <label pos="18 -21" text="Login" action="test/login" />
        </frame>
    </template>
</component>
