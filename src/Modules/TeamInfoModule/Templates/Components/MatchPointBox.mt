<component>
    <property type="double" name="width" default="24.0"/>
    <property type="double" name="height" default="4"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="halign" default="left"/>
    <property type="string" name="text" default="Map Point"/>

    <template>
        <frame pos='{{ x - (halign=="right" ? width : 0) }} {{ y + height }}'>
            <quad pos='{{ (halign=="right" ? width - 0.7 : 0) }}'
                  size="0.7 {{ height }}"
                  bgcolor="{{ Theme.UI_AccentPrimary }}"
            />
            <quad pos='{{ (halign=="right" ? 0 : 0.7) }} 0'
                  size="{{ width - 0.7 }} {{ height }}"
                  bgcolor="{{ Theme.UI_BgPrimary }}dd"
            />
            <label pos='{{ (halign=="right" ? width-2 : 2) }} {{ height / -2.0 + 0.25 }}'
                   text="{{ text.ToUpper() }}"
                   class="text-primary"
                   valign="center"
                   halign="{{ halign }}"
            />
        </frame>
    </template>
</component>