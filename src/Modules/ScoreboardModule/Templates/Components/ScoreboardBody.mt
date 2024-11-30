<component>
    <import component="ScoreboardModule.Components.Body.Legend" as="Legend"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="y" default="0f"/>
    <property type="double" name="rowSpacing" default="0.25"/>
    <property type="double" name="columnSpacing" default="2f"/>
    <property type="double" name="legendHeight" default="4f"/>
    <property type="double" name="flagWidth" default="4f"/>
    <property type="double" name="clubTagWidth" default="4f"/>

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
