<component>
    <property type="double" name="w" default="140"/>
    <property type="double" name="h" default="80"/>
    <property type="double" name="radius" default="3.0"/>
    <property type="double" name="headerHeight" default="14.0"/>

    <template>
        <!-- Top bar -->
        <quad pos="{{ radius }} 0" 
              size="{{ w - radius * 2.0 }} {{ radius }}" 
              bgcolor="{{ Theme.ScoreboardModule_BackgroundBox_BgHeader }}"
              opacity="0.99"/>

        <!-- Middle part -->
        <quad pos="0 {{ -headerHeight }}" 
              size="{{ w }} {{ h + radius }}"
              bgcolor="{{ Theme.ScoreboardModule_BackgroundBox_BgList }}" 
              opacity="0.1"
        />
    </template>
</component>