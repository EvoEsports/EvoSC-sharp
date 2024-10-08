<component>
    <property type="string" name="id" default="evosc_points_box"/>
    <property type="double" name="width" default="12.0"/>
    <property type="double" name="height" default="10.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="halign" default="left"/>
    <property type="string" name="color" default="111"/>
    <property type="int" name="points" default="-1"/>

    <template>
        <frame pos='{{ x - (halign=="right" ? width : 0) }} {{ y }}'>
            <quad size="{{ width }} {{ height }}"
                  bgcolor="{{ color }}"
            />

            <label id="{{ id }}"
                   pos="{{ width / 2.0 }} {{ height / -2.0 + 0.5 }}"
                   class="text-3xl"
                   text="{{ points }}"
                   textcolor="{{ Theme.UI_TextPrimary }}"
                   textfont="{{ Font.Bold }}"
                   valign="center"
                   halign="center"
            />
        </frame>
    </template>
</component>