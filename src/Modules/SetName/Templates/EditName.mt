<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <import component="EvoSC.FormEntry" as="FormEntry" />
    <import component="EvoSC.FormSubmit" as="FormSubmit" />

    <property type="FormValidationResult" name="Validation" />
    <property type="string" name="Nickname" />

    <template>
        <frame pos="0 0" valign="center" halign="center">
            <quad pos="0 0" size="50 50" bgcolor="000000" opacity="0.5" />
            <FormEntry
                    validationResults='{{ Validation?.GetResult("Nickname") }}'
                    value='{{ Nickname }}'
                    name="Nickname"
                    label="Nickname:"
                    w="30"
            />
            <FormSubmit x="18" y="-21" text="Login" action="SetName/EditName" />
        </frame>
    </template>
</component>
