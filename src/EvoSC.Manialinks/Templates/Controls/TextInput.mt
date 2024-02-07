<!--
Basic text input control.
-->
<component>
    
    <!-- The name of the control. -->
    <property type="string" name="name" />
    
    <!-- The X position of the control. -->
    <property type="double" name="x" default="0.0"/>
    
    <!-- The Y position of the control. -->
    <property type="double" name="y" default="0.0"/>
    
    <!-- Whether to hide the input or not. -->
    <property type="bool" name="isPassword" default="false"/>
    
    <!-- The value type of the text input. -->
    <property type="string" name="valueType" default="Ml_String"/>
    
    <!-- The width of the control. -->
    <property type="double" name="width" default="25.0"/>
    
    <!-- The height of the control. -->
    <property type="double" name="height" default="5.0"/>
    
    <!-- Initial value for the control. -->
    <property type="string" name="value" default="" />
    
    <!-- Whether to automatically select it's content when it is focused. -->
    <property type="bool" name="autoSelect" default="false" />
    
    <!-- The maximum number of characters that can be added. -->
    <property type="int" name="maxLength" default="255" />
  
    <property type="string" name="placeholder" default="" />
    
    <template>
        <frame pos="{{ x }} {{ y }}" id="{{ name }}">
            <quad 
                    class="textinput-outline-default"
                    size="{{ width }} {{ height }}"
                    z-index="0"
            />
            <quad
                    class="textinput-default"
                    size="{{ width-0.2 }} {{ height-0.2 }}"
                    pos="0.1 -0.1"
                    scriptevents="1"
                    z-index="0"
            />
            <entry
                    class="textinput-default textinput-entry"
                    scriptevents="1"
                    size="{{ width-2 }} {{ height }}"
                    valuetype="{{ valueType }}"
                    textformat='{{ isPassword ? "Password" : "Basic" }}'
                    name="{{ name }}"
                    data-id="{{ name }}"
                    default="{{ value }}"
                    selecttext="{{ autoSelect }}"
                    maxlen="{{ maxLength }}"
                    valign="center"
                    pos="{{ 1 }} {{ -height/2 }}"
                    z-index="0"
            />
            <frame
                    pos="0.1 -0.1"
                    z-index="1" 
                    size="{{ width-0.2 }} {{ height-0.2 }}"
                    if='placeholder != ""'
            >
              <quad 
                      size="{{ width-0.2 }} {{ height-0.2 }}"
                      bgcolor="{{ Theme.UI_TextField_Default_Bg }}"
              />
              <label
                      text="$i{{ placeholder }}"
                      valign="center"
                      class="textinput-placeholder-default"
                      pos="{{ (height-3)/2 }} {{ -(height-0.15)/2 }}"
              />
            </frame>
        </frame>
    </template>

  <script resource="EvoSC.Scripts.TextInput" once="true" />
</component>
