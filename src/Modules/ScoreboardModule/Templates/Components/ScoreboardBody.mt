<component>
    <import component="ScoreboardModule.Components.Body.Legend" as="Legend"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="y" default="0.0"/>
    <property type="double" name="rowSpacing" default="0.25"/>
    <property type="double" name="columnSpacing" default="2.0"/>
    <property type="double" name="legendHeight" default="4.0"/>
    <property type="double" name="flagWidth" default="4.0"/>
    <property type="double" name="clubTagWidth" default="4.0"/>

    <template>
        <frame pos="0 {{ y }}">
            <Legend width="{{ width }}"
                    height="{{ legendHeight }}"
                    flagWidth="{{ flagWidth }}"
                    clubTagWidth="{{ clubTagWidth }}"
                    columnSpacing="{{ columnSpacing }}"
            />
        </frame>
    </template>
</component>
