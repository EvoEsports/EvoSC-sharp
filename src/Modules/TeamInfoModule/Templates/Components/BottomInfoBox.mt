<component>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="double" name="width" default="44.0"/>
    <property type="double" name="height" default="10.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="text"/>

    <template>
        <frame pos="{{ x }} {{ y }}">
            <Rectangle x="{{ width / -2.0 }}"
                       y="{{ height / 2.0 }}"
                       width="{{ width }}"
                       height="{{ height }}"
                       bgColor="{{ Theme.UI_BgPrimary }}"
                       cornerRadius="1.0"
            />
            
            <label pos="0 {{ height / -4.0 + 0.5 }}"
                   text="{{ text }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   textfont="{{ Font.Regular }}"
                   textsize="{{ Theme.UI_FontSize }}"
                   halign="center"
                   valign="center"
            />
        </frame>
    </template>
</component>