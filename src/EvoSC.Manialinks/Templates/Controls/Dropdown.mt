<component>
    <import component="EvoSC.Controls.Button" as="Button" />
    
    <property type="string" name="text" default="Dropdown" />
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    <property type="double" name="width" default="20.0" />
    <property type="double" name="height" default="5.0" />
    <property type="string" name="type" default="default" />
    <property type="string" name="id" />
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
        <frame id="{{ id }}-btn-panel" pos="{{ x }} {{ y-height }}">
            <slot />
        </frame>
    </template>
    
    <script resource="EvoSC.Scripts.Dropdown" once="true"/>
</component>
