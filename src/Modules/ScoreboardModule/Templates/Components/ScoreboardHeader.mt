<component>
    <import component="ScoreboardModule.Components.Header.HeaderBackground" as="HeaderBackground"/>
    <import component="ScoreboardModule.Components.Header.HeaderContent" as="HeaderContent"/>

    <property type="int" name="maxPlayers" default="0"/>
    <property type="int" name="pointsLimit" default="0"/>
    <property type="int" name="roundsPerMap" default="0"/>
    <property type="double" name="width"/>
    <property type="double" name="height"/>

    <template>
        <HeaderBackground
                width="{{ width }}"
                height="{{ height }}"
        />
        <HeaderContent
                width="{{ width }}"
                height="{{ height }}"
        />
    </template>
</component>
