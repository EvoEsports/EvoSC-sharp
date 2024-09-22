<component>
    <import component="ScoreboardModule.ComponentsNew.Body.BodyBackground" as="BodyBackground"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>
    <property type="double" name="y" default="0.0"/>

    <template>
        <frame pos="0 {{ y }}">
            <BodyBackground width="{{ width }}" height="{{ height }}"/>
        </frame>
    </template>
</component>
