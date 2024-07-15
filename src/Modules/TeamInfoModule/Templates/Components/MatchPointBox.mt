<component>
    <property type="double" name="width" default="24.0"/>
    <property type="double" name="height" default="4"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="halign" default="left"/>

    <template>
        <frame pos='{{ x - (halign=="right" ? width : 0) }} {{ y + height + 0.3 }}'>
            <quad pos='{{ (halign=="right" ? width - 0.7 : 0) }}'
                  size="0.7 {{ height }}"
                  bgcolor="{{ Theme.UI_AccentPrimary }}"
            />
            <quad pos='{{ (halign=="right" ? 0 : 0.7) }} 0'
                  size="{{ width - 0.7 }} {{ height }}"
                  bgcolor="{{ Theme.UI_BgPrimary }}dd"
            />
            <label pos='{{ (halign=="right" ? width-2 : 2) }} {{ height / -2.0 + 0.25 }}'
                   text="MATCH POINT"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize }}"
                   valign="center"
                   halign="{{ halign }}"
            />
        </frame>
    </template>
</component>