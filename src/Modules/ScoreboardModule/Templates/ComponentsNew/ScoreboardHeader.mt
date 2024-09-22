<component>
    <import component="ScoreboardModule.ComponentsNew.Header.HeaderBackground" as="Background"/>
    <import component="ScoreboardModule.ComponentsNew.Header.HeaderContent" as="Content"/>

    <property type="double" name="width"/>
    <property type="double" name="height"/>

    <template>
        <Background width="{{ width }}" height="{{ height }}"/>
        <Content width="{{ width }}" height="{{ height }}"/>
    </template>
</component>
