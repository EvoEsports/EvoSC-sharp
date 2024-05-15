<!--
A radio button that can be grouped with other radio buttons to form a selective list.
-->
<component>
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />
    <import component="EvoSC.Drawing.Circle" as="Circle" />
    
    <!-- The ID of the radio button. -->
    <property type="string" name="id" />
    
    <!-- The group this radio button is part of. The default group name is 'evosc-default'. -->
    <property type="string" name="group" default="evosc-default" />
    
    <!-- The initial checked state of the radio button. -->
    <property type="bool" name="isChecked" default="false" />
    
    <!-- The text to display alongside the radio button. -->
    <property type="string" name="text" default="" />
    
    <!-- The X position of the radio button. -->
    <property type="double" name="x" default="0.0" />
    
    <!-- The Y position of the radio button. -->
    <property type="double" name="y" default="0.0" />
    
    <template>
      <frame
              class="evosc-radiobutton-frame {{ group }}-radiogroup"
              id="{{ id }}"
              pos="{{ x }} {{ y }}"
              data-initvalue="{{ isChecked }}"
              data-group="{{ group }}"
      >
        <Circle bgColor="{{ Theme.UI_SurfaceBgPrimary }}" 
                radius="2.5" 
                scriptEvents="true" 
                className="evosc-radiobutton" 
                dataId="{{ id }}" />
        
        <Circle bgColor="{{ Theme.UI_SurfaceBgSecondary }}" 
                radius="1.5"
                x="1"
                y="-1" 
                scriptEvents="true"
                className="evosc-radiobutton"
                dataId="{{ id }}" 
                hidden="{{ !isChecked }}"
        />

        <label
                text="{{ text.ToUpper() }}"
                class="radiobutton-default evosc-radiobutton text-primary"
                data-group="{{ group }}"
                data-id="{{ id }}"
                scriptevents="1"
                pos="6.5 -2.5"
                valign="center"
        />
        
        <HiddenEntry
                name="{{ id }}"
                value="{{ isChecked }}"
        />
      </frame>
      
        <!-- <frame 
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
                    textsize="{{ Theme.UI_RadioButton_Default_CheckSize }}"
                    pos="0 -1.2"
                    valign="center"
                    textcolor="{{ Theme.UI_BgPrimary }}"
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
        </frame>-->
    </template>
    
    <script resource="EvoSC.Scripts.RadioButton" once="true" />
</component>