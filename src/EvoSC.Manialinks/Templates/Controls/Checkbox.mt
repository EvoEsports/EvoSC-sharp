<component>
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    <import component="EvoSC.Theme" as="Theme" />

    <property type="string" name="id" />
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    <property type="bool" name="isChecked" default="false" />
    <property type="string" name="text" default="" />
    
    <template>
        <Theme />
        <frame pos="{{ x }} {{ y }}" class="evosc-checkbox-frame" id="{{ id }}">
            <quad scriptevents="1" class="checkbox-outline-default evosc-checkbox" data-id="{{ id }}" size="0.1 3" pos="0 0" />
            <quad scriptevents="1" class="checkbox-outline-default evosc-checkbox" data-id="{{ id }}" size="0.1 3" pos="2.9 0" />
            <quad scriptevents="1" class="checkbox-outline-default evosc-checkbox" data-id="{{ id }}" size="3 0.1" pos="0 0" />
            <quad scriptevents="1" class="checkbox-outline-default evosc-checkbox" data-id="{{ id }}" size="3 0.1" pos="0 -2.9" />
            <quad
                    class="checkbox-default evosc-checkbox"
                    data-id="{{ id }}"
                    size="2.8 2.8"
                    pos="0.1 -0.1"
                    scriptevents="1"
                    opacity="{{ isChecked ? 1 : 0 }}"
                    bgcolorfocus='{{ isChecked ? "ff0058" : "00000000" }}'
            />
            <label 
                    class="checkbox-default evosc-checkbox"
                    data-id="{{ id }}"
                    text="{{ text }}"
                    height="3"
                    valign="center"
                    pos="5 -1.2"
                    scriptevents="1"
            />
            <label
                    class="checkbox-default evosc-checkbox"
                    data-id="{{ id }}"
                    textsize="1"
                    text='{{ isChecked ? "" : "" }}'
                    textcolor="E8E8EA"
                    height="3"
                    size="3 3"
                    halign="center"
                    valign="center"
                    pos="1.5 -1.2"
                    scriptevents="1"
            />
            <HiddenEntry
                    name="{{ id }}"
                    value="{{ isChecked }}"
            />
        </frame>
    </template>
    
    <script resource="EvoSC.Scripts.Checkbox" once="true" />
</component>