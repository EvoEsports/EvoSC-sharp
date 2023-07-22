<component>
    <import component="EvoSC.Controls.Button" as="Button" />

    <property type="string" name="text" default=""/>
    <property type="double" name="x" default="0.0" />
    <property type="double" name="y" default="0.0" />
    <property type="double" name="width" default="17.0" />
    <property type="double" name="height" default="5.0" />
    <property type="string" name="id" />
    <property type="string" name="type" default="default" />
    <property type="string" name="action" default="" />
    <property type="bool" name="disabled" default="false" />

    <property type="string" name="icon" />
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
