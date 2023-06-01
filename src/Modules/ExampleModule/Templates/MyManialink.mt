<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <import component="EvoSC.FormEntry" as="FormEntry" />
    <import component="EvoSC.FormSubmit" as="FormSubmit" />
    
    <property type="FormValidationResult" name="Validation" />
    <property type="int?" name="Username" />
    <property type="string" name="Password" />
    
    <template>
        <frame pos="0 0">
            <quad pos="0 0" size="30 26" bgcolor="000000" opacity="0.5" />
            <FormEntry
                    validationResults='{{ Validation?.GetResult("Username") }}'
                    value='{{ Username }}'
                    name="Username"
                    label="Username:"
                    w="30"
            />
            <FormEntry
                    validationResults='{{ Validation?.GetResult("Password") }}'
                    value='{{ Password }}'
                    name="Password"
                    label="Password:"
                    w="30"
                    y="-10"
                    isPassword="true"
            />
            <FormSubmit x="18" y="-21" text="Login" action="ExampleManialink/HandleAction" />
        </frame>
    </template>
</component>
