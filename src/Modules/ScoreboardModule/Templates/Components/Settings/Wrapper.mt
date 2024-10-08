<component>
    <property type="double" name="h" />
    <property type="double" name="padding" />
    
    <template>
        <frame pos="{{ padding }} {{ h + padding }}">
            <frame pos="{{ padding }} {{ -padding }}">
                <slot />
            </frame>
        </frame>
    </template>
</component>