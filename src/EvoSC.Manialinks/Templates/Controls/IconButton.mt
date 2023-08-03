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
    <property type="string" name="type" default="default" />
    
    <!-- The action to call when the button is clicked. This disables event scripts. -->
    <property type="string" name="action" default="" />
    
    <!-- Whether the button is disabled or not. If disabled, the button wont fire events. -->
    <property type="bool" name="disabled" default="false" />

    <!-- The position of the icon relative to the button text. -->
    <property type="string" name="iconPos" default="left" />

    <template>
        <Button 
                text='{{ (iconPos == "right" ? $"{text}{(text == "" ? "" : " ")}{icon}" : $"{icon}{(text == "" ? "" : " ")}{text}") }}'
                x="{{ x }}"
                y="{{ y }}"
                width="{{ width }}"
                height="{{ height }}"
                id="{{ id }}"
                type="{{ type }}"
                action="{{ action }}"
                disabled="{{ disabled }}"
        />
    </template>
</component>
