<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    <import component="EvoSC.FormEntry" as="FormEntry" />
    <import component="EvoSC.FormSubmit" as="FormSubmit" />

    <property type="FormValidationResult" name="Validation" />
    <property type="string" name="Nickname" />

    <template>
        <frame pos="-25 9" valign="center" halign="center">
            <quad pos="0 0" size="50 18" bgcolor="000000" opacity="0.5" />
            <quad pos="0 0" size="50 5" bgcolor="000000" opacity="0.5" />
            <label pos="1 -1" text="Edit your nickname" textsize="1.5" />
            <label pos="47 -1" text="X" action="SetName/Cancel" textsize="1.5" />
            <FormEntry
                    validationResults='{{ Validation?.GetResult("Nickname") }}'
                    value='{{ Nickname }}'
                    name="Nickname"
                    label="Nickname:"
                    w="48"
                    x="1"
                    y="-6"
            />
            <FormSubmit x="25" y="-14" text="Submit" action="SetName/EditName" />
            <FormSubmit x="37" y="-14" text="$f00Cancel" action="SetName/Cancel" />
        </frame>
    </template>
</component>
