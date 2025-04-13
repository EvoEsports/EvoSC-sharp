<component>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="double" name="width" default="12.0"/>
    <property type="double" name="height" default="4.5"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="-10.3"/>
    <property type="string" name="halign" default="left"/>
    <property type="string" name="color" default="Theme.UI_BgPrimary"/>
    <property type="int" name="gained" default="0"/>

    <template>
        <frame pos='{{ x + (halign=="right" ? width/-2.0+0.3 : width/2.0) }} {{ y }}'>
            <Rectangle x="{{ width / -2.0 }}"
                       width="{{ width - 0.3 }}"
                       height="{{ height }}"
                       bgColor="{{ color }}"
                       cornerRadius="0.75"
                       corners='{{ halign=="right" ? "TopRight,BottomRight,BottomLeft" : "TopLeft,BottomLeft,BottomRight" }}'
            />
            
            <label pos='{{ halign=="right" ? 0 : -0.15 }} {{ height / -2.0 + 0.25 }}'
                   text="{{ gained }}"
                   textprefix="{{ gained >= 0 ? '+' : '-' }}"
                   textcolor="{{ Theme.TeamInfoModule_Widget_GainedPoints_Text }}"
                   textfont="{{ Font.Bold }}"
                   textsize="{{ Theme.TeamInfoModule_Widget_GainedPoints_Text_Size }}"
                   halign="center"
                   valign="center"
            />
        </frame>
    </template>
</component>