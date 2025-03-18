<component>
    <import component="EvoSC.Drawing.Rectangle" as="Rectangle"/>

    <property type="double" name="width" default="44.0"/>
    <property type="double" name="height" default="4.5"/>
    <property type="double" name="x" default="0.0"/>
    <property type="double" name="y" default="0.0"/>
    <property type="string" name="text"/>

    <template>
        <frame pos="{{ x }} {{ y }}">
            <Rectangle x="{{ width / -2.0 }}"
                       width="{{ width }}"
                       height="{{ height }}"
                       bgColor="{{ Theme.TeamInfoModule_Widget_BottomInfo_Bg }}"
                       cornerRadius="0.75"
                       corners="BottomLeft,BottomRight"
            />
            
            <Label pos="0 {{ height / -2.0 + 0.25 }}"
                   size="{{ width -2.0 }} {{ height - 1.0 }}"
                   text="{{ text }}"
                   class="text-primary"
                   textcolor="{{ Theme.TeamInfoModule_Widget_BottomInfo_Text }}"
                   halign="center"
                   valign="center"
            />
        </frame>
    </template>
</component>