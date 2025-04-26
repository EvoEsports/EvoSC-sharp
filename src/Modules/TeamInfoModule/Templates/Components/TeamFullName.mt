<component>
    <using namespace="GbxRemoteNet.Structs"/>

    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="TmTeamInfo" name="teamInfo"/>
    <property type="double" name="width" default="45.0"/>
    <property type="double" name="height" default="10.0"/>
    <property type="double" name="barHeight" default="1.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="halign" default="left"/>

    <template>
        <frame pos='{{ x - (halign=="right" ? width : 0) }} {{ y }}'>
            <Rectangle width="{{ width }}"
                       height="{{ height }}"
                       bgColor="{{ teamInfo.RGB }}"
            />
            <quad pos='{{ halign=="right" ? 0 : width }} {{ halign=="right" ? -height : -barHeight }}'
                  rot='{{ halign=="right" ? -90 : 90 }}'
                  size="{{ height - barHeight }} {{ width }}"
                  image="file:///Media/Painter/Stencils/04-SquareGradient/Brush.tga"
                  modulatecolor="000000"
                  opacity="0.5"
            />
            <label pos='{{ width/2.0 }} {{ height / -2.0 }}'
                   size="{{ width - 4.0 }} {{ height - barHeight - 2.0 }}"
                   text="{{ teamInfo.Name.ToUpper() }}"
                   class="text-xl"
                   textcolor="{{ Theme.TeamInfoModule_Widget_TeamFullName_Text }}"
                   textfont="{{ Font.Bold }}"
                   halign="center"
                   valign="center"
            />
        </frame>
    </template>
</component>