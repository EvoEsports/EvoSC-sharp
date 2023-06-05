<component>
    <using namespace="EvoSC.Common.Interfaces.Models"/>
    <property type="IMap" name="map"/>
    <property type="string" name="country"/>
    <template>
        <frame pos="158 88">
            <label pos="-11 -3" z-index="0" size="60 5" text="$s{{map.Name}}" textfont="GameFontSemiBold" halign="right"
                   valign="center2" textsize="3"/>
            <label id="" pos="-11 -7.5" z-index="0" size="60 5" text="$s{{map.Author.NickName}}" halign="right"
                   textfont="GameFontRegular" valign="center2" textsize="2"/>
            <quad pos="0 -5" image="file://Media/Flags/{{country}}.dds" z-index="0" size="10 10" bgcolor="0000" opacity="1"
                  halign="right" valign="center" keepratio="Fit" />
        </frame>
    </template>
</component>