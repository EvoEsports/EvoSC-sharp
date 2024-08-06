<component>
    <property type="double" name="width" default="20.0"/>
    <property type="double" name="height" default="10.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="int" name="roundNumber" default="-1"/>

    <template>
        <frame pos="{{ x }} {{ y }}" halign="center">
            <quad size="{{ width }} {{ height }}"
                  bgcolor="{{ Theme.UI_TextPrimary }}"
                  halign="center"
            />

            <label pos="0 -1.25"
                   text="{{ roundNumber }}"
                   textcolor="333"
                   textfont="{{ Font.Bold }}"
                   textsize="{{ Theme.UI_FontSize * 3.75 }}"
                   halign="center"
            />

            <label pos="0 -6.25"
                   text="ROUND"
                   textcolor="333"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize }}"
                   halign="center"
            />
        </frame>
    </template>
</component>