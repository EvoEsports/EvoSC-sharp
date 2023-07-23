<component>
    <import component="EvoSC.Theme" as="Theme" />
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    
    <property type="string" name="id" />
    <property type="string" name="group" default="evosc-default" />
    <property type="bool" name="isChecked" default="false" />
    <property type="string" name="text" default="" />
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    
    <template>
        <Theme />
        <frame 
                class="evosc-radiobutton-frame {{ group }}-radiogroup" 
                id="{{ id }}" 
                pos="{{ x }} {{ y }}"
                data-initvalue="{{ isChecked }}"
                data-group="{{ group }}"
        >
            <label
                    text='{{ isChecked ? "◉" : "○" }}'
                    class="evosc-radiobutton"
                    data-group="{{ group }}"
                    data-id="{{ id }}"
                    scriptevents="1"
                    textsize="2"
                    pos="0 -1.2"
                    valign="center"
                    textcolor="ff0058"
                    focusareacolor1="0000ffff"
                    focusareacolor2="0000ffff"
                    textfont="GameFontRegular"
            />
            
            <label
                    text="{{ text }}"
                    class="radiobutton-default evosc-radiobutton"
                    data-group="{{ group }}"
                    data-id="{{ id }}"
                    scriptevents="1"
                    pos="5 -1.2"
                    valign="center"
            />
            <HiddenEntry
                    name="{{ id }}"
                    value="{{ isChecked }}"
            />
        </frame>
    </template>
    
    <script resource="EvoSC.Scripts.RadioButton" once="true" />
</component>