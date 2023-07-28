<!--
    Simple clickable button.
-->
<component>
    <!-- The ID of the button. -->
    <property type="string" name="id" />
    
    <!-- Text to display in the button -->
    <property type="string" name="text" />
    
    <!-- X position of the button. -->
    <property type="double" name="x" default="0.0" />
    
    <!-- Y position of the button. -->
    <property type="double" name="y" default="0.0" />
    
    <!-- Width of the button background. -->
    <property type="double" name="width" default="17.0" />
    
    <!-- Height of the button background. -->
    <property type="double" name="height" default="5.0" />
    
    <!-- The button style type, can be default or secondary. -->
    <property type="string" name="type" default="default" />
    
    <!-- The action to call when clicking the button. This disables script events. -->
    <property type="string" name="action" default="" />
    
    <!-- Whether the button is disabled or not. If disabled, the button wont fire events. -->
    <property type="bool" name="disabled" default="false" />
    
    <!-- Class to assign to the button. -->
    <property type="string" name="className" default="evosc-button" />
    
    <template>
        <frame id="{{ id }}-frame" pos="{{ x }} {{ y }}" class="{{ className }}-frame" data-disabled="{{ disabled }}">
            <frame if="!disabled">
                <quad
                        class='{{ type == "secondary" ? "btn-secondary" : "btn-default" }}'
                        size="{{ width }} {{ height }}"
                        scriptevents="1"
                />
                <label
                        size="{{ width }} {{ height }}"
                        class='{{ type == "secondary" ? "btn-secondary" : "btn-default" }} {{ className }}-btn'
                        text="{{ text }}"
                        scriptevents="1"
                        halign="center"
                        valign="center"
                        pos="{{ width/2 }} {{ -height/2 }}"
                        if='action.Equals("", StringComparison.Ordinal)'
                        data-id="{{ id }}"
                        id="{{ id }}"
                />
                <label
                        class='{{ type == "secondary" ? "btn-secondary" : "btn-default" }} {{ className }}-btn'
                        size="{{ width }} {{ height }}"
                        text="{{ text }}"
                        scriptevents="1"
                        action="{{ action }}"
                        halign="center"
                        valign="center"
                        pos="{{ width/2 }} {{ -height/2 }}"
                        if='!action.Equals("", StringComparison.Ordinal)'
                        data-id="{{ id }}"
                        id="{{ id }}"
                />
            </frame>
            <frame if="disabled">
                <quad
                        size="{{ width }} {{ height }}"
                        class='{{ type == "secondary" ? "btn-secondary-disabled" : "btn-disabled" }}'
                />
                <label
                        id="{{ id }}"
                        class='{{ type == "secondary" ? "btn-secondary-disabled" : "btn-disabled" }}'
                        size="{{ width }} {{ height }}"
                        text="{{ text }}"
                        halign="center"
                        valign="center"
                        pos="{{ width/2 }} {{ -height/2 }}"
                />
            </frame>
        </frame>
    </template>
</component>
