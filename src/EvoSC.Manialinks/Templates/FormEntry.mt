<component>
    <using namespace="EvoSC.Manialinks.Validation" />
    
    <property type="ValidationResult?" name="validationResult" />
    <property type="string?" name="value"/>
    <property type="string" name="name"/>
    <property type="int" name="zIndex"/>
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
                    text='$s$F00 {{ validationResult?.Message ?? "Invalid input." }}'
                    x="0"
                    y="-6"
                    textsize="1"
                    halign="left" valign="top"
                    if='validationResult?.IsInvalid ?? false'
            />
        </frame>
    </template>
</component>
