<component>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="double" name="width" default="13.0"/>
    <property type="double" name="height" default="10.0"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="halign" default="left"/>

    <template>
        <frame pos='{{ x - (halign=="right" ? width : 0) }} {{ y }}'>
            <Rectangle width="{{ width }}"
                       height="{{ height }}"
                       bgColor="{{ Theme.UI_BgPrimary }}"
                       cornerRadius="1.0"
            />
        </frame>
    </template>
</component>