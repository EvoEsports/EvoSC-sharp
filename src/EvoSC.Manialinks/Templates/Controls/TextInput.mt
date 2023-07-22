<component>
    <import component="EvoSC.Theme" as="Theme" />

    <property type="string" name="name" />
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="bool" name="isPassword" default="false"/>
    <property type="string" name="valueType" default="Ml_String"/>
    <property type="double" name="width" default="25.0"/>
    <property type="double" name="height" default="5.0"/>
    <property type="string" name="value" default="" />
    <property type="bool" name="autoSelect" default="false" />
    <property type="int" name="maxLength" default="255" />
    
    <template>
        <Theme />
        <frame pos="{{ x }} {{ y }}">
            <quad 
                    class="textinput-outline-default"
                    size="{{ width }} {{ height }}"
            />
            <quad 
                    class="textinput-default"
                    size="{{ width-0.2 }} {{ height-0.2 }}"
                    pos="0.1 -0.1"
                    scriptevents="1"
            />
            <entry
                    class="textinput-default"
                    scriptevents="1"
                    size="{{ width-2 }} {{ height }}"
                    valuetype="{{ valueType }}"
                    textformat='{{ isPassword ? "Password" : "Basic" }}'
                    name="{{ name }}"
                    default="{{ value }}"
                    selecttext="{{ autoSelect }}"
                    maxlen="{{ maxLength }}"
                    valign="center"
                    pos="{{ 1 }} {{ -height/2 }}"
            />
        </frame>
    </template>
</component>
