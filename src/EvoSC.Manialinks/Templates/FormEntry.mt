<component>
    <!--
        Generic input element for Manialink Forms that can also handle validation errors.
    -->

    <using namespace="EvoSC.Manialinks.Validation" />
    <using namespace="System.Linq" />

    <property type="IEnumerable<EntryValidationResult>?" name="validationResults" />
    <property type="string?" name="value"/>
    <property type="string" name="name"/>
    <property type="int" name="zIndex" default="0"/>
    <property type="string" name="label" default=""/>
    <property type="double" name="w" default="0.0"/>
    <property type="double" name="h" default="9.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="bool" name="isPassword" default="false"/>
    <property type="string" name="valueType" default="Ml_String"/>
    
    <template>
        <frame pos="{{ x }} {{ y }}" size="{{ w }} {{ h }}">
            <Label
                    text="{{ label }}"
                    x="0"
                    y="0"
                    textsize="1"
                    halign="left" valign="top"
            />
            <entry
                    pos="0 -3"
                    name="{{ name }}"
                    default='{{ value ?? "" }}'
                    size="{{ w }} 3"
                    textsize="1"
                    halign="left" valign="top"
                    textformat='{{ isPassword ? "Password" : "Basic" }}'
                    valuetype="{{ valueType }}"
            />
            <Label
                    text='$s$F00 {{ validationResults?.FirstOrDefault(v => v.IsInvalid)?.Message ?? "Invalid input." }}'
                    x="0"
                    y="-6"
                    textsize="0.5"
                    halign="left" valign="top"
                    if='validationResults?.Any(v => v.IsInvalid) ?? false'
            />
        </frame>
    </template>
</component>
