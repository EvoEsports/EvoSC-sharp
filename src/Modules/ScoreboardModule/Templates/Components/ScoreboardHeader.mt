<component>
    <import component="ScoreboardModule.Components.Header.HeaderBackground" as="Background"/>
    <import component="ScoreboardModule.Components.Header.HeaderContent" as="Content"/>

    <property type="int" name="maxPlayers" default="0"/>
    <property type="int" name="pointsLimit" default="0"/>
    <property type="int" name="roundsPerMap" default="0"/>
    <property type="double" name="width"/>
    <property type="double" name="height"/>

    <template>
        <Background width="{{ width }}" height="{{ height }}"/>
        <Content 
                width="{{ width }}" 
                height="{{ height }}"
        />
    </template>
</component>
