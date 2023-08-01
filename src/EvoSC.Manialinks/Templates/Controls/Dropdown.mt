<!--
Toggles a panel below the button that can contain any components.
-->
<component>
    <import component="EvoSC.Controls.Button" as="Button" />

    <!-- The ID of the dropdown. -->
    <property type="string" name="id" />
    
    <!-- The text to display in the button. -->
    <property type="string" name="text" default="Dropdown" />
    
    <!-- X position of the button. -->
    <property type="double" name="x" default="0.0" />
    
    <!-- Y position of the button. -->
    <property type="double" name="y" default="0.0" />
    
    <!-- Width of the button background. -->
    <property type="double" name="width" default="20.0" />
    
    <!-- Height of the button background. -->
    <property type="double" name="height" default="5.0" />
    
    <!-- Style type of the button, can be default or secondary. -->
    <property type="string" name="type" default="default" />

    <!-- Whether the dropdown button is disabled or not. If disabled, the panel wont open. -->
    <property type="bool" name="disabled" default="false" />
    
    <template>
        <Button
                text='{{ text }} '
                x="{{ x }}"
                y="{{ y }}"
                width="{{ width }}"
                height="{{ height }}"
                id="{{ id }}-btn"
                type="{{ type }}"
                disabled="{{ disabled }}"
                className="evosc-dropdown"
        />
        <frame 
                id="{{ id }}-btn-slotpanel" 
                pos="{{ x }} {{ y-height }}" 
                class="evosc-dropdown-slotpanel"
                data-id="{{ id }}"
        >
            <slot />
        </frame>
    </template>
    
    <script resource="EvoSC.Scripts.Dropdown" once="true"/>
</component>
