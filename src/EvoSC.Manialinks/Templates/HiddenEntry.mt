<component>
    <property type="string" name="name" />
    <property type="string" name="value" default="" />
    <property type="string" name="id" default="" />
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
