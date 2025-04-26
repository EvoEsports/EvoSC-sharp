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
                  bgcolor="{{ Theme.TeamInfoModule_Widget_MatchPointBox_Accent }}"
                  opacity="{{ Theme.TeamInfoModule_Widget_MatchPointBox_Accent_Opacity }}"
            />
            <quad pos='{{ (halign=="right" ? 0 : 0.7) }} 0'
                  size="{{ width - 0.7 }} {{ height }}"
                  bgcolor="{{ Theme.TeamInfoModule_Widget_MatchPointBox_Bg }}"
                  opacity="{{ Theme.TeamInfoModule_Widget_MatchPointBox_Bg_Opacity }}"
            />
            <label pos='{{ (halign=="right" ? width-2 : 2) }} {{ height / -2.0 + 0.25 }}'
                   text="{{ text.ToUpper() }}"
                   class="text-primary"
                   textcolor="{{ Theme.TeamInfoModule_Widget_MatchPointBox_Text }}"
                   valign="center"
                   halign="{{ halign }}"
            />
        </frame>
    </template>
</component>