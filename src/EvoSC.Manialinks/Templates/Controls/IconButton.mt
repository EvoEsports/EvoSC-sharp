<!--
    Shows a button with an icon.
-->
<component>
  <import component="EvoSC.Controls.Button" as="Button" />

  <!-- The ID of the button. -->
  <property type="string" name="id" />

  <!-- The icon to display in the button. -->
  <property type="string" name="icon" />
  
  <!-- The text to display in the button. -->
  <property type="string" name="text" default=""/>
  
  <!-- The X position of the button. -->
  <property type="double" name="x" default="0.0" />
  
  <!-- The Y position of the button. -->
  <property type="double" name="y" default="0.0" />
  
  <!-- The width of the button background. -->
  <property type="double" name="width" default="17.0" />
  
  <!-- The height of the button background. -->
  <property type="double" name="height" default="5.0" />
  
  <!-- The style type of the button, can be default or secondary. -->
  <property type="string" name="type" default="primary" />
  
  <!-- The action to call when the button is clicked. This disables event scripts. -->
  <property type="string" name="action" default="" />
  
  <!-- Whether the button is disabled or not. If disabled, the button wont fire events. -->
  <property type="bool" name="disabled" default="false" />

  <!-- Whether the button has text on it. -->
  <property type="bool" name="hasText" default="false" />

  <!-- Possible values: normal, round -->
  <property type="string" name="style" default="normal" />

  <!-- The position of the icon relative to the button text. -->
  <property type="string" name="iconPos" default="left" />

  <!-- Custom style -->
  <property type="string" name="className" default="evosc-iconbutton" />

  <!-- Custom data passed to the component -->
  <property type="string" name="data" default="" />

  <!-- Size of the button can be normal, small, big or custom. -->
  <property type="string" name="size" default="normal" />

  <!-- Whether the button is hidden or not -->
  <property type="bool" name="hidden" default="false" />

  <template>
    <Button 
            text='{{ hasText ? (iconPos == "right" ? $"{text}{(text == "" ? "" : " ")}{icon}" : $"{icon}{(text == "" ? "" : " ")}{text}") : icon }}'
            x="{{ x }}"
            y="{{ y }}"
            width='{{ hasText ? width : Theme.UI_Button_Size(size, height) }}'
            height="{{ Theme.UI_Button_Size(size, height) }}"
            id="{{ id }}"
            type="{{ type }}"
            action="{{ action }}"
            disabled="{{ disabled }}"
            style="{{ style }}"
            className="{{ className }}"
            data="{{ data }}"
            size="{{ size }}"
            hidden="{{ hidden }}"
    />
  </template>
</component>
