<!--
A hidden entry used to add entry value to custom components for form actions.
Can also be used for other hidden input values.
This out of bounds and does not show for users.
-->
<component>
    
    <!-- The name of the entry. -->
    <property type="string" name="name" />
    
    <!-- The initial value of the entry. -->
    <property type="string" name="value" default="" />
    
    <!-- The ID of the entry. -->
    <property type="string" name="id" default="" />
    
    <!-- Class to use for the entry. -->
    <property type="string" name="className" default="" />
    
    <template>
        <entry
                pos="500 500"
                size="0 0"
                opacity="0"
                z-index="-1000"
                
                default="{{ value }}"
                name="{{ name }}"
                id="{{ id }}"
                class="{{ className }}"
        />
    </template>
</component>
